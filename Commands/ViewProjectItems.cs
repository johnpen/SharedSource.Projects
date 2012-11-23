using System.Collections.Specialized;
using Sitecore;
using Sitecore.Shell.Framework.Commands;
using Sitecore.Text;
using Sitecore.Web.UI.Sheer;

namespace SharedSource.Projects.Commands
{
    class ViewProjectItems : Sitecore.Shell.Framework.Commands.Command
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


        protected void Run(ClientPipelineArgs args)
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
                UrlString str = new UrlString(UIUtil.GetUri("control:ProjectItems"));
                str["id"] = args.Parameters["id"];
                str["la"] = args.Parameters["Language"];
                str["vs"] = args.Parameters["Version"];
                SheerResponse.ShowModalDialog(str.ToString());
                args.WaitForPostBack(true);
            }
        }

        public override CommandState QueryState(CommandContext context)
        {
            if (context.Items[0].IsProjectItem() || context.Items[0].TemplateID == Data.ProjectDetails)
            {
                return CommandState.Enabled;
            }
            else
            {
                return CommandState.Disabled;
            }
        }
    }
}
