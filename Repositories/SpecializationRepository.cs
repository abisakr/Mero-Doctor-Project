using Mero_Doctor_Project.Data;
using Mero_Doctor_Project.Repositories.Interfaces;

namespace Mero_Doctor_Project.Repositories
{
    public class SpecializationRepository : ISpecializationRepository
    {
        private readonly ApplicationDbContext _context;
        public SpecializationRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        // Implement methods for specialization management here
    }
    
}
