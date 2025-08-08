using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using DevSkill.Inventory.Domain.Repositories;
using Microsoft.Extensions.Configuration;

namespace DevSkill.Inventory.Infrastructure.Repositories
{
    public class S3Repository : IS3Repository
    {
        private readonly IConfiguration _configuration;
        private readonly string _bucketName;
        private readonly IAmazonS3 _s3Client;

        public S3Repository(IConfiguration configuration)
        {
            _configuration = configuration;
            _bucketName = _configuration["AWS:BucketName"];
            _s3Client = new AmazonS3Client(
                _configuration["AWS:AccessKey"],
                _configuration["AWS:SecretKey"],
                RegionEndpoint.GetBySystemName(_configuration["AWS:Region"])
            );
        }

        public string GeneratePreSignedURL(string key)
        {
            var request = new GetPreSignedUrlRequest
            {
                BucketName = _bucketName,
                Key = key,
                Expires = DateTime.UtcNow.AddHours(12)
            };

            return _s3Client.GetPreSignedURL(request);
        }
    }

}
