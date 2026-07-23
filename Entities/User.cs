
using System.ComponentModel.DataAnnotations;
namespace myfirstrestapi.Entities
{
    public class User
    {
        [Key]
        public Guid id { get; set; } = Guid.NewGuid();
        public string? name { get; set; }
        public string?email { get; set; }
        public string? password { get; set; }



    }
}
