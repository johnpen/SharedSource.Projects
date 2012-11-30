using Sitecore.Configuration;
using Sitecore.Data.Items;
using Sitecore.SecurityModel;
using Sitecore.Workflows;
using Sitecore.Workflows.Simple;


namespace SharedSource.Projects.WorkflowActions
{
    class CheckItemProjectStatus
    {
        Sitecore.Data.Database master = Factory.GetDatabase("master");

        public void Process(WorkflowPipelineArgs args)
        {       
            // Item being workflowed
            Item workflowItem = args.DataItem;
            IWorkflow wf = this.master.WorkflowProvider.GetWorkflow(workflowItem);

            
            if (workflowItem.IsProjectItem())
            {
                // Move item to the project workflow   
                using (new SecurityDisabler())
                {
                    workflowItem.Editing.BeginEdit();
                    workflowItem["__Workflow"] = Data.ProjectWorkflow.ToString(); 
                    workflowItem["__Workflow state"] = Data.ProjectWorkflowReadytoGoLiveState.ToString();
                    workflowItem["OrigionalWorkflow"] = wf.WorkflowID;
                    workflowItem.Editing.EndEdit();
                }

                wf = this.master.WorkflowProvider.GetWorkflow(Data.ProjectWorkflow.ToString());
                wf.Start(workflowItem);
            }
            else
            {
                // get current state
                var state = wf.GetState(workflowItem);
                var cmds = wf.GetCommands(state.StateID);

                foreach(var cmd in cmds)
                {
                    if (cmd.SuppressComment)
                    {
                        wf.Execute(cmd.CommandID, workflowItem, "", false);
                        return;
                    }
                }
            }            
            return; 
        }

    }
}
