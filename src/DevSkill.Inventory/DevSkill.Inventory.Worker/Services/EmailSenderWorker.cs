using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.SQS;
using Amazon.SQS.Model;
using DevSkill.Inventory.Domain.Entities;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace DevSkill.Inventory.Worker.Services
{
    public class EmailSenderWorker : BackgroundService
    {
        private readonly ILogger<EmailSenderWorker> _logger;
        private readonly IConfiguration _configuration;
        private readonly IAmazonSQS _sqsClient;
        private readonly string _emailQueueUrl;
        private readonly IEmailSender _emailSender;
        private readonly string _processorName = "TanvirSakil";

        public EmailSenderWorker(
            ILogger<EmailSenderWorker> logger,
            IConfiguration configuration,
            IAmazonSQS sqsClient,
            IEmailSender emailSender)
        {
            _logger = logger;
            _configuration = configuration;
            _sqsClient = sqsClient;
            _emailSender = emailSender;
            _emailQueueUrl = _configuration["AWS:EmailSQSQueueUrl"];
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var response = await _sqsClient.ReceiveMessageAsync(new ReceiveMessageRequest
                {
                    QueueUrl = _emailQueueUrl,
                    MaxNumberOfMessages = 5,
                    WaitTimeSeconds = 10
                }, stoppingToken);

                if (response.Messages.Count == 0)
                {
                    await Task.Delay(5000, stoppingToken);
                    continue;
                }

                foreach (var message in response.Messages)
                {
                    try
                    {
                        var emailMsg = JsonConvert.DeserializeObject<EmailMessage>(message.Body);

                        if (emailMsg?.ProcessorName != _processorName)
                        {
                            _logger.LogInformation($"Skipping message for different processor: {emailMsg?.ProcessorName}");
                            continue;
                        }

                        await _emailSender.SendEmailAsync(emailMsg.ToEmail, emailMsg.Subject, emailMsg.Body);

                        await _sqsClient.DeleteMessageAsync(_emailQueueUrl, message.ReceiptHandle);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to process email message");
                        
                    }
                }
            }
        }
    }

}
