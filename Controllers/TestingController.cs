using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mero_Doctor_Project.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestingController : ControllerBase
    {
            
        private readonly IWebHostEnvironment _environment;
        public TestingController(IWebHostEnvironment environment)
        {
           _environment = environment;
        }

       

        [HttpPost("upload")]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            // Ensure the file is an image
            if (!file.ContentType.StartsWith("image/"))
                return BadRequest("File is not an image.");

            // Get the path to wwwroot/images
            var uploadsFolder = Path.Combine(_environment.WebRootPath, "images");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            // Generate a unique file name to avoid overwriting
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploadsFolder, fileName);

            // Save the file
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Return the URL to access the image
            var imageUrl = $"/images/{fileName}";
            return Ok(new { url = imageUrl });
        }
       
        [HttpPost("replace/{fileName}")]
        public async Task<IActionResult> ReplaceImage(string fileName, IFormFile newFile)
        {
            if (newFile == null || newFile.Length == 0)
                return BadRequest("No new file uploaded.");

            if (!newFile.ContentType.StartsWith("image/"))
                return BadRequest("File is not an image.");

            var uploadsFolder = Path.Combine(_environment.WebRootPath, "images");
            var existingFilePath = Path.Combine(uploadsFolder, fileName);

            // Check if the existing file exists
            if (!System.IO.File.Exists(existingFilePath))
                return NotFound("Image not found.");

            // Delete the existing file
            System.IO.File.Delete(existingFilePath);

            // Save the new file with the same name
            using (var stream = new FileStream(existingFilePath, FileMode.Create))
            {
                await newFile.CopyToAsync(stream);
            }

            var imageUrl = $"/images/{fileName}";
            return Ok(new { url = imageUrl });
        }
    }
}