using Microsoft.AspNetCore.Http;

namespace MovieStream.Core.Application.Interfaces.Services
{
    public interface IFileManagerService
    {
        Task<string> UploadFileAsync(IFormFile file, int id, string entityType, string existingImagePath = "");
        void DeleteFile(int id, string entityType = "");
    }
}
