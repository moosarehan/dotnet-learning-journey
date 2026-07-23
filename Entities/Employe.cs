using System.ComponentModel.DataAnnotations;

namespace myfirstrestapi.Entities
{
    public class Employe
    {
        [Key]
        public Guid id { get; set; } = Guid.NewGuid();
        public string? name { get; set; }
        public DateTime? CreatedDate { get; set; }
        
        public DateTime? modifieddate { get; set; }
        public DateOnly? DOB { get; set; }
        public string? EmailAddress { get; set; }
        public string? position { get; set; }
        public string? department { get; set; }
    }
}
