using AutoMapper;
using DevSkill.Web.Areas.Customers.Models;
using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Application.Features.Categories.Queries;
using DevSkill.Inventory.Application.Features.Units.Queries;
using DevSkill.Inventory.Application.Features.Customers.Commands;
using DevSkill.Inventory.Application.Features.Customers.Queries;
using DevSkill.Inventory.Application.Features.CashAccounts.Queries;
using DevSkill.Inventory.Application.Features.Departments.Queries;
using DevSkill.Inventory.Web.Areas.Settings.Models;
using DevSkill.Inventory.Application.Features.UserRoles.Queries;
using DevSkill.Inventory.Application.Features.BankAccounts.Queries;
using DevSkill.Inventory.Application.Features.MobileAccounts.Queries;
using DevSkill.Inventory.Application.Features.Expenses.Queries;
using DevSkill.Inventory.Infrastructure.Identity;
using DevSkill.Inventory.Web.Areas.Customers.Models;
using DevSkill.Web.Areas.BalanceTransfers.Models;
using DevSkill.Inventory.Application.Features.BalanceTransfers.Queries;

namespace DevSkill.Web
{
    public class WebProfile : Profile
    {
        public WebProfile() 
        { 
            CreateMap<CustomerAddCommand, Customer>().ReverseMap();
            //CreateMap<UpdateCustomerModel, Customer>().ReverseMap();
            CreateMap<CustomerSearchModel,CustomerSearchDto>().ReverseMap();
            CreateMap<UserRoleSearchModel,GetUserRoleListQuery>().ReverseMap();
            CreateMap<CategorySearchModel,GetCategoryListQuery>().ReverseMap();
            CreateMap<ExpenseSearchModel,GetExpenseListQuery>().ReverseMap();
            CreateMap<DepartmentSearchModel,GetDepartmentListQuery>().ReverseMap();
            CreateMap<CustomerSearchModel,GetCustomersQuery>().ReverseMap();
            CreateMap<BalanceTransferSearchModel,GetAllBalanceTransferQuery>().ReverseMap();
            CreateMap<UnitSearchModel,GetUnitListQuery>().ReverseMap();
            CreateMap<CashSearchModel, GetCashAccountListQuery>().ReverseMap();
            CreateMap<BankSearchModel, GetBankAccountListQuery>().ReverseMap();
            CreateMap<MobileSearchModel, GetMobileAccountListQuery>().ReverseMap();
            CreateMap<CustomerViewDto, CustomerViewModel>();
            CreateMap<SalesInvoiceViewDto, SalesInvoiceViewModel>();
            CreateMap<UserRole, ApplicationRole>().ReverseMap();
            CreateMap<User, ApplicationUser>().ReverseMap();
            CreateMap<CompanyProfile, CompanyProfileViewDto>().ReverseMap();
        }
    }
}
