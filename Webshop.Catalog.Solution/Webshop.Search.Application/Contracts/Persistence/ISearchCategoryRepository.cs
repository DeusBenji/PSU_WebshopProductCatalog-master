using System.Collections.Generic;
using System.Threading.Tasks;
using Webshop.Search.Domain.AggregateRoots;

namespace Webshop.Search.Application.Contracts.Persistence
{
    /// <summary>
    /// Repository for søgekategorier.
    /// </summary>
    public interface ISearchCategoryRepository
    { 
   Task<SearchCategory> GetCategoryByIdAsync(int id);
    Task<IEnumerable<SearchCategory>> GetAllCategoriesAsync();
    Task<IEnumerable<SearchCategory>> GetChildCategoriesAsync(int parentId);
    Task<bool> ExistsCategoryAsync(int parentId);
    }
}

