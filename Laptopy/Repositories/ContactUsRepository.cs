using Laptopy.Data;
using Laptopy.Models;
using Laptopy.Repositories.IRepositories;

namespace Laptopy.Repositories
{
    public class ContactUsRepository:Repository<ContactUs>,IContactUsRepository
    {
        private readonly ApplicationDbContext dbContext;
        public ContactUsRepository(ApplicationDbContext dbContext):base(dbContext)
        {
            this.dbContext = dbContext;
        }
    }
}
