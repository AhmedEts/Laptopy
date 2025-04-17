using Laptopy.DTOs.Request;
using Laptopy.DTOs.Response;
using Laptopy.Models;
using Laptopy.Repositories.IRepositories;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Laptopy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        public ProductController(IProductRepository productRepository)
        {
            this._productRepository = productRepository;
        }
        [HttpGet("")]
        public IActionResult GetAll()
        {
            var products = _productRepository.Get();
            return Ok(products.Adapt<IEnumerable<ProductResponse>>());
        }

        [HttpGet("{id}")]
        public IActionResult GetOne([FromRoute] int id)
        {
            var product = _productRepository.GetOne(e => e.Id == id, includes: [e => e.ProductImages]);
            if (product != null)
            {
                return Ok(product.Adapt<ProductResponse>());

            }
            return NotFound();
        }



        [HttpPost("")]
        public IActionResult Create([FromForm] ProductRequest productRequest)
        {
            // Check if the files are empty or null
            if (productRequest.File != null)
            {
                ModelState.AddModelError("File", "No files uploaded.");
                return BadRequest(ModelState);
            }

            var product = new Product
            {
                Name = productRequest.Name,
                Description = productRequest.Description,
                Price = productRequest.Price,
                Discount = productRequest.Discount,
                Model = productRequest.Model,
                CategoryID = productRequest.CategoryID,
                ProductImages = new List<ProductImages>()
            };

            // Continue processing if files exist
            foreach (var item in productRequest.File)
            {
                // Save the image file
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(item.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "images", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    item.CopyTo(stream);
                }

                product.ProductImages.Add(new ProductImages
                {
                    ImageUrl = fileName
                });
            }

            var productDB = _productRepository.Create(product);
            _productRepository.Comitt(); 

            return CreatedAtAction(nameof(GetOne), new { id = productDB.Id }, productDB);
        }


        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            var product = _productRepository.GetOne(e => e.Id == id);
            if (product != null)
            {
                // Delete old images from wwwroot
                if (product.ProductImages != null && product.ProductImages.Count > 0)
                {
                    foreach (var productImage in product.ProductImages)
                    {
                        var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "images", productImage.ImageUrl);

                        // Check if file exists before deleting
                        if (System.IO.File.Exists(oldPath))
                        {
                            System.IO.File.Delete(oldPath);
                        }
                    }
                }

                // Delete product from the database
                _productRepository.Delete(product);
                return NoContent();
            }

            return NotFound();
        }



        [HttpGet("search")]
        public IActionResult search([FromQuery] string? name, [FromQuery] string? category, [FromQuery] decimal? price, [FromQuery] string? model)
        {
            var product = _productRepository.Get();
            if (name != null || category != null || price != 0)
            {
                product = product.Where(e => e.Name.Contains(name)
                || e.Category.Name.Contains(category) || e.Price == price || e.Model.Contains(model)
                );
            }
            return Ok(product);
        }
    }
}
