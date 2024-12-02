using System.Collections.Generic;
using Webshop.Domain.Common;

namespace Webshop.Search.Domain.AggregateRoots
{
    public class SearchCategory : AggregateRoot
    {
        public SearchCategory(string name, int? parentId = null)
        {
            Name = name;
            ParentId = parentId;
            ChildCategories = new List<SearchCategory>();
        }

        public SearchCategory() { } // For ORM
        public string Name { get; private set; }
        public string Description { get; set; }

        /// <summary>
        /// Parent category ID if this category is a subcategory.
        /// </summary>
        public int? ParentId { get; private set; }

        /// <summary>
        /// List of subcategories for this category.
        /// </summary>
        public IEnumerable<SearchCategory> ChildCategories { get; private set; }
    }
}
