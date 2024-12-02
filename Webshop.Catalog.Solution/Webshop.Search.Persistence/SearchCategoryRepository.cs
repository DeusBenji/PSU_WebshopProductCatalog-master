using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Webshop.Search.Application.Contracts.Persistence;
using Webshop.Search.Domain.AggregateRoots;

namespace Webshop.Search.Persistence
{
    /// <summary>
    /// Implementering af repository for søgekategorier.
    /// </summary>
    public class SearchCategoryRepository : ISearchCategoryRepository
    {
        private readonly IDbConnection _dbConnection;

        public SearchCategoryRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        /// <summary>
        /// Henter en kategori baseret på dens ID.
        /// </summary>
        public async Task<SearchCategory> GetCategoryByIdAsync(int id)
        {
            var query = @"SELECT Id, Name, Description, ParentId 
                          FROM Categories 
                          WHERE Id = @Id";

            return await _dbConnection.QuerySingleOrDefaultAsync<SearchCategory>(query, new { Id = id });
        }

        /// <summary>
        /// Henter alle kategorier.
        /// </summary>
        public async Task<IEnumerable<SearchCategory>> GetAllCategoriesAsync()
        {
            var query = @"SELECT Id, Name, Description, ParentId 
                          FROM Categories";

            return await _dbConnection.QueryAsync<SearchCategory>(query);
        }

        /// <summary>
        /// Henter alle underkategorier for en given kategori.
        /// </summary>
        public async Task<IEnumerable<SearchCategory>> GetChildCategoriesAsync(int parentId)
        {
            var query = @"SELECT Id, Name, Description, ParentId 
                          FROM Categories 
                          WHERE ParentId = @ParentId";

            return await _dbConnection.QueryAsync<SearchCategory>(query, new { ParentId = parentId });
        }

        /// <summary>
        /// Tjekker, om en kategori med det angivne parentId findes.
        /// </summary>
        public async Task<bool> ExistsCategoryAsync(int parentId)
        {
            var query = @"SELECT COUNT(1) 
                          FROM Categories 
                          WHERE ParentId = @ParentId";

            var count = await _dbConnection.ExecuteScalarAsync<int>(query, new { ParentId = parentId });
            return count > 0;
        }
    }
}
