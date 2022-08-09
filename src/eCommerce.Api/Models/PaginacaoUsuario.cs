using System.Collections.Generic;

namespace eCommerce.Api.Models
{
    public class PaginacaoUsuario
    {
        public IEnumerable<Usuario> Items { get; set; }
        public int Total { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public double TotalPages { get; set; }
    }
}
