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
    public class UserContribRepository : IUserContibRepository
    {
        private readonly IDbConnection _connection;

        public UserContribRepository()
        {
            _connection = new SqlConnection("Data Source=localhost,1433;Initial Catalog=eCommerce;User ID=sa;Password=#Br@sil10;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        }

        public async Task<List<Usuario>> Get()
        {
            var result = await _connection.GetAllAsync<Usuario>();

            return result.ToList();
        }

        public async Task<Usuario> Get(int id)
        {
            var result = await _connection.GetAsync<Usuario>(id);

            return result;
        }

        public async Task<Usuario> Insert(Usuario usuario)
        {
            var id = await _connection.InsertAsync(usuario);

            usuario.Id = id;

            return usuario;
        }

        public async Task<Usuario> Update(Usuario usuario)
        {
            await _connection.UpdateAsync(usuario);

            return usuario;
        }

        public async Task<bool> Delete(int id)
        {
            var usuario = await Get(id);

            if (usuario == null)
                return false;

            await _connection.DeleteAsync(usuario);

            return true;
        }
    }
}
