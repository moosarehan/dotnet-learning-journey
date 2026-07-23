using System.ComponentModel.DataAnnotations;

namespace myfirstrestapi.Dto
{
    public class UserDto
    {
       
        public Guid id { get; set; }
        public string? name { get; set; }
        public string? email { get; set; }
        public string? password { get; set; }
    }
}
