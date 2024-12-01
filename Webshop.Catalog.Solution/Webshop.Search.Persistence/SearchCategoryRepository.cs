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

        public async Task<Category> GetCategoryByIdAsync(int id)
        {
            var query = @"SELECT Id, Name, Description, ParentId 
                          FROM Categories 
                          WHERE Id = @Id";

            return await _dbConnection.QuerySingleOrDefaultAsync<Category>(query, new { Id = id });
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            var query = @"SELECT Id, Name, Description, ParentId 
                          FROM Categories";

            return await _dbConnection.QueryAsync<Category>(query);
        }

        public async Task<IEnumerable<Category>> GetChildCategoriesAsync(int parentId)
        {
            var query = @"SELECT Id, Name, Description, ParentId 
                          FROM Categories 
                          WHERE ParentId = @ParentId";

            return await _dbConnection.QueryAsync<Category>(query, new { ParentId = parentId });
        }

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
