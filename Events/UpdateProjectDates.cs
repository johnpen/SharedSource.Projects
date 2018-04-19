using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Events;
using Sitecore.SecurityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedSource.Projects.Events
{
    public class UpdateProjectDates
    {
        public string Database
        {
            get;
            set;
        }

        public void OnItemSaved(object sender, EventArgs args)
        {
            Assert.ArgumentNotNull(sender, "sender");
            Assert.ArgumentNotNull(args, "args");
            Item savedItem = Event.ExtractParameter(args, 0) as Item;

            if (savedItem != null && savedItem.Database.Name.ToLower() == "master")
            {
                if (savedItem.TemplateID == Data.ProjectDetails)
                {
                    var projectDate = savedItem[Data.ProjectDetailsReleaseDate];
                    //Get all project Items
                    var projectItems = savedItem.Database.SelectItems("fast:/sitecore/content//*[@Project = '" + savedItem.ID + "']");

                    foreach (var item in projectItems)
                    {
                        using (new SecurityDisabler())
                        {
                            item.Editing.BeginEdit();
                            item.Fields[Sitecore.FieldIDs.ValidFrom].Value = projectDate;
                            item.Editing.EndEdit();
                        }
                    }

                    projectItems = savedItem.Database.SelectItems("fast:/sitecore/media library/images//*[@Project = '" + savedItem.ID + "']");

                    foreach (var item in projectItems)
                    {
                        using (new SecurityDisabler())
                        {
                            item.Editing.BeginEdit();
                            item.Fields[Sitecore.FieldIDs.ValidFrom].Value = projectDate;
                            item.Editing.EndEdit();
                        }
                    }
                }
                else
                {
                    if (savedItem.Fields.Contains(Data.ProjectFieldId))
                    {
                        if (string.IsNullOrEmpty(savedItem.Fields[Sitecore.FieldIDs.ValidFrom].Value))
                        {
                            var id = savedItem.Fields[Data.ProjectFieldId].Value;
                            if (!string.IsNullOrEmpty(id))
                            {
                                var projectItem = savedItem.Database.GetItem(new ID(id));
                                using (new SecurityDisabler())
                                {
                                    savedItem.Editing.BeginEdit();
                                    savedItem.Fields[Sitecore.FieldIDs.ValidFrom].Value = projectItem[Data.ProjectDetailsReleaseDate];
                                    savedItem.Editing.EndEdit();
                                }
                            }
                        }
                    }
                }
            }
        }



    }
}
