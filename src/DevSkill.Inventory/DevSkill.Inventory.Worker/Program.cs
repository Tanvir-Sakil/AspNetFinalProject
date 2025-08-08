using Amazon.SQS;
using DevSkill.Inventory.Infrastructure;
using DevSkill.Inventory.Worker;
using DevSkill.Inventory.Worker.Services;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Amazon.Extensions.NETCore.Setup;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false)
    .AddEnvironmentVariables()
    .Build();

var connectionString = configuration.GetConnectionString("DefaultConnection");
var migrationAssemblyName = typeof(Worker).Assembly.FullName;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .ReadFrom.Configuration(configuration)
    .CreateLogger();

try
{
    Log.Information("Application Starting...");

    IHost host = Host.CreateDefaultBuilder(args)
        .UseWindowsService()
        .UseSerilog()
        .ConfigureServices((context, services) =>
        {
            // Register ApplicationDbContext
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString,
                    sql => sql.MigrationsAssembly(migrationAssemblyName)));

            services.Configure<WebPathOptions>(context.Configuration.GetSection("WebPathOptions"));

            services.AddDefaultAWSOptions(context.Configuration.GetAWSOptions());
            services.AddAWSService<IAmazonSQS>();

            services.AddHostedService<ImageResizeWorker>();
            services.AddHostedService<EmailSenderWorker>();
            services.Configure<SmtpSettings>(context.Configuration.GetSection("SmtpSettings"));
            services.AddTransient<IEmailSender, MailtrapEmailSender>();
        })
        .Build();

    await host.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application start up failed");
}
finally
{
    Log.CloseAndFlush();
}
