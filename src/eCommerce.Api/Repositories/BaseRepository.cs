using Dapper.Contrib.Extensions;
using eCommerce.Api.Models;
using eCommerce.Api.Repositories.Interfaces;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerce.Api.Repositories
{
    public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : Base
    {
        private readonly IDbConnection _connection;
        public BaseRepository()
        {
            _connection = new SqlConnection("Data Source = localhost, 1433; Initial Catalog = dapper; User ID = sa; Password = Password@?2022; Connect Timeout = 30; Encrypt = False; TrustServerCertificate = False; ApplicationIntent = ReadWrite; MultiSubnetFailover = False");
        }

        public async Task<List<TEntity>> Get()
        {
            var result = await _connection.GetAllAsync<TEntity>();

            return result.ToList();
        }

        public async Task<TEntity> Get(int id)
        {
            var result = await _connection.GetAsync<TEntity>(id);

            return result;
        }

        public async Task<TEntity> Insert(TEntity item)
        {
            var id = await _connection.InsertAsync(item);

            item.Id = id;

            return item;
        }

        public async Task<TEntity> Update(TEntity item)
        {
            await _connection.UpdateAsync(item);

            return item;
        }

        public async Task<bool> Delete(int id)
        {
            var item = await Get(id);

            if (item == null)
                return false;

            await _connection.DeleteAsync(item);

            return true;
        }

    }
}
