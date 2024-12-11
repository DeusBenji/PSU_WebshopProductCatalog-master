using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Webshop.Search.Application.Contracts.Persistence;
using Webshop.Search.Domain.AggregateRoots;
using Webshop.Data.Persistence;

namespace Webshop.Search.Persistence
{
    public class SearchCategoryRepository : BaseRepository, ISearchCategoryRepository
    {
        public SearchCategoryRepository(DataContext context) : base(TableNames.Search.CATEGORYTABLE, context) { }

        public async Task<SearchCategory> GetCategoryByIdAsync(int id)
        {
            using (var connection = dataContext.CreateConnection())
            {
                string query = $"SELECT Id, Name, Description, ParentId FROM {TableName} WHERE Id = @id";
                return await connection.QuerySingleOrDefaultAsync<SearchCategory>(query, new { id });
            }
        }

        public async Task<IEnumerable<SearchCategory>> GetAllCategoriesAsync()
        {
            using (var connection = dataContext.CreateConnection())
            {
                string query = $"SELECT Id, Name, Description, ParentId FROM {TableName}";
                return await connection.QueryAsync<SearchCategory>(query);
            }
        }

        public async Task<IEnumerable<SearchCategory>> GetChildCategoriesAsync(int parentId)
        {
            using (var connection = dataContext.CreateConnection())
            {
                string query = $"SELECT Id, Name, Description, ParentId FROM {TableName} WHERE ParentId = @parentId";
                return await connection.QueryAsync<SearchCategory>(query, new { parentId });
            }
        }

        public async Task<bool> ExistsCategoryAsync(int parentId)
        {
            using (var connection = dataContext.CreateConnection())
            {
                string query = $"SELECT COUNT(1) FROM {TableName} WHERE ParentId = @parentId";
                var count = await connection.ExecuteScalarAsync<int>(query, new { parentId });
                return count > 0;
            }
        }

        public async Task CreateAsync(SearchCategory category)
        {
            using (var connection = dataContext.CreateConnection())
            {
                string command = $"INSERT INTO {TableName} (Name, ParentId, Description) VALUES (@Name, @ParentId, @Description)";
                await connection.ExecuteAsync(command, new { category.Name, category.ParentId, category.Description });
            }
        }

        public async Task UpdateAsync(SearchCategory category)
        {
            using (var connection = dataContext.CreateConnection())
            {
                string command = $"UPDATE {TableName} SET Name = @Name, Description = @Description WHERE Id = @Id";
                await connection.ExecuteAsync(command, new { category.Name, category.Description, category.Id });
            }
        }

        public async Task DeleteAsync(int id)
        {
            using (var connection = dataContext.CreateConnection())
            {
                string command = $"DELETE FROM {TableName} WHERE Id = @id";
                await connection.ExecuteAsync(command, new { id });
            }
        }
    }
}
