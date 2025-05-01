using Mero_Doctor_Project.Data;
using Mero_Doctor_Project.Repositories.Interfaces;

namespace Mero_Doctor_Project.Repositories
{
    public class DoctorWeeklyTimeRangeRepository : IDoctorWeeklyTimeRangeRepository
    {
        private readonly ApplicationDbContext _context;
        public DoctorWeeklyTimeRangeRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        // Implement methods for managing doctor's weekly time ranges here
    }
    
}
