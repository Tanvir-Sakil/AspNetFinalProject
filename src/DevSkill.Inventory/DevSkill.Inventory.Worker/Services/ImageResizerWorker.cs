using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.SQS;
using Amazon.SQS.Model;
using Newtonsoft.Json;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Jpeg;
using DevSkill.Inventory.Worker;
using Microsoft.Extensions.Options;
using DevSkill.Inventory.Worker.Services;

public class ImageResizeWorker : BackgroundService
{
    private readonly ILogger<ImageResizeWorker> _logger;
    private readonly IConfiguration _configuration;
    private readonly IHostEnvironment _env;
    private readonly string _processorName;
    private readonly WebPathOptions _webPathOptions;

    public ImageResizeWorker(ILogger<ImageResizeWorker> logger,
        IConfiguration configuration, IHostEnvironment env,
        IOptions<WebPathOptions> webPathOptions)
    {
        _logger = logger;
        _configuration = configuration;
        _env = env;
        _webPathOptions = webPathOptions.Value;
        _processorName = "TanvirSakil"; 
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var region = RegionEndpoint.GetBySystemName(_configuration["AWS:Region"]);
        var sqsClient = new AmazonSQSClient(
            _configuration["AWS:AccessKey"],
            _configuration["AWS:SecretKey"],
            region
        );

        var s3Client = new AmazonS3Client(
            _configuration["AWS:AccessKey"],
            _configuration["AWS:SecretKey"],
            region
        );

        var queueUrl = _configuration["AWS:SQSQueueUrl"];
        var bucketName = "aspnetb11";

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var receiveRequest = new ReceiveMessageRequest
                {
                    QueueUrl = queueUrl,
                    MaxNumberOfMessages = 5,
                    WaitTimeSeconds = 5
                };

                var response = await sqsClient.ReceiveMessageAsync(receiveRequest, stoppingToken);

                if (response.HttpStatusCode != System.Net.HttpStatusCode.OK)
                {
                    _logger.LogError($"Failed to receive messages from SQS. Status: {response.HttpStatusCode}");
                    await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
                    continue;
                }

                if (response.Messages.Count == 0)
                {
                    _logger.LogInformation("No messages received from SQS.");
                }

                foreach (var message in response.Messages)
                {
                    try
                    {
                        ResizeImageMessage msg = null;
                        try
                        {
                            msg = JsonConvert.DeserializeObject<ResizeImageMessage>(message.Body);
                            if (msg == null)
                                throw new Exception("Deserialized message is null.");
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, $"Failed to deserialize SQS message: {message.Body}");
                            await sqsClient.DeleteMessageAsync(queueUrl, message.ReceiptHandle, stoppingToken);
                            continue;
                        }
                        if (msg == null || string.IsNullOrEmpty(msg.FileName) && (msg.ProcessorName == _processorName))
                        {
                            _logger.LogWarning("Deserialized message is null or missing FileName.");
                            await sqsClient.DeleteMessageAsync(queueUrl, message.ReceiptHandle, stoppingToken);
                            continue;
                        }

                        if (msg.ProcessorName != _processorName)
                        {
                            _logger.LogInformation($"Skipping message for different processor: {msg.ProcessorName}");
                            continue;
                        }

                        string localImagePath = Path.Combine(
                                                _webPathOptions.WebRootPath,
                                                "uploads",
                                                msg.FileName.Replace('/', Path.DirectorySeparatorChar));
                        localImagePath = localImagePath.Replace('\\', '/');

                        _logger.LogInformation("Resolved image path: {Path}", localImagePath);


                        if (!File.Exists(localImagePath))
                        {
                            _logger.LogError($"Image file not found on disk: {localImagePath}");
                            await sqsClient.DeleteMessageAsync(queueUrl, message.ReceiptHandle, stoppingToken);
                            continue;
                        }

                        using var image = Image.Load(localImagePath);

                        image.Mutate(x => x.Resize(new ResizeOptions
                        {
                            Size = new Size(300, 300),
                            Mode = ResizeMode.Max
                        }));

                        using var outputStream = new MemoryStream();
                        await image.SaveAsJpegAsync(outputStream, new JpegEncoder(), stoppingToken);
                        outputStream.Position = 0;

                        string s3ResizedKey = $"{_processorName}/resized/resized_{Path.GetFileName(msg.FileName)}";

                        var putRequest = new PutObjectRequest
                        {
                            BucketName = bucketName,
                            Key = s3ResizedKey,
                            InputStream = outputStream,
                            ContentType = "image/jpeg"
                        };

                        var putResponse = await s3Client.PutObjectAsync(putRequest, stoppingToken);
                        if (putResponse.HttpStatusCode != System.Net.HttpStatusCode.OK)
                        {
                            _logger.LogError($"Failed to upload resized image to S3. Key: {s3ResizedKey}, Status: {putResponse.HttpStatusCode}");
                            continue;
                        }

                        _logger.LogInformation($"Successfully uploaded resized image to S3: {s3ResizedKey}");

                        await sqsClient.DeleteMessageAsync(queueUrl, message.ReceiptHandle, stoppingToken);
                        _logger.LogInformation("Deleted processed message from SQS.");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"Unexpected error while processing message: {message.Body}");
                    }
                }
            }
            catch (Exception outerEx)
            {
                _logger.LogError(outerEx, "Critical error in main worker loop.");
            }

            await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
        }
    }

}


