using Mero_Doctor_Project.Data;
using Mero_Doctor_Project.Repositories.Interfaces;

namespace Mero_Doctor_Project.Repositories
{
    public class BlogRepository : IBlogRepository
    {
        private readonly ApplicationDbContext _context;
        public BlogRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        // Implement methods for blog management here
    }
 
}
