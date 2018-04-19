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
    }
}
