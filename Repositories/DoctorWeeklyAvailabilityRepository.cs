using Mero_Doctor_Project.Data;
using Mero_Doctor_Project.Models.Common;
using Mero_Doctor_Project.Models;
using Mero_Doctor_Project.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Mero_Doctor_Project.Repositories
{
    public class DoctorWeeklyAvailabilityRepository : IDoctorWeeklyAvailabilityRepository
    {
        private readonly ApplicationDbContext _context;
        public DoctorWeeklyAvailabilityRepository(ApplicationDbContext context)
        {
            _context = context;
        }

    }
}
