using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Webshop.Search.Application.Contracts.Persistence;
using Webshop.Search.Domain.AggregateRoots;

namespace Webshop.Search.Persistence
{
    /// <summary>
    /// Implementering af repository for søgeprodukter.
    /// </summary>
    public class SearchProductRepository : ISearchProductRepository
    {
        private readonly IDbConnection _dbConnection;

        public SearchProductRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        /// <summary>
        /// Søger efter produkter baseret på forskellige kriterier.
        /// </summary>
        public async Task<IEnumerable<SearchProduct>> SearchProductsAsync(string query, int? categoryId, decimal? minPrice, decimal? maxPrice)
        {
            var sql = @"SELECT Id, Name, Description, Price, Currency 
                        FROM Products
                        WHERE (@Query IS NULL OR Name LIKE @Query)
                          AND (@CategoryId IS NULL OR CategoryId = @CategoryId)
                          AND (@MinPrice IS NULL OR Price >= @MinPrice)
                          AND (@MaxPrice IS NULL OR Price <= @MaxPrice)";

            return await _dbConnection.QueryAsync<SearchProduct>(sql, new
            {
                Query = query != null ? $"%{query}%" : null,
                CategoryId = categoryId,
                MinPrice = minPrice,
                MaxPrice = maxPrice
            });
        }

        /// <summary>
        /// Henter et produkt baseret på dets ID.
        /// </summary>
        public async Task<SearchProduct> GetProductByIdAsync(int productId)
        {
            var sql = @"SELECT Id, Name, Description, Price, Currency 
                        FROM Products
                        WHERE Id = @ProductId";

            return await _dbConnection.QuerySingleOrDefaultAsync<SearchProduct>(sql, new { ProductId = productId });
        }

        /// <summary>
        /// Henter alle produkter.
        /// </summary>
        public async Task<IEnumerable<SearchProduct>> GetAllProductsAsync()
        {
            var sql = @"SELECT Id, Name, Description, Price, Currency 
                        FROM Products";

            return await _dbConnection.QueryAsync<SearchProduct>(sql);
        }
    }
}
