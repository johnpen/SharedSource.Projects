namespace SharedSource.Projects.WorkflowActions 
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using Sitecore;
    using Sitecore.Data;
    using Sitecore.Data.Items;
    using Sitecore.Diagnostics;
    using Sitecore.Shell.Applications.ContentEditor.Gutters;
    using Sitecore.Shell.Applications.WebEdit;
    using Sitecore.Text;
    using Sitecore.Web;
    using Sitecore.Web.UI.Sheer;

    public class UpdateGutterText : WorkflowState
    {
        protected override GutterIconDescriptor GetIconDescriptor(Item item)
        {
            GutterIconDescriptor iconDescriptor = base.GetIconDescriptor(item);
            if (item.IsProjectItem())
            {
                string iconPath = "Applications/32x32/folder_document.png";
                GutterIconDescriptor gutterIcon = new GutterIconDescriptor()
                {
                    Icon = iconPath,
                    Tooltip = "Item belongs to '" + item.ProjectTitle() + "' project"
                };

                return gutterIcon;
            }

            return null;
        }
    }
}
