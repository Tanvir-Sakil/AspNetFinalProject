using Autofac;
using DevSkill.Inventory.Domain.Repositories;
using DevSkill.Inventory.Domain.Services;
using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Web.Data;
using DevSkill.Inventory.Web.Models;
using DevSkill.Inventory.Application.Services;
using DevSkill.Inventory.Infrastructure;
using DevSkill.Inventory.Application.Features.Products.Commands;
using DevSkill.Inventory.Domain.Utilities;
using DevSkill.Inventory.Infrastructure.Utilities;
using Microsoft.EntityFrameworkCore;
using DevSkill.Inventory.Application.Features.Categories.Handlers;
using MediatR;
using System.Reflection;
using DevSkill.Inventory.Infrastructure.Repositories;
using DevSkill.Inventory.Application.Features.Units.Handlers;
using DevSkill.Inventory.Application.Features.Products.Handlers;
using DevSkill.Inventory.Application.Features.Customers.Handlers;
using DevSkill.Inventory.Application.Features.CashAccounts.Queries;
using DevSkill.Inventory.Application.Features.CashAccounts.Handlers;
using DevSkill.Inventory.Application.Features.Sale.Handlers;
using DevSkill.Inventory.Application.Features.Sale.Commands;
using DevSkill.Inventory.Application.Features.Departments.Handlers;
using DevSkill.Inventory.Application.Features.UserRoles.Handlers;
using DevSkill.Inventory.Application.Features.Account.Handlers;


namespace DevSkill.Inventory.Web
{

    public class WebModule : Autofac.Module
    {
        private readonly string _connectionString;
        private readonly string _migrationAssembly;

        public WebModule(string connectionString, string migrationAssembly)
        {
            _connectionString = connectionString;
            _migrationAssembly = migrationAssembly;
        }
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<Item>().As<IItem>().InstancePerLifetimeScope();
            builder.RegisterType<ApplicationDbContext>().AsSelf()
                .WithParameter("connectionString", _connectionString)
                .WithParameter("migrationAssembly", _migrationAssembly)
                .InstancePerLifetimeScope();

            builder.RegisterType<ApplicationUnitOfWork>().As<IApplicationUnitOfWork>()
            .InstancePerLifetimeScope();

            builder.RegisterType<ProductRepository>().As<IProductRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<CustomerRepository>().As<ICustomerRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<ProductService>().As<IProductService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<CompanyService>().As<ICompanyService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<CategoryRepository>().As<ICategoryRepository>()
                .InstancePerLifetimeScope();
            builder.RegisterType<ExpenseRepository>().As<IExpenseRepository>()
    .InstancePerLifetimeScope();

            builder.RegisterType<UserRoleRepository>().As<IUserRoleRepository>()
    .InstancePerLifetimeScope();

            builder.RegisterType<DepartmentRepository>().As<IDepartmentRepository>()
    .InstancePerLifetimeScope();

            builder.RegisterType<UnitRepository>().As<IUnitRepository>()
         .InstancePerLifetimeScope();

            builder.RegisterType<SaleTypeRepository>().As<ISaleTypeRepository>()
.InstancePerLifetimeScope();


            builder.RegisterType<UserRepository>().As<IUserRepository>()
.InstancePerLifetimeScope();

            builder.RegisterType<CashAccountRepository>().As<ICashAccountRepository>()
         .InstancePerLifetimeScope();

            builder.RegisterType<MobileAccountRepository>().As<IMobileAccountRepository>()
.InstancePerLifetimeScope();

            builder.RegisterType<BankAccountRepository>().As<IBankAccountRepository>()
.InstancePerLifetimeScope();

            builder.RegisterType<SupplierRepository>().As<ISupplierRepository>()
.InstancePerLifetimeScope();

            builder.RegisterType<AccountTypeRepository>().As<IAccountTypeRepository>()
.InstancePerLifetimeScope();

            builder.RegisterType<BalanceTransferRepository>().As<IBalanceTransferRepository>()
.InstancePerLifetimeScope();


            builder.RegisterType<AccountSearchRepository>().As<IAccountSearchRepository>()
.InstancePerLifetimeScope();

