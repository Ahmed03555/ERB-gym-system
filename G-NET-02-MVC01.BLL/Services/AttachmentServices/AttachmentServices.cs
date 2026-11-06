using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace G_NET_02_MVC01.BLL.Services.AttachmentServices
{
    public class AttachmentServices : IAttachmentServices
    {
        private readonly string[] _allowedExtension = { ".jpg", ".jpeg", ".png" };
        private readonly long _maxSize = 5 * 1024 * 1024; // 5 MB
        private readonly ILogger<AttachmentServices> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AttachmentServices(ILogger<AttachmentServices> logger, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
        }

        public bool Delete(string fileName, string folderName)
        {
            if(string.IsNullOrEmpty(fileName) || string.IsNullOrEmpty(folderName))
            {
                _logger.LogWarning("Invalid file or folder name.");
                return false;
            }

            try
            {
                var filePath = Path.Combine(_webHostEnvironment.ContentRootPath, folderName, fileName);

                if (!File.Exists(filePath))
                {
                    
                    return false;
                }
                File.Delete(filePath);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the file {FileName}.", fileName);
                return false;
            }
        }

        public (Stream Stream, string ContentType)? GetFile(string fileName, string folderName)
        {
            if (string.IsNullOrEmpty(fileName) || string.IsNullOrEmpty(folderName))
            {
                
                return null;
            }
            var filePath = Path.Combine(_webHostEnvironment.ContentRootPath, folderName, fileName); 
            if (!File.Exists(filePath))
            {
                return null;
            }

            var contentType = Path.GetExtension(filePath).ToLowerInvariant() switch
            {
                ".jpg" => "image/jpeg",
                ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                _ => "application/octet-stream",
            };
            var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            return (fileStream, contentType);
        }

        public async Task<string?> UpdateAsync(Stream fileStream, string fileName, string folderName, CancellationToken cancellationToken)
        {
            if (fileStream == null || !fileStream.CanRead)
                return null;
            if (fileStream.Length > _maxSize)
            {
                _logger.LogWarning($"File size exceeds the maximum limit of {_maxSize} bytes.");
                return null;
            }
            if (fileStream.Length == 0)
                return null;

            var fileExtension = Path.GetExtension(fileName);
            if (!_allowedExtension.Contains(fileExtension) || string.IsNullOrEmpty(fileExtension))
            {
                _logger.LogWarning($"File extension {fileExtension} is not allowed.");
                return null;
            }
            var FolderPath = Path.Combine(_webHostEnvironment.ContentRootPath, folderName);
            Directory.CreateDirectory(FolderPath);
            var stordFileName = $"{Guid.NewGuid()}{fileExtension}";
            var filePath = Path.Combine(FolderPath, stordFileName);

            try
            {
                await using var fa = new FileStream(filePath, FileMode.Create,FileAccess.Write, FileShare.None);
                await fileStream.CopyToAsync(fa, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while saving the file.");
                return null;
            }

            return stordFileName;
        }
    }

}
