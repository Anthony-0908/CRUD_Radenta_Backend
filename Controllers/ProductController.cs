using CRUD_Radenta.Data;
using CRUD_Radenta.Model;
using CRUD_Radenta.Model.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace CRUD_Radenta.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;

        public ProductController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }


        [HttpGet]
        public IActionResult GetAllProducts()
        {
            var allProducts = dbContext.Products.ToList();
            return Ok(allProducts);
        }

        [HttpGet]
        [Route("{id:guid}")]

        public IActionResult GetProudctById(Guid Id)
        {
            var product = dbContext.Products.Find(Id);
            if(product is null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        [HttpPost]
        [HttpPost]
        public IActionResult AddProduct(AddProductDto addProductDto)
        {
            if (addProductDto == null)
            {
                return BadRequest("Product data is required.");
            }

            // Validate required fields
            if (string.IsNullOrWhiteSpace(addProductDto.ProductName) ||
                string.IsNullOrWhiteSpace(addProductDto.ProductDescription) ||
                string.IsNullOrWhiteSpace(addProductDto.ProductCategory))
            {
                return BadRequest("Product name, description, and category are required.");
            }

            var productEntity = new Product
            {
                ProductName = addProductDto.ProductName,
                ProductDescription = addProductDto.ProductDescription,
                ProductCategory = addProductDto.ProductCategory,
            };

            try
            {
                dbContext.Products.Add(productEntity);
                dbContext.SaveChanges();
                return Ok(productEntity);
            }
            catch (DbUpdateException ex)
            {
                // Log the inner exception
                var innerExceptionMessage = ex.InnerException?.Message ?? "Unknown error.";
                return BadRequest($"Error saving to the database: {innerExceptionMessage}");
            }
        }



        [HttpPut]
        [Route("{id:guid}")]

        public IActionResult UpdateProduct(Guid Id, UpdateProdcutDto updateProductDto)
        {
            var product = dbContext.Products.Find(Id);

            if(product is null)
            {
                return NotFound();  
            }

            product.ProductDescription = updateProductDto.ProductDescription;   
            product.ProductName = updateProductDto.ProductName;
            product.ProductCategory = updateProductDto.ProductCategory;
            
            dbContext.SaveChanges();

            return Ok();
        }


        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id)
        {
            var product = dbContext.Products.Find(id);  // Use id directly, since it's an int

            if (product == null)
            {
                return NotFound();
            }

            dbContext.Products.Remove(product);
            dbContext.SaveChanges();

            return Ok();
        }




    }
}
