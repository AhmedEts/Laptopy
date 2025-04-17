using Laptopy.Data;
using Laptopy.Models;
using Laptopy.Repositories.IRepositories;

namespace Laptopy.Repositories
{
    public class CategoryRepository:Repository<Category>,ICategoryRepository
    {
        private readonly ApplicationDbContext dbContext;
        public CategoryRepository(ApplicationDbContext dbContext):base(dbContext)
        {
            this.dbContext = dbContext;
        }
    }
}
