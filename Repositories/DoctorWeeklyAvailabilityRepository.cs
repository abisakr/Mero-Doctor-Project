using Mero_Doctor_Project.Data;
using Mero_Doctor_Project.Repositories.Interfaces;

namespace Mero_Doctor_Project.Repositories
{
    public class DoctorWeeklyAvailabilityRepository : IDoctorWeeklyAvailabilityRepository
    {
        private readonly ApplicationDbContext _context;
        public DoctorWeeklyAvailabilityRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        // Implement methods for doctor weekly availability management here
    }
  
}
