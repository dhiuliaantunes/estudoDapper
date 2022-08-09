namespace eCommerce.Api.Models
{
    public class Contato : Base
    {
        public int UsuarioId { get; set; }
        public string Telefone { get; set; }
        public string Celular { get; set; }
        public Usuario Usuario { get; set; }
    }
}
