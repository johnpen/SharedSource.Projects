using System;
using System.Collections;
using System.Linq;
using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Globalization;
using Sitecore.Publishing;
using Sitecore.SecurityModel;
using Sitecore.Workflows;
using Sitecore.Workflows.Simple;

namespace SharedSource.Projects.WorkflowActions
{
    class CheckProjectItems
    {
        bool AllItemsReady = true;
        Sitecore.Data.Database master = Factory.GetDatabase("master");
       

        public void Process(WorkflowPipelineArgs args)
        {
            Item workflowItem = args.DataItem;
            IWorkflow itemwf = this.master.WorkflowProvider.GetWorkflow(workflowItem);

            // Only run on project workflow
            if (itemwf.WorkflowID != Data.ProjectWorkflow.ToString())
            {
                return;
            }

            // Find other items that are in the same project
            var items = master.SelectItems("fast:/sitecore/content//*[@Project = '" + workflowItem.Fields[Data.ProjectFieldId].Value + "']");

            // Get the project defintion
            var projectDefinition = master.GetItem(workflowItem.Fields[Data.ProjectFieldId].Value);
            if (projectDefinition == null) { return; }
            DateField projectReleaseDate = projectDefinition.Fields[Data.ProjectDetailsReleaseDate];            

            // If there are no other items in the project, and it's after the project release date, continue the workflow.
            if (items == null && projectReleaseDate.DateTime < DateTime.Now)
            {
                // no other items using this so, send back
                RevertItemsWorkflowToOrigional(workflowItem);
                AutoPublish(args);
            }
            else
            {
                foreach (var item in items)
                {
                    // is the item in the same project
                    if (item.Fields[Data.ProjectFieldId].Value == workflowItem.Fields[Data.ProjectFieldId].Value)
                    {
                        // is it not in the waiting state
                        if (item.Fields["__Workflow state"].Value != Data.ProjectWorkflowReadytoGoLiveState.ToString())
                        {
                            if (item.ID != workflowItem.ID) // ignore if same item
                            {
                                AllItemsReady = false;
                                break;
                            }
                        }
                    }
                }

                if (AllItemsReady && projectReleaseDate.DateTime < DateTime.Now)
                {
                    foreach (var item in items)
                    {
                        // is the item in the same project
                        if (item.Fields[Data.ProjectFieldId].Value == workflowItem.Fields[Data.ProjectFieldId].Value)
                        {
                            RevertItemsWorkflowToOrigional(item);
                        }
                    }

                    AutoPublish(args);
                }

            }
    
        }


        private bool InheritsFromTemplate(TemplateItem templateItem, Sitecore.Data.ID templateId)
        {
            return templateItem.ID == templateId
                || (templateItem.BaseTemplates != null
                    && templateItem.BaseTemplates
                        .Where(baseTempl => InheritsFromTemplate(baseTempl, templateId)).Count() > 0);
        }

        private void RevertItemsWorkflowToOrigional(Item item)
        {
            var wf = master.WorkflowProvider.GetWorkflow(item.Fields["OrigionalWorkflow"].Value);
            string finalWfState = string.Empty;

            foreach (var state in wf.GetStates())
            {
                if (state.FinalState)
                {
                    finalWfState = state.StateID;
                    break;
                }
            }

            using (new SecurityDisabler())
            {
                item.Editing.BeginEdit();
                item["__Workflow"] = item.Fields["OrigionalWorkflow"].Value;
                item["__Workflow state"] = finalWfState;
                item.Editing.EndEdit();
            }
        }

        private bool GetDeep(Item actionItem)
        {
            return ((actionItem["deep"] == "1"));
        }

        private Database[] GetTargets(Item item)
        {
            using (new SecurityDisabler())
            {
                Item item2 = item.Database.Items["/sitecore/system/publishing targets"];
                if (item2 != null)
                {
                    ArrayList list = new ArrayList();
                    foreach (Item item3 in item2.Children)
                    {
                        string name = item3["Target database"];
                        if (name.Length > 0)
                        {
                            Database database = Factory.GetDatabase(name, false);
                            if (database != null)
                            {
                                list.Add(database);
                            }
                            else
                            {
                                Log.Warn("Unknown database in PublishAction: " + name, this);
                            }
                        }
                    }
                    return (list.ToArray(typeof(Database)) as Database[]);
                }
            }
            return new Database[0];
        }

        public void AutoPublish(WorkflowPipelineArgs args)
        {
            Item dataItem = args.DataItem;
            Item innerItem = args.ProcessorItem.InnerItem;
            Database[] targets = this.GetTargets(dataItem);
            PublishManager.PublishItem(dataItem, targets, new Language[] { dataItem.Language }, this.GetDeep(innerItem), false);
        }

 
    }
}
