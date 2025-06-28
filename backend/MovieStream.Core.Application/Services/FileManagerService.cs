using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using MovieStream.Core.Application.Interfaces.Services;

namespace MovieStream.Core.Application.Services
{
    public class FileManagerService : IFileManagerService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public FileManagerService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<string> UploadFileAsync(IFormFile file, int id, string entityType, string existingImagePath = "")
        {
            if (file == null)
            {
                return existingImagePath;
            }

            string storagePath = Path.Combine(_webHostEnvironment.WebRootPath, "Images", entityType, id.ToString());
            string relativePath = $"/Images/{entityType}/{id}";

            if (!Directory.Exists(storagePath))
            {
                Directory.CreateDirectory(storagePath);
            }

            string fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            string newFilePath = Path.Combine(storagePath, fileName);

            using (var stream = new FileStream(newFilePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            if (!string.IsNullOrEmpty(existingImagePath))
            {
                string oldImagePath = Path.Combine(storagePath, Path.GetFileName(existingImagePath));
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }

            return $"{relativePath}/{fileName}";
        }

        public void DeleteFile(int id, string entityType = "")
        {
            string path = Path.Combine(_webHostEnvironment.WebRootPath, "Images", entityType, id.ToString());

            if (Directory.Exists(path))
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(path);
                foreach (FileInfo file in directoryInfo.GetFiles())
                {
                    file.Delete();
                }

                foreach (DirectoryInfo folder in directoryInfo.GetDirectories())
                {
                    folder.Delete(true);
                }

                Directory.Delete(path);
            }
        }
    }
}
