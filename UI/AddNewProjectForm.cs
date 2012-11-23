using System;
using Sitecore;
using Sitecore.Configuration;
using Sitecore.Data.Items;
using Sitecore.Web.UI.HtmlControls;
using Sitecore.Web.UI.Pages;
using Sitecore.Web.UI.Sheer;
using Sitecore.Web.UI.WebControls;

namespace SharedSource.Projects.UI
{
    class AddNewProjectForm : DialogForm
    {
        Item item;
        Sitecore.Data.Database master;
        protected TreeviewEx Treeview;
        protected DatePicker StartDatePicker;
        protected TimePicker StartTimePicker;
        protected Edit ProjectName;
        protected Memo ProjectDescription;


        protected override void OnLoad(EventArgs e)
        {
            master = Factory.GetDatabase("master");

            if (!Context.ClientPage.IsEvent)
            {
                Treeview.SetSelectedItem(master.GetItem(Data.ProjectRootFolder));

            }
            base.OnLoad(e);
        }

        protected override void OnCancel(object sender, EventArgs args)
        {
            SheerResponse.CloseWindow();
        }


        protected override void OnOK(object sender, EventArgs args)
        {
            Item selectionItem = this.Treeview.GetSelectionItem();
            
            if(selectionItem!=null)
            {
                using(new Sitecore.SecurityModel.SecurityDisabler())
                {
                    TemplateItem template = master.GetTemplate(Data.ProjectDetails);
                    Item newProject = selectionItem.Add(ProjectName.Value, template);

                    newProject.Editing.BeginEdit();
                    newProject.Fields[Data.ProjectDetailsName].Value = ProjectName.Value;
                    newProject.Fields[Data.ProjectDetailsDescription].Value = ProjectDescription.Value;

                    if (! string.IsNullOrEmpty(StartDatePicker.Value))
                    {
                        DateTime projDate = DateTime.Parse(Sitecore.DateUtil.FormatIsoDate(StartDatePicker.Value, "yyyy-MM-dd") + " " + StartTimePicker.Value);
                        newProject.Fields[Data.ProjectDetailsReleaseDate].Value = Sitecore.DateUtil.ToIsoDate(projDate);
                    }

                    newProject.Editing.EndEdit();
                }
            }
            base.OnOK(sender, args);
        }


        protected void SelectTreeNode()
        {
            Item selectionItem = this.Treeview.GetSelectionItem();
            if (selectionItem != null)
            {
                if (selectionItem.TemplateID != Data.ProjectFolder)
                {
                    Treeview.SetSelectedItem(master.GetItem(Data.ProjectRootFolder));
                }
            }
        }


    }
}
