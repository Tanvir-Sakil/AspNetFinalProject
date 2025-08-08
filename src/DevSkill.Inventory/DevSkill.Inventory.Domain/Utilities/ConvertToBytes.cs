using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace DevSkill.Inventory.Domain.Utilities
{
    public static class ConvertToBytes
    {
        public static async Task<byte[]> ConvertFileToBytes(IFormFile file)
        {
            if (file == null) return null;
            using var ms = new MemoryStream();
            await file.CopyToAsync(ms);
            return ms.ToArray();
        }
    }
}
