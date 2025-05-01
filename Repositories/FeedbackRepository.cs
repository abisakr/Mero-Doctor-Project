using Mero_Doctor_Project.Data;
using Mero_Doctor_Project.Repositories.Interfaces;

namespace Mero_Doctor_Project.Repositories
{
    public class FeedbackRepository : IFeedbackRepository
    {
        private readonly ApplicationDbContext _context;
        public FeedbackRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        // Implement methods for feedback management here
    }
   
}
