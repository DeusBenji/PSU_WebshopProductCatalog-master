using Dapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Webshop.Search.Application.Contracts.Persistence;
using Webshop.Search.Domain.AggregateRoots;
using Webshop.Data.Persistence;

namespace Webshop.Search.Persistence
{
    /// <summary>
    /// Implementering af repository for søgeprodukter.
    /// </summary>
    public class SearchProductRepository : BaseRepository, ISearchProductRepository
    {
        public SearchProductRepository(DataContext context) : base(TableNames.Search.PRODUCTTABLE, context) { }

        /// <summary>
        /// Søger efter produkter baseret på forskellige kriterier.
        /// </summary>
        public async Task<IEnumerable<SearchProduct>> SearchProductsAsync(string query, int? categoryId, decimal? minPrice, decimal? maxPrice)
        {
            using (var connection = dataContext.CreateConnection())
            {
                var sql = $@"SELECT Id, Name, Description, Price, Currency
                             FROM {TableName}
                             WHERE (@Query IS NULL OR Name LIKE @Query)
                               AND (@CategoryId IS NULL OR CategoryId = @CategoryId)
                               AND (@MinPrice IS NULL OR Price >= @MinPrice)
                               AND (@MaxPrice IS NULL OR Price <= @MaxPrice)";

                return await connection.QueryAsync<SearchProduct>(sql, new
                {
                    Query = query != null ? $"%{query}%" : null,
                    CategoryId = categoryId,
                    MinPrice = minPrice,
                    MaxPrice = maxPrice
                });
            }
        }

        /// <summary>
        /// Henter et produkt baseret på dets ID.
        /// </summary>
        public async Task<SearchProduct> GetProductByIdAsync(int productId)
        {
            using (var connection = dataContext.CreateConnection())
            {
                var sql = $"SELECT Id, Name, Description, Price, Currency FROM {TableName} WHERE Id = @ProductId";
                return await connection.QuerySingleOrDefaultAsync<SearchProduct>(sql, new { ProductId = productId });
            }
        }

        /// <summary>
        /// Henter alle produkter.
        /// </summary>
        public async Task<IEnumerable<SearchProduct>> GetAllProductsAsync()
        {
            using (var connection = dataContext.CreateConnection())
            {
                var sql = $"SELECT Id, Name, Description, Price, Currency FROM {TableName}";
                return await connection.QueryAsync<SearchProduct>(sql);
            }
        }

        /// <summary>
        /// Opretter et nyt produkt.
        /// </summary>
        public async Task CreateAsync(SearchProduct product)
        {
            using (var connection = dataContext.CreateConnection())
            {
                var sql = $"INSERT INTO {TableName} (Name, Description, Price, Currency, CategoryId) VALUES (@Name, @Description, @Price, @Currency, @CategoryId)";
                await connection.ExecuteAsync(sql, product);
            }
        }

        /// <summary>
        /// Opdaterer et eksisterende produkt.
        /// </summary>
        public async Task UpdateAsync(SearchProduct product)
        {
            using (var connection = dataContext.CreateConnection())
            {
                var sql = $"UPDATE {TableName} SET Name = @Name, Description = @Description, Price = @Price, Currency = @Currency, CategoryId = @CategoryId WHERE Id = @Id";
                await connection.ExecuteAsync(sql, product);
            }
        }

        /// <summary>
        /// Sletter et produkt baseret på dets ID.
        /// </summary>
        public async Task DeleteAsync(int productId)
        {
            using (var connection = dataContext.CreateConnection())
            {
                var sql = $"DELETE FROM {TableName} WHERE Id = @ProductId";
                await connection.ExecuteAsync(sql, new { ProductId = productId });
            }
        }
    }
}
