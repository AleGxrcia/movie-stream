using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using MovieStream.Core.Application.Interfaces.Services;

namespace MovieStream.Core.Application.Services
{
    public class FileManagerService : IFileManagerService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public FileManagerService(IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor)
        {
            _webHostEnvironment = webHostEnvironment;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<string> UploadFileAsync(IFormFile file, int id, string entityType, string existingImagePath = "")
        {
            if (file == null)
            {
                return existingImagePath;
            }

            var request = _httpContextAccessor.HttpContext!.Request!;
            var baseUrl = $"{request.Scheme}://{request.Host}";

            string storagePath = Path.Combine(_webHostEnvironment.WebRootPath, "Images", entityType, id.ToString());
            string relativePath = $"/Images/{entityType}/{id}";

            if (!Directory.Exists(storagePath))
            {
                Directory.CreateDirectory(storagePath);
            }

            string fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            string newFilePath = Path.Combine(storagePath, fileName);

            using var stream = new FileStream(newFilePath, FileMode.Create);
            await file.CopyToAsync(stream);

            if (!string.IsNullOrEmpty(existingImagePath))
            {
                try
                {
                    string oldImageName = Path.GetFileName(existingImagePath);
                    string oldImagePath = Path.Combine(storagePath, oldImageName);
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }
                catch
                {
                    
                }
            }

            return $"{baseUrl}{relativePath}/{fileName}";
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
