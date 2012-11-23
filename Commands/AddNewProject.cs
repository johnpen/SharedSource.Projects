using System.Collections.Generic;
using System.Collections.Specialized;
using Sitecore;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Shell.Applications.WebEdit;
using Sitecore.Shell.Framework.Commands;
using Sitecore.Text;
using Sitecore.Web.UI.Sheer;

namespace SharedSource.Projects.Commands
{
    class AddNewProject : Command
    {
        public override void Execute(CommandContext context)
        {
            var item = context.Items[0];
            var parameters = new NameValueCollection();
            parameters["id"] = item.ID.ToString();
            parameters["database"] = item.Database.Name;
            parameters["Language"] = item.Language.ToString();
            parameters["Version"] = item.Version.ToString();
            Sitecore.Context.ClientPage.Start(this, "Run", parameters);
        }

        /// <summary>
        /// Only enable icon when the project folder has been installed
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override CommandState QueryState(CommandContext context)
        {
            // Has the projects folder been installed?
            if (context.Items[0].Database.GetItem(Data.ProjectRootFolder) != null)
            {
                return CommandState.Enabled;
            }
            else
            {
                return CommandState.Disabled;
            }
        }

        public void Run(ClientPipelineArgs args)
        {
            if (args.IsPostBack)
            {
                if (!args.HasResult)
                {
                    return;
                }
            }
            else
            {
                UrlString str = new UrlString(UIUtil.GetUri("control:AddNewProject"));
                str["id"] = args.Parameters["id"];
                str["la"] = args.Parameters["Language"];
                str["vs"] = args.Parameters["Version"];
                SheerResponse.ShowModalDialog(str.ToString(), "550px", "400px", "", true);
                args.WaitForPostBack(true);
            }
        }

        private PageEditFieldEditorOptions GetOptions(ClientPipelineArgs args, NameValueCollection form)
        {
            Item item = Database.GetItem(ItemUri.Parse(args.Parameters["uri"]));
            List<FieldDescriptor> fieldDescriptors = new List<FieldDescriptor>();
            fieldDescriptors.Add(new FieldDescriptor(item, Data.ProjectFieldId.ToString()));
            List<FieldDescriptor> fieldDescriptors1 = fieldDescriptors;
            Assert.IsNotNull(item, "item");
            PageEditFieldEditorOptions pageEditFieldEditorOption = new PageEditFieldEditorOptions(form, fieldDescriptors1);
            pageEditFieldEditorOption.Title = "Select Project";
            pageEditFieldEditorOption.Icon = "";
            pageEditFieldEditorOption.DialogTitle = "Select Project";
            PageEditFieldEditorOptions pageEditFieldEditorOption1 = pageEditFieldEditorOption;
            return pageEditFieldEditorOption1;
        }

    }
}
