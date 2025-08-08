using Amazon.SQS;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using DevSkill.Inventory.Application.Features.CashAccounts.Handlers;
using DevSkill.Inventory.Application.Features.Categories.Handlers;
using DevSkill.Inventory.Application.Features.Companies.Queries;
using DevSkill.Inventory.Application.Features.Departments.Handlers;
using DevSkill.Inventory.Application.Features.Products.Commands;
using DevSkill.Inventory.Application.Features.Products.Handlers;
using DevSkill.Inventory.Application.Features.Products.Queries;
using DevSkill.Inventory.Application.Features.Sale.Handlers;
using DevSkill.Inventory.Application.Features.Units.Handlers;
using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Infrastructure;
using DevSkill.Inventory.Infrastructure.Extensions;
using DevSkill.Inventory.Infrastructure.Identity;
using DevSkill.Inventory.Web;
using DevSkill.Inventory.Web.Data;
using DevSkill.Inventory.Web.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;
using System.CodeDom;
using System.Reflection;
using Amazon.Extensions.NETCore.Setup;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Hosting;
using DevSkill.Inventory.Infrastructure.Seeds;

var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                    .AddEnvironmentVariables()
                    .AddIniFile("web.env", optional: true, reloadOnChange: true)
                    .Build();

Log.Logger = new LoggerConfiguration()
             .ReadFrom.Configuration(configuration)
             .CreateBootstrapLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
    var migrationAssembly = Assembly.GetExecutingAssembly();

    #region AWS SQS Register
    builder.Services.Configure<AwsSettings>(builder.Configuration.GetSection("AWS"));

    builder.Services.AddSingleton<IAmazonSQS>(sp =>
    {
        var settings = sp.GetRequiredService<IOptions<AwsSettings>>().Value;

        var credentials = new Amazon.Runtime.BasicAWSCredentials(settings.AccessKey, settings.SecretKey);
        var region = Amazon.RegionEndpoint.GetBySystemName(settings.Region);

        return new AmazonSQSClient(credentials, region);
    });
    #endregion

    #region Autofac Configuration
    builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
    builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
    {
        containerBuilder.RegisterModule(new WebModule(connectionString, migrationAssembly?.FullName));
    });
    #endregion

    builder.Services.AddKeyedScoped<IProduct, Product1>("Config1");
    builder.Services.AddKeyedScoped<IProduct, Product2>("Config2");

    builder.Host.UseSerilog((context, lc) =>
    lc.MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Console()
      .WriteTo.MSSqlServer(
          connectionString: context.Configuration.GetConnectionString("DefaultConnection"),
          sinkOptions: new Serilog.Sinks.MSSqlServer.MSSqlServerSinkOptions
          {
              TableName = "Logs",
              AutoCreateSqlTable = false
          })
    .ReadFrom.Configuration(builder.Configuration)
    );

    #region MediatR Configuration
    builder.Services.AddMediatR(cfg =>
    {
        cfg.RegisterServicesFromAssembly(migrationAssembly);
        cfg.RegisterServicesFromAssembly(typeof(ProductAddCommand).Assembly);
        cfg.RegisterServicesFromAssembly(typeof(ProductUpdateCommand).Assembly);
        //  cfg.RegisterServicesFromAssembly(typeof(GetAllCompaniesQueryHandler).Assembly);
        cfg.RegisterServicesFromAssembly(typeof(GetCategoryListQueryHandler).Assembly);
        cfg.RegisterServicesFromAssembly(typeof(GetUnitListQueryHandler).Assembly);
        cfg.RegisterServicesFromAssembly(typeof(GetCashAccountListQueryHandler).Assembly);
        cfg.RegisterServicesFromAssembly(typeof(GetCategoryByIdQueryHandler).Assembly);
        cfg.RegisterServicesFromAssembly(typeof(GetDepartmentByIdQueryHandler).Assembly);
        cfg.RegisterServicesFromAssembly(typeof(GetUnitByIdQueryHandler).Assembly);
        cfg.RegisterServicesFromAssembly(typeof(GetCashAccountByIdQueryHandler).Assembly);
        cfg.RegisterServicesFromAssembly(typeof(SearchProductQueryHandler).Assembly);
        cfg.RegisterServicesFromAssembly(typeof(GetSaleTypesQueryHandler).Assembly);
        cfg.RegisterServicesFromAssembly(typeof(GetProductDetailsQueryHandler).Assembly);
    });
    #endregion

    #region Automapper Configuration
    builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
    #endregion

    builder.Services.AddIdentity();

    #region Authorization Configuration
    builder.Services.AddPolicy();
    #endregion


    builder.Services.AddDbContext<ApplicationDbContext>(options =>
      options.UseSqlServer(connectionString, (x) => x.MigrationsAssembly(migrationAssembly)));
    builder.Services.AddDatabaseDeveloperPageExceptionFilter();

    builder.Services.AddControllersWithViews();
    builder.Services.AddRazorPages();
    // builder.Services.AddAuthorization();

    builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("SmtpSettings"));

    var app = builder.Build();

    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        await ApplicationDbContextSeed.SeedSuperAdminAsync(services);
    }

    if (app.Environment.IsDevelopment())
    {
        app.UseMigrationsEndPoint();
    }
    else
    {
        app.UseExceptionHandler("/Home/Error");
        app.UseHsts();
    }

    app.UseStaticFiles();
    app.UseHttpsRedirection();
    app.UseRouting();
    app.UseAuthentication();
    app.UseAuthorization();




    app.MapStaticAssets();

    //app.MapControllerRoute(
    //name: "admin-clean",
    //pattern: "{controller=Dashboard}/{action=Index}/{id?}",
    //defaults: new { area = "Admin" });

    app.MapControllerRoute(
        name: "areas",
        pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}")
        .WithStaticAssets();

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Account}/{action=Auth}/{id?}")
        .WithStaticAssets();

    app.MapRazorPages()
       .WithStaticAssets();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Applcation crashed");
}
finally
{
    Log.CloseAndFlush();
}
