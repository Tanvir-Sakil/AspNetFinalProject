//using DevSkill.Inventory.Application.Features.Products.Queries;
//using DevSkill.Inventory.Application.Features.Units.Queries;
//using Microsoft.AspNetCore.Mvc;
//using MediatR;
//using AutoMapper;
//using DevSkill.Inventory.Application.Features.Customers.Queries;
//using DevSkill.Inventory.Application.Features.Sale.Queries;


//namespace DevSkill.Inventory.Web.Areas.Settings.Controllers
//{
//    [Area("Settings")]
//    public class SalesController : Controller
//    {

//        private readonly IMediator _mediator;


//        public SalesController(IMediator mediator)
//        {
//            _mediator = mediator;

//        }
//        public IActionResult Index()
//        {
//            return View();
//        }


//        public IActionResult Add()
//        {
//            return View();
//        }

//        [HttpGet]
//        public async Task<IActionResult> SearchProducts(string query, Guid saleTypeId)
//        {
//            var products = await _mediator.Send(new SearchProductQuery
//            {
//                Query = query,
//                SaleTypeId = saleTypeId
//            });

//            return Json(products.Select(p => new {
//                id = p.Id,
//                text = p.Name,
//                code = p.Code,
//                stock = p.Stock,
//                unitPrice = p.UnitPrice
//            }));
//        }

//        [HttpGet]
//        public async Task<JsonResult> GetSaleTypes()
//        {
//            var saleTypes = await _mediator.Send(new GetSaleTypesQuery()); // Create this query in your Application layer

//            var response = saleTypes.Select(st => new
//            {
//                id = st.Id.ToString(),    // Guid as string
//                text = st.Name
//            }).ToList();

//            return Json(response);
//        }



//        [HttpGet]
//        public async Task<JsonResult> searchCustomers(string query)
//        {
//            var result = await _mediator.Send(new SearchCustomerQuery { Query = query });

//            var response = result.Select(u => new
//            {
//                id = u.Id.ToString(),
//                text = u.Name
//            }).
//            Cast<Object>().
//            ToList();

//            // Optional: add "Add new unit" as last item
//            // response.Add(new { id = "add_new", text = "➕ Add new unit" });

//            return Json(response);
//        }

//        [HttpGet]
//        public async Task<IActionResult> GetProductDetails(Guid productId, Guid saleTypeId)
//        {
//            var Updatedproduct = await _mediator.Send(new GetProductDetailsQuery
//            {
//                ProductId = productId,
//                SaleTypeId = saleTypeId
//            });

//            return Json(new
//            {
//                id = Updatedproduct.Id,
//                code = Updatedproduct.Code,
//                name = Updatedproduct.Name,
//                stock = Updatedproduct.Stock,
//                unitPrice = Updatedproduct.UnitPrice
//            });
//        }
//    }
//}




using DevSkill.Inventory.Application.Features.Sale.Commands;
using DevSkill.Inventory.Application.Features.Sale.Queries;
using DevSkill.Inventory.Application.Features.Products.Queries;
using DevSkill.Inventory.Application.Features.Customers.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using DevSkill.Inventory.Domain.Dtos;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using DevSkill.Inventory.Web.Areas.Settings.Models;
using DevSkill.Inventory.Web.Areas.Sales.Models;
using DevSkill.Inventory.Application.Features.Profile.Queries;
using DevSkill.Web.Areas.Customers.Models;
using AutoMapper;

