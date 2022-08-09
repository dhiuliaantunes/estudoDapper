using System.Collections.Generic;

namespace eCommerce.Api.Models
{
    public class Departamento : Base
    {
        public string Nome { get; set; }

        public ICollection<Usuario> Usuarios { get; set; }
    }
}
