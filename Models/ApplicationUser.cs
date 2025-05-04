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

        public ICollection<BlogComment> BlogComments { get; set; } // Navigation property
        public ICollection<Doctor> Doctors { get; set; } // Navigation property
        public ICollection<Patient> Patients { get; set; } // Navigation property
        public ICollection<Like> Likes { get; set; } // Navigation property
        public ICollection<RatingReview> RatingReviews { get; set; } // Navigation property

    }
}
