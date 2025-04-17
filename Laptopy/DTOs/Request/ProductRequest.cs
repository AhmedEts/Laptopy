using Laptopy.Models;

namespace Laptopy.DTOs.Request
{
    public class ProductRequest
    {
       
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public string Description { get; set; } = string.Empty;
         public string Model { get; set; } = string.Empty;
        public int CategoryID { get; set; }
        public List<IFormFile> File { get; set; } = new();


    }
}
