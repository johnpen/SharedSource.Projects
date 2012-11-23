using Sitecore.Data.Items;
using Sitecore.Data;
using Sitecore.Pipelines.GetContentEditorWarnings;


namespace SharedSource.Projects.Pipelines
{
    class DisplayProjectContentEditorWarning
    {
        public void Process(GetContentEditorWarningsArgs args)
        {
            GetContentEditorWarningsArgs.ContentEditorWarning contentEditorWarning;

            if (args.Item.IsProjectItem())
            {
                string project = args.Item.ProjectTitle();
                string output = string.Format(" <a href=\"#\" onclick='javascript:return scForm.invoke(\"{0}\")' title=\"View project items\">[ View project items ]</a>", string.Format("Project:ViewItems(Id={0})", args.Item.ID));

                contentEditorWarning = args.Add();
                contentEditorWarning.Title = "Sitecore Project";
                contentEditorWarning.Text = "This item is part of the '" + project + "'  project    " + output;
            }
        }
    }
}
