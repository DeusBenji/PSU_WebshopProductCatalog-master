using System.Collections.Generic;
using System.Threading.Tasks;
using Webshop.Search.Domain.AggregateRoots;

namespace Webshop.Search.Application.Contracts.Persistence
{
    /// <summary>
    /// Repository for søgeprodukter.
    /// </summary>
    public interface ISearchProductRepository
    {
        /// <summary>
        /// Søger efter produkter baseret på forskellige kriterier.
        /// </summary>
        /// <param name="query">Søgetekst.</param>
        /// <param name="categoryId">Kategori-ID (valgfrit).</param>
        /// <param name="minPrice">Minimumspris (valgfrit).</param>
        /// <param name="maxPrice">Maksimumspris (valgfrit).</param>
        /// <returns>Liste over matchende produkter.</returns>
        Task<IEnumerable<Product>> SearchProductsAsync(string query, int? categoryId, decimal? minPrice, decimal? maxPrice);

        /// <summary>
        /// Henter et produkt baseret på dets ID.
        /// </summary>
        /// <param name="productId">Produktets ID.</param>
        /// <returns>Produktobjektet.</returns>
        Task<Product> GetProductByIdAsync(int productId);

        /// <summary>
        /// Henter alle produkter.
        /// </summary>
        /// <returns>Liste over alle produkter.</returns>
        Task<IEnumerable<Product>> GetAllProductsAsync();
    }
}
