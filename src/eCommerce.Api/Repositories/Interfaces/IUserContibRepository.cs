using eCommerce.Api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eCommerce.Api.Repositories.Interfaces
{
    public interface IUserContibRepository
    {
        public Task<List<Usuario>> Get();
        public Task<Usuario> Get(int id);
        public Task<Usuario> Insert(Usuario usuario);
        public Task<Usuario> Update(Usuario usuario);
        public Task<bool> Delete(int id);
    }
}
