using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Mero_Doctor_Project.Models
{
    public class ApplicationUser : IdentityUser
    {
        [StringLength(25)]
        public string FullName { get; set; } 

        [Url]
        public string? ProfilePictureUrl { get; set; }
        [Required]
        public double Latitude { get; set; } // Required for location
        [Required]
        public double Longitude { get; set; } // Required for location
        public ICollection<BlogComment> BlogComments { get; set; } // Navigation property
        public ICollection<Doctor> Doctors { get; set; } // Navigation property
        public ICollection<Patient> Patients { get; set; } // Navigation property
        public ICollection<Like> Likes { get; set; } // Navigation property
        public ICollection<RatingReview> RatingReviews { get; set; } // Navigation property
        public ICollection<XRayRecord> XRayRecords { get; set; }

    }
}
