using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Byway.Application.Services
{
    public interface IFileService
    {
        Task<string> SaveImageAsync(IFormFile image, string folderName);
        void DeleteImage(string imagePath);
    }

    public class FileService : IFileService
    {
        private readonly string _webRootPath;

        public FileService(string webRootPath)
        {
            _webRootPath = webRootPath;
        }

        public async Task<string> SaveImageAsync(IFormFile image, string folderName)
        {
            if (image == null || image.Length == 0)
                return "/images/placeholder.jpg"; // Default placeholder

            // Create unique filename
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(image.FileName)}";
            var folderPath = Path.Combine(_webRootPath, "images", folderName);
            var filePath = Path.Combine(folderPath, fileName);

            // Create directory if it doesn't exist
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            // Save file
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }

            // Return relative path for web access
            return $"/images/{folderName}/{fileName}";
        }

        public void DeleteImage(string imagePath)
        {
            if (string.IsNullOrEmpty(imagePath) || imagePath == "/images/placeholder.jpg")
                return;

            var fullPath = Path.Combine(_webRootPath, imagePath.TrimStart('/'));
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }
    }
}




