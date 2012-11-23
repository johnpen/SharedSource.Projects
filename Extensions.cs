using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sitecore.Data.Items;
using Sitecore.Data;

namespace SharedSource.Projects
{
    public static class Extensions
    {
        /// <summary>
        /// Extension method to determine if an Item is linked to a project
        /// </summary>
        /// <param name="item">Item</param>
        /// <returns>true if Item is in a project</returns>
        public static bool IsProjectItem(this Item item)
        {
            if (!string.IsNullOrEmpty(item.Fields[Data.ProjectFieldId].Value))
            {
                return true;
            }
            return false;
        }


        /// <summary>
        /// Gets the project title from the project definition
        /// </summary>
        /// <param name="item">Item</param>
        /// <returns>Project title</returns>
        public static string ProjectTitle(this Item item)
        {
            return item.Database.GetItem(new ID(item.Fields[Data.ProjectFieldId].Value)).DisplayName;
        }
    }
}
