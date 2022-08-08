using Dapper;
using eCommerce.Api.Models;
using eCommerce.Api.Repositories.Interfaces;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerce.Api.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IDbConnection _connection;

        public UserRepository()
        {
            _connection = new SqlConnection("Data Source=localhost,1433;Initial Catalog=eCommerce;User ID=sa;Password=#Br@sil10;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        }

        public async Task<List<Usuario>> Get()
        {
            var result = new List<Usuario>();

            var query = "  SELECT * " +
                        "    FROM Usuarios AS u " +
                        "    LEFT JOIN Contatos AS c ON u.Id = c.UsuarioId " +
                        "    LEFT JOIN EnderecosEntrega AS ee ON u.Id = ee.UsuarioId ";

            await _connection.QueryAsync<Usuario, Contato, EnderecoEntrega, Usuario>(query,
                (usuario, contato, enderecoEntrega) =>
                {
                    if (!result.Any(a => a.Id.Equals(usuario.Id)))
                    {
                        usuario.Contato = contato;
                        usuario.EnderecosEntrega = new List<EnderecoEntrega>();

                        result.Add(usuario);
                    }
                    else
                    {
                        usuario = result.FirstOrDefault(a => a.Id == usuario.Id);
                    }

                    usuario.EnderecosEntrega.Add(enderecoEntrega);

                    return usuario;
                });

            return result;
        }

        public async Task<Usuario> Get(int id)
        {
            var result = new List<Usuario>();

            var usuario = await _connection.QueryAsync<Usuario, Contato, EnderecoEntrega, Usuario>
                (
                    "    SELECT * " +
                    "      FROM Usuarios AS u " +
                    "      LEFT JOIN Contatos AS c ON u.Id = c.UsuarioId " +
                    "      LEFT JOIN EnderecosEntrega AS ee ON u.Id = ee.UsuarioId " +
                    "     WHERE 1 = 1 " +
                    "       AND u.Id = @Id ",
                    (usuario, contato, enderecoEntrega) =>
                    {
                        if (!result.Any(u => u.Id.Equals(usuario.Id)))
                        {
                            usuario.Contato = contato;

                            usuario.EnderecosEntrega = new List<EnderecoEntrega>();

                            result.Add(usuario);
                        }
                        else
                        {
                            usuario = result.FirstOrDefault(u => u.Id.Equals(usuario.Id));
                        }

                        usuario.EnderecosEntrega.Add(enderecoEntrega);

                        return usuario;
                    },
                    new { Id = id }
                );

            if (usuario == null)
                return null;

            return usuario.First();
        }

        public async Task<Usuario> GetMultiple(int id)
        {
            var query = "SELECT * FROM Usuarios WHERE Id = @id; " +
                        "SELECT * FROM Contatos WHERE UsuarioId = @id; " +
                        "SELECT * FROM EnderecosEntrega WHERE UsuarioId = @id; ";

            using var multipleResultSet = await _connection.QueryMultipleAsync(query, new { Id = id });

            var usuario = multipleResultSet.Read<Usuario>().SingleOrDefault();
            var contato = multipleResultSet.Read<Contato>().SingleOrDefault();
            var enderecos = multipleResultSet.Read<EnderecoEntrega>().ToList();

            if (usuario != null)
            {
                usuario.Contato = contato;
                usuario.EnderecosEntrega = enderecos;
            }

            return usuario;
        }

        public async Task<Usuario> Insert(Usuario usuario)
        {
            var result = new Usuario();

            _connection.Open();

            var transaction = _connection.BeginTransaction();

            try
            {
                var queryUsuario = "INSERT INTO Usuarios(Nome, Email, Sexo, RG, CPF, NomeMae, SituacaoCadastro, DataCadastro) VALUES (@Nome, @Email, @Sexo, @RG, @CPF, @NomeMae, @SituacaoCadastro, @DataCadastro); " +
                                   "SELECT CAST(SCOPE_IDENTITY() AS INT); ";

                var resultUser = await _connection.QueryAsync<int>(queryUsuario, usuario, transaction);

                usuario.Id = resultUser.Single();

                if (usuario.Contato != null)
                {
                    usuario.Contato.UsuarioId = usuario.Id;

                    var queryContato = "INSERT INTO Contatos(UsuarioId, Telefone, Celular) VALUES(@UsuarioId, @Telefone, @Celular)" +
                                       "SELECT CAST(SCOPE_IDENTITY() AS INT); ";

                    var resultContato = await _connection.QueryAsync<int>(queryContato, usuario.Contato, transaction);

                    usuario.Contato.Id = resultContato.Single();
                }

                if (usuario.EnderecosEntrega != null)
                {
                    foreach (var enderecoEntrega in usuario.EnderecosEntrega)
                    {
                        enderecoEntrega.UsuarioId = usuario.Id;

                        var queryEndereco = "INSERT INTO EnderecosEntrega(UsuarioId, NomeEndereco, CEP, Estado, Cidade, Bairro, Endereco, Numero, Complemento) " +
                                            "VALUES(@UsuarioId, @NomeEndereco, @CEP, @Estado, @Cidade, @Bairro, @Endereco, @Numero, @Complemento); " +
                                            "SELECT CAST(SCOPE_IDENTITY() AS INT); ";

                        var resultEndereco = await _connection.QueryAsync<int>(queryEndereco, enderecoEntrega, transaction);

                        enderecoEntrega.Id = resultEndereco.Single();
                    }
                }

                transaction.Commit();

                result = usuario;
            }
            catch
            {
                transaction.Rollback();
            }
            finally
            {
                _connection.Close();
            }

            return result;
        }

        public async Task<Usuario> Update(Usuario usuario)
        {
            var result = new Usuario();

            _connection.Open();

            var transaction = _connection.BeginTransaction();

            try
            {
                var queryUsuarios = "UPDATE Usuarios SET Nome = @Nome, Email = @Email, Sexo = @Sexo, RG = @Rg, CPF = @CPF, NomeMae = @NomeMae, SituacaoCadastro = @SituacaoCadastro, DataCadastro = @DataCadastro WHERE Id = @Id";

                await _connection.ExecuteAsync(queryUsuarios, usuario, transaction);

                #region CONTATOS

                if (usuario.Contato != null)
                {
                    var queryContatos = "UPDATE Contatos SET Telefone = @Telefone, Celular = @Celular WHERE UsuarioId = @UsuarioId ";

                    await _connection.ExecuteAsync(queryContatos, usuario.Contato, transaction);
                }

                #endregion

                #region ENDERECOS DE ENTREGA

                var sqlExcluirEnderecos = "DELETE FROM EnderecosEntrega WHERE UsuarioId = @Id ";

                await _connection.ExecuteAsync(sqlExcluirEnderecos, usuario, transaction);

                if (usuario.EnderecosEntrega != null)
                {
                    foreach (var enderecoEntrega in usuario.EnderecosEntrega)
                    {
                        enderecoEntrega.UsuarioId = usuario.Id;

                        var queryEndereco = "INSERT INTO EnderecosEntrega(UsuarioId, NomeEndereco, CEP, Estado, Cidade, Bairro, Endereco, Numero, Complemento) " +
                                            "VALUES(@UsuarioId, @NomeEndereco, @CEP, @Estado, @Cidade, @Bairro, @Endereco, @Numero, @Complemento); " +
                                            "SELECT CAST(SCOPE_IDENTITY() AS INT); ";

                        var resultEndereco = await _connection.QueryAsync<int>(queryEndereco, enderecoEntrega, transaction);

                        enderecoEntrega.Id = resultEndereco.Single();
                    }
                }

                #endregion

                transaction.Commit();

                result = usuario;
            }
            catch
            {
                transaction.Rollback();
            }
            finally
            {
                _connection.Close();
            }

            return result;
        }

        public async Task<bool> Delete(int id)
        {
            var usuario = await Get(id);

            if (usuario == null)
                return false;

            await _connection.ExecuteAsync("DELETE FROM Usuarios WHERE Id = @Id", new { Id = id });

            return true;
        }


    }
}
