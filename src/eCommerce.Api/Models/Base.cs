using System.ComponentModel.DataAnnotations;

namespace eCommerce.Api.Models
{
    public class Base
    {
        [Key]
        public int Id { get; set; }
    }
}