            builder.RegisterType<UserPermissionRepository>().As<IUserPermissionRepository>()
.InstancePerLifetimeScope();

            builder.RegisterType<CustomerLedgerRepository>().As<ICustomerLedgerRepository>()
.InstancePerLifetimeScope();

            builder.RegisterType<CompanyProfileRepository>().As<ICompanyProfileRepository>()
.InstancePerLifetimeScope();

            builder.RegisterType<LocalFileUploader>().As<IFileUploader>()
.InstancePerLifetimeScope();



            builder.RegisterType<StaffRepository>().As<IStaffRepository>()
.InstancePerLifetimeScope();


            builder.RegisterType<SqsRepository>().As<ISqsRepository>()
.InstancePerLifetimeScope();

            builder.RegisterType<SalesRepository>().As<ISalesRepository>()
.InstancePerLifetimeScope();

            builder.RegisterType<S3Repository>().As<IS3Repository>()
.InstancePerLifetimeScope();


            builder.RegisterType<ProductAddCommand>().AsSelf();
            builder.RegisterType<SaleAddCommand>().AsSelf();


            builder.RegisterType<ProductUpdateCommand>().AsSelf();


            builder.RegisterAssemblyTypes(typeof(GetCategoryListQueryHandler).Assembly)
              .AsClosedTypesOf(typeof(IRequestHandler<,>))
              .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(typeof(GetUserRoleListQueryHandler).Assembly)
  .AsClosedTypesOf(typeof(IRequestHandler<,>))
  .InstancePerLifetimeScope();


            builder.RegisterAssemblyTypes(typeof(GetDepartmentListQueryHandler).Assembly)
              .AsClosedTypesOf(typeof(IRequestHandler<,>))
              .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(typeof(GetCustomerQueryHandler).Assembly)
   .AsClosedTypesOf(typeof(IRequestHandler<,>))
   .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(typeof(GetCategoryByIdQueryHandler).Assembly)
                .AsClosedTypesOf(typeof(IRequestHandler<,>))
                .InstancePerLifetimeScope();


            builder.RegisterAssemblyTypes(typeof(GetUserRoleByIdQueryHandler).Assembly)
                .AsClosedTypesOf(typeof(IRequestHandler<,>))
                .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(typeof(GetDepartmentByIdQueryHandler).Assembly)
    .AsClosedTypesOf(typeof(IRequestHandler<,>))
    .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(typeof(GetUnitListQueryHandler).Assembly)
  .AsClosedTypesOf(typeof(IRequestHandler<,>))
  .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(typeof(GetCashAccountListQueryHandler).Assembly)
.AsClosedTypesOf(typeof(IRequestHandler<,>))
.InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(typeof(GetSaleTypesQueryHandler).Assembly)
.AsClosedTypesOf(typeof(IRequestHandler<,>))
.InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(typeof(GetAccountTypesQueryHandler).Assembly)
.AsClosedTypesOf(typeof(IRequestHandler<,>))
.InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(typeof(GetUnitByIdQueryHandler).Assembly)
                .AsClosedTypesOf(typeof(IRequestHandler<,>))
                .InstancePerLifetimeScope();



            builder.RegisterAssemblyTypes(typeof(GetCashAccountByIdQueryHandler).Assembly)
    .AsClosedTypesOf(typeof(IRequestHandler<,>))
    .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(typeof(GetProductDetailsQueryHandler).Assembly)
.AsClosedTypesOf(typeof(IRequestHandler<,>))
.InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(typeof(GetSaleTypesQueryHandler).Assembly)
.AsClosedTypesOf(typeof(IRequestHandler<,>))
.InstancePerLifetimeScope();


            builder.RegisterAssemblyTypes(typeof(SearchProductQueryHandler).Assembly)
    .AsClosedTypesOf(typeof(IRequestHandler<,>))
    .InstancePerLifetimeScope();


            builder.RegisterType<EmailUtility>().As<IEmailUtility>()
                .InstancePerLifetimeScope();

            base.Load(builder);
        }

    }
}
