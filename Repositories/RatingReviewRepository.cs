using Mero_Doctor_Project.Data;
using Mero_Doctor_Project.Repositories.Interfaces;

namespace Mero_Doctor_Project.Repositories
{
    public class RatingReviewRepository : IRatingReviewRepository
    {
        private readonly ApplicationDbContext _context;
        public RatingReviewRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        // Implement methods for rating and review management here
    }
   
}
