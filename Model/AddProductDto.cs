namespace CRUD_Radenta.Model
{
    public class AddProductDto
    {
       //private int Id { get; set; }

        public  string ProductName { get; set; } = string.Empty;

        public string ProductDescription { get; set; } = string.Empty;

        public string ProductCategory { get; set; } = string.Empty;
    }
}
