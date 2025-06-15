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
        [HttpPost("uploadProfilePicture")]
        public async Task<IActionResult> UploadImage(IFormFile file)
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

            if (!file.ContentType.StartsWith("image/"))
            {
                return BadRequest(new ResponseModel<string>
                {
                    Success = false,
                    Message = "Uploaded file is not an image."
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

            if (!string.IsNullOrEmpty(user.ProfilePictureUrl))
            {
                return BadRequest(new ResponseModel<string>
                {
                    Success = false,
                    Message = "Profile picture is already uploaded."
                });
            }

            string folderName = "ProfilePictures";
            string filePath = await _uploadImageHelper.UploadImageAsync(file, folderName);

            user.ProfilePictureUrl = filePath;
            await _context.SaveChangesAsync();

            return Ok(new ResponseModel<string>
            {
                Success = true,
                Message = "Profile picture uploaded successfully.",
                Data = filePath
            });
        }




        [Authorize]
        [HttpPost("replaceProfilePicture")]
        public async Task<IActionResult> ReplaceProfilePicture(IFormFile newFile)
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

            if (newFile == null || newFile.Length == 0)
            {
                return BadRequest(new ResponseModel<string>
                {
                    Success = false,
                    Message = "No new file uploaded."
                });
            }

            if (!newFile.ContentType.StartsWith("image/"))
            {
                return BadRequest(new ResponseModel<string>
                {
                    Success = false,
                    Message = "Uploaded file is not an image."
                });
            }

            // Fetch user
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return NotFound(new ResponseModel<string>
                {
                    Success = false,
                    Message = "User not found."
                });
            }
            if(user.ProfilePictureUrl==null)
                return BadRequest(new ResponseModel<string>
                {
                    Success = false,
                    Message = "No previous profile picture found to replace."

                });
            string folderName = "ProfilePictures";

            // Replace image file
            var newFilePath = await _uploadImageHelper.ReplaceImageAsync(folderName, user.ProfilePictureUrl, newFile);
            if (newFilePath == null)
            {
                return NotFound(new ResponseModel<string>
                {
                    Success = false,
                    Message = "Original image not found for replacement."
                });
            }

            // Update user record
            user.ProfilePictureUrl = newFilePath;
            await _context.SaveChangesAsync();

            return Ok(new ResponseModel<string>
            {
                Success = true,
                Message = "Profile picture replaced successfully.",
                Data = newFilePath
            });
        }

    }
}
