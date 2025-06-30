using Mero_Doctor_Project.Data;
using Mero_Doctor_Project.DTOs.DoctorDto;
using Mero_Doctor_Project.Models.Common;
using Mero_Doctor_Project.Models;
using Mero_Doctor_Project.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Mero_Doctor_Project.Repositories
{
    public class RatingReviewRepository : IRatingReviewRepository
    {
        private readonly ApplicationDbContext _context;
        public RatingReviewRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<ResponseModel<string>> CreateRatingAsync(RatingReviewCreateDto dto, string userId)
        {
            var existing = await _context.RatingReviews
                .FirstOrDefaultAsync(r => r.UserId == userId && r.DoctorId == dto.DoctorId);
            var doctor=await _context.Doctors.FindAsync(dto.DoctorId);
            if (existing != null)
            {
                return new ResponseModel<string>
                {
                    Success = false,
                    Message = "You have already rated this doctor. Please update instead."
                };
            }

            var entity = new RatingReview
            {
                UserId = userId,
                DoctorId = dto.DoctorId,
                Rating = dto.Rating,
                Review = dto.Review,
                CreatedDate = DateTime.UtcNow
            };

            _context.RatingReviews.Add(entity);
            await _context.SaveChangesAsync();

            return new ResponseModel<string> { Success = true, Message = "Rating added successfully.",Data= doctor.UserId };
        }

        public async Task<ResponseModel<string>> UpdateRatingAsync(RatingReviewUpdateDto dto, string userId)
        {
            var entity = await _context.RatingReviews
                .FirstOrDefaultAsync(r => r.RatingReviewId == dto.RatingReviewId && r.UserId == userId);

            if (entity == null)
            {
                return new ResponseModel<string> { Success = false, Message = "Rating not found or unauthorized." };
            }

            entity.Rating = dto.Rating;
            entity.Review = dto.Review;

            await _context.SaveChangesAsync();

            return new ResponseModel<string> { Success = true, Message = "Rating updated successfully." };
        }

        public async Task<ResponseModel<RatingReviewGetDto>> GetUserRatingForDoctorAsync(int doctorId, string userId)
        {
            var review = await _context.RatingReviews
                .Include(r => r.Doctor)
                .Include(r => r.User)
                .Where(r => r.DoctorId == doctorId && r.UserId == userId)
                .Select(r => new RatingReviewGetDto
                {
                    RatingReviewId = r.RatingReviewId,
                    DoctorId = r.DoctorId,
                    DoctorName = r.Doctor.User.FullName,
                    UserName = r.User.FullName,
                    Rating = r.Rating,
                    Review = r.Review,
                    CreatedDate = r.CreatedDate
                }).FirstOrDefaultAsync();

            if (review==null)
            {
                return new ResponseModel<RatingReviewGetDto> { Success = false, Message = "No rating found." };
            }

            return new ResponseModel<RatingReviewGetDto> { Success = true, Message = "Ratings found.", Data = review };
        }

        public async Task<ResponseModel<DoctorRatingSummaryDto>> GetAllRatingsForDoctorAsync(int doctorId)
        {
            var reviews = await _context.RatingReviews
                .Include(r => r.User)
                .Include(r => r.Doctor)
                .Where(r => r.DoctorId == doctorId)
                .Select(r => new RatingReviewGetDto
                {
                    RatingReviewId = r.RatingReviewId,
                    DoctorId = r.DoctorId,
                    DoctorName = r.Doctor.User.FullName,
                    UserName = r.User.FullName,
                    Rating = r.Rating,
                    Review = r.Review,
                    CreatedDate = r.CreatedDate
                })
                .ToListAsync();

            int average = reviews.Any() ? (int)Math.Round(reviews.Average(r => r.Rating)) : 0;

            var responseData = new DoctorRatingSummaryDto
            {
                AverageRating = average,
                Reviews = reviews
            };

            return new ResponseModel<DoctorRatingSummaryDto>
            {
                Success = true,
                Data = responseData
            };
        }


    }

}
