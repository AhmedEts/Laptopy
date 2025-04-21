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
            var product = _productRepository.GetOne(e => e.Id == id);
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
            if (productRequest.File != null && productRequest.File.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(productRequest.File.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "images", fileName);

                //if(!System.IO.File.Exists(filePath))
                //{
                //    System.IO.File.Create(filePath);
                //}

                using (var stream = System.IO.File.Create(filePath))
                {
                    productRequest.File.CopyTo(stream);
                }

                // Save img name in db
                Product product = productRequest.Adapt<Product>();
                product.MainImg = fileName;
                var productInDb = _productRepository.Create(product);
                 _productRepository.Comitt();

                return CreatedAtAction(nameof(GetOne), new { id = productInDb.Id }, productRequest);
            }
            ModelStateDictionary keyValuePairs = new();
            keyValuePairs.AddModelError("File", "The file is not found");
            return BadRequest(keyValuePairs);
        }


     

    

        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            var product = _productRepository.GetOne(e => e.Id == id);
            if (product != null)
            {
                // Delete old img from wwwroot
                if (product.MainImg != null)
                {
                    var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "images", product.MainImg);
                    if (System.IO.File.Exists(oldPath))
                    {
                        System.IO.File.Delete(oldPath);
                    }
                }

                // Delete img name in db
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
