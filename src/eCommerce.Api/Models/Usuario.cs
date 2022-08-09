using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using TableAttribute = Dapper.Contrib.Extensions.TableAttribute;

namespace eCommerce.Api.Models
{
    [Table("Usuarios")]
    public class Usuario : Base
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Sexo { get; set; }
        public string RG { get; set; }
        public string CPF { get; set; }
        public string NomeMae { get; set; }
        public string SituacaoCadastro { get; set; }
        public DateTimeOffset DataCadastro { get; set; }

        [Write(false)]
        public Contato Contato { get; set; }

        [Write(false)]
        public ICollection<EnderecoEntrega> EnderecosEntrega { get; set; }

        [Write(false)]
        public ICollection<Departamento> Departamentos { get; set; }
    }
}
