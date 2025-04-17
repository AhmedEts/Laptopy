using Laptopy.Data;
using Laptopy.Models;
using Laptopy.Repositories.IRepositories;

namespace Laptopy.Repositories
{
    public class ProductRepository:Repository<Product>,IProductRepository
    {
        private readonly ApplicationDbContext dbContext;
        public ProductRepository(ApplicationDbContext dbContext):base(dbContext)
        {
            this.dbContext = dbContext;
        }
    }
}
