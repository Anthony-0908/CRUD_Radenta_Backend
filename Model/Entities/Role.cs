namespace CRUD_Radenta.Model.Entities
{
    public class Role
    {
        public Guid Id { get; set; }

        public required string Rolename { get; set; }

        public string RoleDescription { get; set; }
    }
}
