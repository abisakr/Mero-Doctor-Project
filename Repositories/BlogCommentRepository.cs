using Mero_Doctor_Project.Data;
using Mero_Doctor_Project.Repositories.Interfaces;

namespace Mero_Doctor_Project.Repositories
{
    public class BlogCommentRepository : IBlogCommentRepository
    {
        private readonly ApplicationDbContext _context;
        public BlogCommentRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        // Implement methods for blog comment management here
    }
    
}
