using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Sitecore;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Shell.Applications.WebEdit;
using Sitecore.Shell.Framework.Commands;
using Sitecore.Text;
using Sitecore.Web;
using Sitecore.Web.UI.Sheer;

namespace SharedSource.Projects.Commands
{
    class AddItemtoProject : Command
    {

        public override void Execute(CommandContext context)
        {
            ClientPipelineArgs clientPipelineArg = new ClientPipelineArgs(context.Parameters);
            clientPipelineArg.Parameters.Add("uri", context.Items[0].Uri.ToString());
            Context.ClientPage.Start(this, "Run", clientPipelineArg);
        }

        public override CommandState QueryState(CommandContext context)
        {

            if (context.Items[0].Fields.Contains(Data.ProjectFieldId))
            {
                return CommandState.Enabled;
            }
            else
            {
                return CommandState.Disabled;
            }
        }

        public override string GetHeader(CommandContext context, string header)
        {
            if (context.Items[0].IsProjectItem())
            {
                return "Edit Project";
            }
            else
            {
                return "Set Item Project";
            }
        }


        public void Run(ClientPipelineArgs args)
        {
            Item item = Database.GetItem(ItemUri.Parse(args.Parameters["uri"]));
            bool isPostBack = args.IsPostBack;

            if (isPostBack)
            {
                isPostBack = !args.HasResult;
                if (!isPostBack)
                {
                   
                    UrlHandle urlHandle = UrlHandle.Get(new UrlString(string.Concat("hdl=", args.Result)), "hdl", true);
                    string str = urlHandle["fields"];
                    FieldDescriptor fieldDescriptor = FieldDescriptor.Parse(str);
                    string value = fieldDescriptor.Value; 
                    item.Editing.BeginEdit();
                    item.Fields[Data.ProjectFieldId].Value = value;
                    item.Editing.EndEdit();

                    String refresh = String.Format("item:refreshchildren(id={0})", item.Parent.ID);
                    Sitecore.Context.ClientPage.SendMessage(this, refresh);
                }
            }
            else
            {
                SheerResponse.ShowModalDialog(GetOptions(args, args.Parameters).ToUrlString().ToString(), "720", "320", string.Empty, true);
                args.WaitForPostBack();
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
