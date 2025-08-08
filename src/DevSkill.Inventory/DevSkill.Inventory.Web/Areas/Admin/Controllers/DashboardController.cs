using DevSkill.Inventory.Application.Features.BalanceTransfers.Queries;
using DevSkill.Inventory.Application.Features.Customers.Queries;
using DevSkill.Inventory.Application.Features.Products.Queries;
using DevSkill.Inventory.Application.Features.Sale.Queries;
using DevSkill.Inventory.Application.Features.Users.Queries;
using DevSkill.Inventory.Domain.Features.Sales.Queries;
using DevSkill.Inventory.Web.Areas.Admin.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DevSkill.Inventory.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DashboardController : Controller
    {
        private readonly IMediator _mediator;
        public DashboardController(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task<IActionResult> Index()
        {
            var totalProducts = await _mediator.Send(new GetTotalProductsQuery());
            var (customersData,totalCustomers,totalCustomerDisplay) = await _mediator.Send(new GetCustomersQuery());
            var (salesData,totalSales,totalSalesDisplay) = await _mediator.Send(new GetAllSalesQuery());
            var (BalanceTransferData,totalBalanceTransfers,totalBalanceTransfersDisplay) = await _mediator.Send(new GetAllBalanceTransferQuery());
            var (userData,totalUsers,totalUsersDisplay) = await _mediator.Send(new GetUserListQuery());

            var model = new DashboardViewModel
            {
                TotalProducts = totalProducts,
                TotalCustomers = totalCustomers,
                TotalSales = totalSales,
                TotalBalanceTransfer = totalBalanceTransfers,
                TotalUsers = totalUsers,
            };

            return View(model);
        }
    }
}
