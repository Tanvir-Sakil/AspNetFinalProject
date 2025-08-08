using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Domain.Utilities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace DevSkill.Inventory.Infrastructure.Utilities
{
    //public class FileUploader : Domain.Utilities.IFileUploader
    //{
    //    private readonly IWebHostEnvironment _env;

    //    public FileUploader(IWebHostEnvironment env)
    //    {
    //        _env = env;
    //    }

    //    public async Task<string> UploadAsync(IFormFile file, string folder)
    //    {
    //        var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads", folder);
    //        if (!Directory.Exists(uploadsFolder))
    //            Directory.CreateDirectory(uploadsFolder);

    //        var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
    //        var filePath = Path.Combine(uploadsFolder, fileName);

    //        using var stream = new FileStream(filePath, FileMode.Create);
    //        await file.CopyToAsync(stream);

    //        return $"/uploads/{folder}/{fileName}";
    //    }
    //}
    public class LocalFileUploader : IFileUploader
    {
        private readonly IWebHostEnvironment _env;

        public LocalFileUploader(IWebHostEnvironment env)
        {
            _env = env;
        }

        public async Task<string> UploadAsync(byte[] content, string fileName, string folder)
        {
            var uploadsRoot = Path.Combine(_env.WebRootPath, folder);
            if (!Directory.Exists(uploadsRoot))
                Directory.CreateDirectory(uploadsRoot);

            var filePath = Path.Combine(uploadsRoot, fileName);
            await File.WriteAllBytesAsync(filePath, content);

            // Return relative path to store in DB
            return Path.Combine(folder, fileName).Replace("\\", "/");
        }
    }
}