namespace DevSkill.Inventory.Web.Areas.Sales.Controllers
{
    [Area("Sales")]
    [Route("Sales")]
    public class SalesController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public SalesController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            return View();
        }

        [HttpGet("Add")]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost("GetAllSalesList")]
        public async Task<JsonResult> GetAllSalesList([FromBody] GetAllSalesQuery salesQuery)
        {
             salesQuery.OrderBy = salesQuery.FormatSortExpression(
                   "Id", "InvoiceNo","CustomerID","TotalAmount","PaidAmount","DueAmount",
                    "Date","Terms","VAT","SalesTypeId"
               );

            var (data, total, totalDisplay) = await _mediator.Send(salesQuery);

            var response = data.Select(a => new
            {
                id = a.Id,
                invoiceNo = a.InvoiceNo,
                date = a.Date.ToString("yyyy-MM-dd"),
                customerName = a.CustomerName,
                totalAmount = a.TotalAmount,
                paidAmount = a.PaidAmount,
                dueAmount = a.DueAmount,
                paymentStatus = a.PaymentStatus
            }).ToList();

            return Json(new
            {
                recordsTotal = total,
                recordsFiltered = totalDisplay,
                data = response
            });
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody]SaleAddCommand command)
        {
            if (!ModelState.IsValid)
                return View(command);

            // Generate invoice number here or inside handler
            // For example, you can generate sequential invoice here before sending command

           // command.InvoiceNo = await GenerateNextInvoiceNo();

            var result = await _mediator.Send(command);

            if (result)
                return Ok();

            ModelState.AddModelError("", "Failed to add sale.");
            return View(command);
        }

        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(Guid id)
        {
            var sale = await _mediator.Send(new GetSaleByIdQuery(id));
            var companyProfileView = await _mediator.Send(new GetCompanyProfileQuery());
            if (sale == null)
                return NotFound();

            sale.CompanyProfileView = companyProfileView;

            return View("Details",sale);
        }

        [HttpGet("SalesDetails/{id}")]
        public async Task<IActionResult> SalesDetails(Guid id)
        {
            var sale = await _mediator.Send(new GetSaleByIdQuery(id));
            var companyProfileView = await _mediator.Send(new GetCompanyProfileQuery());
            if (sale == null)
                return NotFound();

            sale.CompanyProfileView = companyProfileView;

            return View("SalesDetails", sale);
        }

        // GET: Settings/Sales/Edit/{id}
        //public async Task<IActionResult> Edit(Guid id)
        //{
        //    var sale = await _mediator.Send(new GetSaleByIdQuery(id));
        //    if (sale == null)
        //        return NotFound();

        //     You might want to map Sale entity to SaleUpdateCommand or a ViewModel for editing
        //    var command = new SaleUpdateCommand
        //    {
        //        Id = sale.Id,
        //        InvoiceNo = sale.InvoiceNo,
        //        Date = sale.Date,
        //        CustomerID = sale.CustomerID,
        //        SalesType = sale.SalesType,
        //        AccountType = sale.AccountType,
        //        AccountNo = sale.AccountNo,
        //        Note = sale.Note,
        //        Terms = sale.Terms,
        //        VAT = sale.VAT,
        //        Discount = sale.Discount,
        //        TotalAmount = sale.TotalAmount,
        //        PaidAmount = sale.PaidAmount,
        //        DueAmount = sale.DueAmount,
        //        PaymentStatus = sale.PaymentStatus,
        //        Items = sale.Items.Select(i => new SaleItemDto
        //        {
        //            ProductID = i.ProductId,
        //            Quantity = i.Quantity,
        //            UnitPrice = i.UnitPrice,
        //            SubTotal = i.SubTotal
        //        }).ToList()
        //    };

        //    return View(command);
        //}

        // POST: Settings/Sales/Edit
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(SaleUpdateCommand command)
        //{
        //    if (!ModelState.IsValid)
        //        return View(command);

        //    var result = await _mediator.Send(command);
        //    if (result)
        //        return RedirectToAction(nameof(Index));

        //    ModelState.AddModelError("", "Failed to update sale.");
        //    return View(command);
        //}

        // POST: Settings/Sales/Delete/{id}
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Delete(Guid id)
        //{
        //    var result = await _mediator.Send(new SaleDeleteCommand(id));
        //    if (!result)
        //        return BadRequest("Failed to delete sale.");

        //    return RedirectToAction(nameof(Index));
        //}

        // Utility method: generate next invoice number
        //private async Task<string> GenerateNextInvoiceNo()
        //{
        //     Implement logic to get last invoice number from repository or DB
        //     and increment sequentially.
        //     This is a simple example (you should adapt to your actual repo):

        //    var sales = await _mediator.Send(new GetAllSalesQuery());
        //    var lastInvoice = sales
        //        .Select(s => s.InvoiceNo)
        //        .Where(i => i.StartsWith("INV-"))
        //        .OrderByDescending(i => i)
        //        .FirstOrDefault();

        //    if (lastInvoice == null)
        //        return "INV-00001";

        //    var numberPart = lastInvoice.Substring(4);
        //    if (!int.TryParse(numberPart, out var num))
        //        return "INV-00001";

        //    return $"INV-{(num + 1).ToString("D5")}";
        //}

        // Your existing product and customer search endpoints can remain here...

        [HttpGet("SearchProducts")]
        public async Task<IActionResult> SearchProducts(string query, Guid saleTypeId)
        {
            var products = await _mediator.Send(new SearchProductQuery
            {
                Query = query,
                SaleTypeId = saleTypeId
            });

            return Json(products.Select(p => new
            {
                id = p.Id,
                text = p.Name,
                code = p.Code,
                stock = p.Stock,
                unitPrice = p.UnitPrice
            }));
        }

        [HttpGet("GetSaleTypes")]
        public async Task<JsonResult> GetSaleTypes()
        {
            var saleTypes = await _mediator.Send(new GetSaleTypesQuery());

            var response = saleTypes.Select(st => new
            {
                id = st.Id.ToString(),
                text = st.Name
            }).ToList();

            return Json(response);
        }

        [HttpGet("SearchCustomers")]
        public async Task<JsonResult> SearchCustomers(string query)
        {
            var result = await _mediator.Send(new SearchCustomerQuery { Query = query });

            var response = result.Select(u => new
            {
                id = u.Id.ToString(),
                text = u.Name
            }).ToList();

            return Json(response);
        }

        [HttpGet("GetProductDetails")]
        public async Task<IActionResult> GetProductDetails(Guid productId, Guid saleTypeId)
        {
            var Updatedproduct = await _mediator.Send(new GetProductDetailsQuery
            {
                ProductId = productId,
                SaleTypeId = saleTypeId
            });

            return Json(new
            {
                id = Updatedproduct.Id,
                code = Updatedproduct.Code,
                name = Updatedproduct.Name,
                stock = Updatedproduct.Stock,
                unitPrice = Updatedproduct.UnitPrice
            });
        }

        //[HttpGet("Edit/{id}")]
        //public async Task<IActionResult> Edit(Guid id)
        //{
        //    var sale = await _mediator.Send(new GetSaleByIdQuery(id));
        //    if (sale == null) return NotFound();
        //    return View("Edit", sale);
        //}


        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var sale = await _mediator.Send(new GetSaleByIdQuery(id));
            if (sale == null)
                return NotFound();

            var model = new SaleEditViewModel
            {
                InvoiceNo = sale.InvoiceNo,
                Id = sale.Id,
                Date = sale.Date,
                CustomerID = sale.CustomerID,
                CustomerName = sale.CustomerName,
                SalesType = sale.SalesType,
                SaleTypeName = sale.SaleTypeName,
                VAT = sale.VAT,
                Discount = sale.Discount,
                TotalAmount = sale.TotalAmount,
                PaidAmount = sale.PaidAmount,
                DueAmount = sale.DueAmount,
                AccountType = sale.AccountType,
                AccountNo = sale.AccountNo,
                Note = sale.Note,
                Terms = sale.Terms,
                PaymentStatus = sale.PaymentStatus.ToString(),
                Items = sale.Items
            };

            return View(model);
        }

        [HttpPost("Update")]
        public async Task<IActionResult> Update([FromBody] SaleUpdateCommand command)
        {
            if (!ModelState.IsValid) return BadRequest("Invalid data.");
            var result = await _mediator.Send(command);
            return result ? Ok(new { message = "Sale updated successfully." }) : BadRequest("Update failed.");
        }

        [HttpPost("Payment")]
        public async Task<IActionResult> Payment(PaymentAddCommand command)
        {
            if (!ModelState.IsValid) return BadRequest("Invalid data.");
            var result = await _mediator.Send(command);
            return result ? Ok(new { message = "Sale updated successfully." }) : BadRequest("Update failed.");
        }

        [HttpPost("Delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _mediator.Send(new DeleteSaleCommand { Id = id });
            return result ? Ok() : BadRequest("Delete failed.");
        }


    }
}

