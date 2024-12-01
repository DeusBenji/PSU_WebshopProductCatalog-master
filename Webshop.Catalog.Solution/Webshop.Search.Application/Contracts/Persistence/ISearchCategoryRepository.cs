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
        /// <summary>
        /// Henter en kategori baseret på dens ID.
        /// </summary>
        /// <param name="id">Kategoriens ID.</param>
        /// <returns>Kategoriobjektet.</returns>
        Task<Category> GetCategoryByIdAsync(int id);

        /// <summary>
        /// Henter alle kategorier.
        /// </summary>
        /// <returns>Liste over alle kategorier.</returns>
        Task<IEnumerable<Category>> GetAllCategoriesAsync();

        /// <summary>
        /// Henter alle underkategorier for en given kategori.
        /// </summary>
        /// <param name="parentId">ID for forældrekategorien.</param>
        /// <returns>Liste over underkategorier.</returns>
        Task<IEnumerable<Category>> GetChildCategoriesAsync(int parentId);

        /// <summary>
        /// Tjekker, om en kategori med det angivne parentId findes.
        /// </summary>
        /// <param name="parentId">ID for forældrekategorien.</param>
        /// <returns>True, hvis kategorien findes; ellers false.</returns>
        Task<bool> ExistsCategoryAsync(int parentId);
    }
}
