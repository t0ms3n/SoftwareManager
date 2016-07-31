using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.OData.Query;
using Microsoft.OData.Core.UriParser.Semantic;
using SoftwareManager.WebApi.Extensions;

namespace SoftwareManager.WebApi.Helpers
{
    public static class ODataQueryOptionsExtractor
    {
        /// <summary>
        /// Parses the SelectExpand clauses and returns an array of strings with all found expands in the format Property.SubProperty (etc.)
        /// </summary>
        /// <param name="selectExpandQueryOption"></param>
        /// <returns></returns>
        public static string[] GetNavigationPropertiesToExpand(SelectExpandQueryOption selectExpandQueryOption)
        {
            HashSet<string> membersToExpand = new HashSet<string>();
            if (selectExpandQueryOption != null)
            {
                var selectItems = selectExpandQueryOption.SelectExpandClause.SelectedItems;
                foreach (var item in ParseNavigationProperties(selectItems, string.Empty))
                {
                    membersToExpand.Add(item);
                }
            }
            return membersToExpand.ToArray();
        }

        private static List<string> ParseNavigationProperties(IEnumerable<SelectItem> selectItems, string parentPath)
        {
            List<string> properties = new List<string>();
            foreach (
                var expandClause in

                    selectItems.OfType<ExpandedNavigationSelectItem>())
            {
                var navigationSegment =
                    expandClause.PathToNavigationProperty.FirstSegment as NavigationPropertySegment;
                if (navigationSegment != null)
                {
                    // Fix lowerCamelCase caused by builder.EnableLowerCamelCase()
                    var navigationPropertyName = navigationSegment.NavigationProperty.Name.FirstLetterToUpper();

                    string navigationPropertyPath = string.IsNullOrEmpty(parentPath)
                        ? navigationPropertyName
                        : $"{parentPath}.{navigationPropertyName}";

                    properties.Add(navigationPropertyPath);
                    if (expandClause.SelectAndExpand.SelectedItems.Any())
                    {
                        properties.AddRange(ParseNavigationProperties(expandClause.SelectAndExpand.SelectedItems,
                            navigationPropertyPath));
                    }
                }
            }
            return properties;
        }

        
    }
}