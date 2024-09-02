namespace CRUD_Radenta.Model
{
    public class AddProductDto
    {
        public int Id { get; set; }

        public required string ProductName { get; set; }

        public string ProductDescription { get; set; } = string.Empty;

        public string ProductCategory { get; set; } = string.Empty;
    }
}
