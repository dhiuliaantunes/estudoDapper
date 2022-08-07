using eCommerce.Api.Models;
using eCommerce.Api.Repositories.Interfaces;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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

        public Task<List<Usuario>> Get()
        {
            throw new System.NotImplementedException();
        }

        public Task<Usuario> Get(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task<Usuario> Insert(Usuario user)
        {
            throw new System.NotImplementedException();
        }

        public Task<Usuario> Update(Usuario user)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> Delete(int id)
        {
            throw new System.NotImplementedException();
        }
    }
}
