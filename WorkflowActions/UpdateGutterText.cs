using System;
using System.Collections.Generic;
using Sitecore;
using Sitecore.Data.Items;
using Sitecore.Data;
using Sitecore.Diagnostics;
using Sitecore.Web.UI.Sheer;
using Sitecore.Shell.Applications.WebEdit;
using System.Collections.Specialized;
using Sitecore.Web;
using Sitecore.Text;

using Sitecore.Shell.Applications.ContentEditor.Gutters;

namespace SharedSource.Projects.WorkflowActions 
{
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
