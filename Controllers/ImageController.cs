using System.Security.Claims;
using Mero_Doctor_Project.Data;
using Mero_Doctor_Project.Helper;
using Mero_Doctor_Project.Models.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Mero_Doctor_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IWebHostEnvironment _environment;
        private readonly UploadImageHelper _uploadImageHelper;
        private readonly ApplicationDbContext _context;

        public ImageController(IWebHostEnvironment environment, UploadImageHelper uploadImageHelper,ApplicationDbContext context)
        {
            _environment = environment;
            _uploadImageHelper = uploadImageHelper;
            _context = context;
        }


        [Authorize]
        [HttpPost("uploadOrReplaceProfilePicture")]
        public async Task<IActionResult> UploadOrReplaceProfilePicture(IFormFile file)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new ResponseModel<string>
                {
                    Success = false,
                    Message = "Please login."
                });
            }

            if (file == null || file.Length == 0)
            {
                return BadRequest(new ResponseModel<string>
                {
                    Success = false,
                    Message = "No file uploaded."
                });
            }

            var permittedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (string.IsNullOrEmpty(ext) || !permittedExtensions.Contains(ext))
            {
                return BadRequest(new ResponseModel<string>
                {
                    Success = false,
                    Message = "Uploaded file is not a supported image format."
                });
            }

            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return NotFound(new ResponseModel<string>
                {
                    Success = false,
                    Message = "User not found."
                });
            }

            string folderName = "ProfilePictures";
            string resultPath;

            if (string.IsNullOrEmpty(user.ProfilePictureUrl))
            {
                // Upload new profile picture
                resultPath = await _uploadImageHelper.UploadImageAsync(file, folderName);
                user.ProfilePictureUrl = resultPath;
            }
            else
            {
                // Replace existing profile picture
                resultPath = await _uploadImageHelper.ReplaceImageAsync(folderName, user.ProfilePictureUrl, file);
                if (resultPath == null)
                {
                    return NotFound(new ResponseModel<string>
                    {
                        Success = false,
                        Message = "Original image not found for replacement."
                    });
                }

                user.ProfilePictureUrl = resultPath;
            }

            await _context.SaveChangesAsync();

            return Ok(new ResponseModel<string>
            {
                Success = true,
                Message = string.IsNullOrEmpty(user.ProfilePictureUrl)
                    ? "Profile picture uploaded successfully."
                    : "Profile picture replaced successfully.",
                Data = resultPath
            });
        }

    }
}
