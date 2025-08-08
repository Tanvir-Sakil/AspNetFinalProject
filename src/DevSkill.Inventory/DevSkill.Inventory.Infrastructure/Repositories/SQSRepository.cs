using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon;
using Amazon.SQS;
using Amazon.SQS.Model;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.Repositories;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace DevSkill.Inventory.Infrastructure.Repositories
{
    public class SqsRepository : ISqsRepository
    {
        private readonly IConfiguration _configuration;

        public SqsRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendResizeRequestAsync(string fileName,string uploader,string imageType,string processorName)
        {
            var message = new SqsImageMessage{ 
                FileName = fileName,
                Uploader = uploader,
                ImageType = imageType,
                ProcessorName = processorName   
            };
            var request = new SendMessageRequest
            {
                QueueUrl = _configuration["AWS:SQSQueueUrl"],
                MessageBody = JsonConvert.SerializeObject(message)
            };

            var client = new AmazonSQSClient(
                _configuration["AWS:AccessKey"],
                _configuration["AWS:SecretKey"],
                RegionEndpoint.GetBySystemName(_configuration["AWS:Region"])
            );

            await client.SendMessageAsync(request);
        }
    }

}
