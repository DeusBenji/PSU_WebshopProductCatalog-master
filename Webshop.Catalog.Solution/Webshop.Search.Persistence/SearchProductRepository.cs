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

        public async Task<IEnumerable<Product>> SearchProductsAsync(string query, int? categoryId, decimal? minPrice, decimal? maxPrice)
        {
            var sql = @"SELECT Id, Name, SKU, Price, Currency, Description, CategoryId 
                        FROM Products
                        WHERE (@Query IS NULL OR Name LIKE @Query)
                          AND (@CategoryId IS NULL OR CategoryId = @CategoryId)
                          AND (@MinPrice IS NULL OR Price >= @MinPrice)
                          AND (@MaxPrice IS NULL OR Price <= @MaxPrice)";

            return await _dbConnection.QueryAsync<Product>(sql, new
            {
                Query = query != null ? $"%{query}%" : null,
                CategoryId = categoryId,
                MinPrice = minPrice,
                MaxPrice = maxPrice
            });
        }

        public async Task<Product> GetProductByIdAsync(int productId)
        {
            var sql = @"SELECT Id, Name, SKU, Price, Currency, Description, CategoryId 
                        FROM Products
                        WHERE Id = @ProductId";

            return await _dbConnection.QuerySingleOrDefaultAsync<Product>(sql, new { ProductId = productId });
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            var sql = @"SELECT Id, Name, SKU, Price, Currency, Description, CategoryId 
                        FROM Products";

            return await _dbConnection.QueryAsync<Product>(sql);
        }
    }
}
