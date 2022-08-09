using eCommerce.Api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eCommerce.Api.Repositories.Interfaces
{
    public interface IBaseRepository<TEntity> where TEntity : Base
    {
        public Task<List<TEntity>> Get();
        public Task<TEntity> Get(int id);
        public Task<TEntity> Insert(TEntity item);
        public Task<TEntity> Update(TEntity item);
        public Task<bool> Delete(int id);
    }
}
