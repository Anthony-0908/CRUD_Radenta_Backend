namespace CRUD_Radenta.Model.Entities
{
    public class User
    {
        public Guid Id { get; set; }

        public required string Firstname { get; set; }

        public required string Middlename { get; set; } 
        
        public required string Lastname {  get; set; }  




    }
}
