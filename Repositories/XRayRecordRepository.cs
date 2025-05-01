using Mero_Doctor_Project.Data;
using Mero_Doctor_Project.Repositories.Interfaces;

namespace Mero_Doctor_Project.Repositories
{
    public class XRayRecordRepository : IXRayRecordRepository
    {
        private readonly ApplicationDbContext _context;
        public XRayRecordRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        // Implement methods for X-Ray record management here
    }
    
}
