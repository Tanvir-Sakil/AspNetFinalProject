using DevSkill.Inventory.Application.Features.BalanceTransfers.Queries;
using DevSkill.Inventory.Application.Features.BankAccounts.Queries;
using DevSkill.Inventory.Application.Features.CashAccounts.Queries;
using DevSkill.Inventory.Application.Features.Categories.Queries;
using DevSkill.Inventory.Application.Features.Departments.Queries;
using DevSkill.Inventory.Application.Features.Expenses.Queries;
using DevSkill.Inventory.Application.Features.MobileAccounts.Queries;
using DevSkill.Inventory.Application.Features.Products.Queries;
using DevSkill.Inventory.Application.Features.Units.Queries;
using DevSkill.Inventory.Application.Features.UserRoles.Queries;
using DevSkill.Inventory.Web.Areas.Admin.Models;
using DevSkill.Inventory.Web.Areas.Settings.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DevSkill.Inventory.Web.Areas.Settings.Controllers
{
    [Area("Settings")]
    [Route("Settings")]
    public class SettingsController : Controller
    {
        private readonly IMediator _mediator;
        public SettingsController(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task<IActionResult> Index()
        {
            var (categoryData,cateogoryTotal,categoryTotalDisplay) = await _mediator.Send(new GetCategoryListQuery());
            var (departmentData,departmentTotal,departmentTotalDisplay) = await _mediator.Send(new GetDepartmentListQuery());
            var (expenseData,expenseTotal,expenseTotalDisplay) = await _mediator.Send(new GetExpenseListQuery());
            var (mobileData,mobileTotal,mobileTotalDisplay) = await _mediator.Send(new GetMobileAccountListQuery());
            var (bankData,bankTotal,bankTotalDisplay) = await _mediator.Send(new GetBankAccountListQuery());
            var (balanceData,balanceTotal,balanceTotalDisplay) = await _mediator.Send(new GetAllBalanceTransferQuery());
            var (cashData,cashTotal,cashTotalDisplay) = await _mediator.Send(new GetCashAccountListQuery());
            var (unitData,unitTotal,unitTotalDisplay) = await _mediator.Send(new GetUnitListQuery());
            var (roleData,roleTotal,roleTotalDisplay) = await _mediator.Send(new GetUserRoleListQuery());
            

   

            var model = new SettingsViewModel
            {
                CategoryTotal = cateogoryTotal,
                DepartmentTotal = departmentTotal,
                ExpenseTotal = expenseTotal,
                MobileTotal = mobileTotal,
                BankTotal = bankTotal,
                BalanceTotal = balanceTotal,
                CashTotal = cashTotal,
                UnitTotal = unitTotal,
                RoleTotal = roleTotal
            };

            return View(model);
        }
    }
}
