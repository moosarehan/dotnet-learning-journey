namespace myfirstrestapi.Dto
{
    public class Employedto
    {

        public Guid id { get; set; } 
        public string? name { get; set; }
        public DateTime? CreatedDate { get; set; }

        public DateTime? modifieddate { get; set; }
        public DateOnly? DOB { get; set; }
        public string? EmailAddress { get; set; }
        public string? position { get; set; }
        public string? department { get; set; }
    }
}
