using System.Collections.Generic;

namespace eCommerce.Api.Models
{
    public class Departamento
    {
        public int Id { get; set; }
        public string Nome { get; set; }

        public ICollection<Usuario> Usuarios { get; set; }
    }
}
