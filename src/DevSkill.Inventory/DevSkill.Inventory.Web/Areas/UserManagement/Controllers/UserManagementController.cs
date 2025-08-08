using System.Linq;
using DevSkill.Inventory.Application.Features.BalanceTransfers.Queries;
using DevSkill.Inventory.Application.Features.BankAccounts.Queries;
using DevSkill.Inventory.Application.Features.CashAccounts.Queries;
using DevSkill.Inventory.Application.Features.Categories.Queries;
using DevSkill.Inventory.Application.Features.Customers.Queries;
using DevSkill.Inventory.Application.Features.Departments.Queries;
using DevSkill.Inventory.Application.Features.Expenses.Queries;
using DevSkill.Inventory.Application.Features.MobileAccounts.Queries;
using DevSkill.Inventory.Application.Features.Staffs.Queries;
using DevSkill.Inventory.Application.Features.Suppliers.Queries;
using DevSkill.Inventory.Application.Features.Units.Queries;
using DevSkill.Inventory.Application.Features.UserRoles.Queries;
using DevSkill.Inventory.Application.Features.Users.Queries;
using DevSkill.Inventory.Web.Areas.Settings.Models;
using DevSkill.Inventory.Web.Areas.UserManagement.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DevSkill.Inventory.Web.Areas.UserManagement.Controllers
{

    [Area("UserManagement")]
    [Route("UserManagement")]
    public class UserManagementController : Controller
    {
        private readonly IMediator _mediator;
        public UserManagementController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var (customerData, customerTotal,
                customerTotalDisplay) = 
                await _mediator.Send(new GetCustomersQuery());
            var staffs 
                = await _mediator.Send(new GetAllStaffQuery());
            var suppliers
                = await _mediator.Send(new GetAllSuppliersQuery());
            var (userData, userTotal, userTotalDisplay) 
                = await _mediator.Send(new GetUserListQuery());

            var model = new UserManagementViewModel
            {
                TotalUser = userTotal,
                TotalCustomer = customerTotal,
                TotalStaff = staffs.Count,
                TotalSupplier = suppliers.Count(),
            };

            return View(model);
        }
    }
}
