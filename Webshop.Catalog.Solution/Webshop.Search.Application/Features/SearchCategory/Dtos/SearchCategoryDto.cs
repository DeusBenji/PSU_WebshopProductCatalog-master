using System.Collections.Generic;


    namespace Webshop.Search.Application.Features.SearchCategory.Dtos
    {
        public class SearchCategoryDto
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public int? ParentId { get; set; }
        }
    }
