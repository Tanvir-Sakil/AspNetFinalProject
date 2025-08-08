using System.Web;
using AutoMapper;
using DevSkill.Inventory.Application.Exceptions;
using DevSkill.Inventory.Application.Features.Categories.Queries;
using DevSkill.Inventory.Application.Features.Customers.Commands;
using DevSkill.Inventory.Application.Features.Customers.Queries;
using DevSkill.Inventory.Application.Features.Products.Commands;
using DevSkill.Inventory.Application.Features.Products.Queries;
using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.Features.Customers.Queries;
using DevSkill.Inventory.Domain.Features.Products.Commands;
using DevSkill.Inventory.Domain.Utilities;
using DevSkill.Inventory.Infrastructure;
using DevSkill.Inventory.Web.Areas.Admin.Models;
using DevSkill.Inventory.Web.Areas.Customers.Models;
using DevSkill.Web.Areas.Customers.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ResponseModel = DevSkill.Inventory.Web.Areas.Customers.Models.ResponseModel;
using ResponseTypes = DevSkill.Inventory.Web.Areas.Customers.Models.ResponseTypes;

namespace DevSkill.Inventory.Web.Areas.Customers.Controllers
{
    [Area("Customers")]
    [Route("Customers")]
    public class CustomersController(ILogger<CustomersController> logger, 
        IMediator mediator, IMapper mapper, 
        IApplicationUnitOfWork applicationUnitOfWork,
        IWebHostEnvironment env) : Controller
    {
        private readonly ILogger<CustomersController> _logger = logger;
        private readonly IMediator _mediator = mediator;
        private readonly IMapper _mapper = mapper;
        private readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork;
        private readonly IWebHostEnvironment _env = env;


        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("Add")]
        public IActionResult Add()
        {
            var model = new CustomerAddCommand();
            return View(model);
        }

        //public async Task<IActionResult> GetCustomerForUpdate(Guid id)
        //{
        //    var customer = await _mediator.Send(new GetCustomerByIdQuery { Id = id });
        //    if (customer == null)
        //        return NotFound();

        //    var command = new UpdateCustomerCommand
        //    {
        //        Id = customer.Id,
        //        Name = customer.Name,
        //        CompanyName = customer.CompanyName,
        //        MobileNumber = customer.MobileNumber,
        //        Email = customer.Email,
        //        Address = customer.Address,
        //        OpeningBalance = customer.OpeningBalance,
        //        IsActive = customer.IsActive,
        //        ImagePath = customer.ImagePath
        //    };

        //    return PartialView("_UpdateCustomerPartial", command);
        //}


        //[HttpPost, ValidateAntiForgeryToken]
        //public async Task<IActionResult> Add(CustomerAddCommand model)
        //{
        //    if (!ModelState.IsValid) return View(model);

        //    try
        //    {
        //        var result = await _mediator.Send(model);
        //        if (!result)
        //        {
        //            ModelState.AddModelError("", "Failed to add customer.");
        //            return View(model);
        //        }

        //        //TempData.Put("ResponseMessage", new ResponseModel
        //        //{
        //        //    Message = "Customer added",
        //        //    Type = ResponseTypes.Success
        //        //});

        //        return RedirectToAction("Index");
        //    }
        //    catch (DuplicateCustomerNameException ex)
        //    {
        //        ModelState.AddModelError("DuplicatCustomer", ex.Message);
        //        //TempData.Put("ResponseMessage", new ResponseModel
        //        //{
        //        //    Message = ex.Message,
        //        //    Type = ResponseTypes.Danger
        //        //});
        //        return View(model);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Failed to add customer");
        //        //TempData.Put("ResponseMessage", new ResponseModel
        //        //{
        //        //    Message = "Failed to add customer",
        //        //    Type = ResponseTypes.Danger
        //        //});
        //        return View(model);
        //    }
        //}

        [HttpPost("Add")]
        public async Task<IActionResult> Add(CustomerAddCommand model)
        {
            if (string.IsNullOrWhiteSpace(model.Name))
                return BadRequest("Customer name is required");

            try
            {
                if (model.CustomerImage != null && model.CustomerImage.Length > 0)
                {
                    var customerImageBytes = await ConvertToBytes.ConvertFileToBytes(model.CustomerImage);
                    model.ImageFile = customerImageBytes;
                    model.ImageFileName = model.CustomerImage.FileName;
                }


                var result = await _mediator.Send(model);
                    //if (!result)
                    //{
                    //    ModelState.AddModelError("", "Failed to add product.");
                    //    return View();
                    //}

                    TempData["SuccessMessage"] = "Product added successfully.";

                    return Json(new
                    {
                        id = result,       // Make sure `model.Id` is set by your command handler
                        text = model.Name
                    });

                }
                // 🔥 Return as JSON if this was called via AJAX
                // return Json(new { id = Guid.NewGuid(), text = model.Name });
            catch (Exception ex)
            {
                return BadRequest("Error occurred: " + ex.Message);
            }
        }

        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var customer = await _mediator.Send(new GetCustomerByIdQuery { Id = id });
            if (customer == null) return NotFound();

            var command = new UpdateCustomerCommand
            {
                Id = customer.Id,
                Name = customer.Name,
                MobileNumber = customer.MobileNumber,
                Email = customer.Email,
                Address = customer.Address,
                OpeningBalance = customer.OpeningBalance,
                IsActive = customer.IsActive,
                ImagePath = customer.ImagePath
            };

            return PartialView("_UpdateCustomerModal", command);
        }

        [HttpPost("Update")]
        public async Task<IActionResult> Update(UpdateCustomerCommand command, IFormFile? CustomerImage)
        {
            if (CustomerImage != null)
            {
                var uploads = Path.Combine(_env.WebRootPath, "uploads");
                Directory.CreateDirectory(uploads);
                var fileName = Guid.NewGuid() + Path.GetExtension(CustomerImage.FileName);
                var filePath = Path.Combine(uploads, fileName);

                using var stream = new FileStream(filePath, FileMode.Create);
                await CustomerImage.CopyToAsync(stream);

                command.ImagePath = "/uploads/" + fileName;
            }

            var result = await _mediator.Send(command);
            if (result)
            {
                TempData["Success"] = "Customer updated successfully.";
                return RedirectToAction("Index");
            }

            TempData["Error"] = "Customer not found or update failed.";
            return RedirectToAction("Index");
        }



        //[HttpGet]
        //public async Task<IActionResult> Update(Guid id)
        //{
        //    var customer = await _mediator.Send(new GetCustomerByIdQuery { Id = id });
        //    if (customer == null) return NotFound();
        //    return View(customer);
        //}

        //[HttpPost, ValidateAntiForgeryToken]
        //public async Task<IActionResult> Update(UpdateCustomerCommand model)
        //{
        //    if (!ModelState.IsValid) return View(model);

        //    try
        //    {
        //        var result = await _mediator.Send(model);
        //        if (!result)
        //        {
        //            ModelState.AddModelError("", "Customer not found or failed to update.");
        //            return View(model);
        //        }

        //        TempData.Put("ResponseMessage", new ResponseModel
        //        {
        //            Message = "Customer updated",
        //            Type = ResponseTypes.Success
        //        });

        //        return RedirectToAction("Index");
        //    }
        //    catch (DuplicateCustomerNameException ex)
        //    {
        //        ModelState.AddModelError("DuplicatCustomer", ex.Message);
        //        TempData.Put("ResponseMessage", new ResponseModel
        //        {
        //            Message = ex.Message,
        //            Type = ResponseTypes.Danger
        //        });
        //        return View(model);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Failed to update customer");
        //        TempData.Put("ResponseMessage", new ResponseModel
        //        {
        //            Message = "Failed to update Customer",
        //            Type = ResponseTypes.Danger
        //        });
        //        return View(model);
        //    }
        //}

        [HttpPost("Delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var result = await _mediator.Send(new DeleteCustomerCommand(id));
                if (!result)
                {
                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Failed to delete Customer",
                        Type = ResponseTypes.Danger
                    });
                    return NotFound();
                }

                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = "Customer deleted",
                    Type = ResponseTypes.Success
                });

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete Customer");
                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = "Failed to delete Customer",
                    Type = ResponseTypes.Danger
                });
                return RedirectToAction("Index");
            }
        }

        [HttpGet("View/{id}")]
        public async Task<IActionResult> View(Guid id)
        {
            var dto = await _mediator.Send(new GetCustomerForViewQuery { Id = id });

            if (dto == null)
                return NotFound();

            var model = _mapper.Map<CustomerViewModel>(dto);

            return View("CustomerView", model);
        }

        [HttpPost("GetCustomerJsonData")]
        public async Task<JsonResult> GetCustomerJsonData([FromBody] GetCustomersQuery query)
        {
            try
            {
                query.OrderBy = query.FormatSortExpression(
                    "Name", "CompanyName", "Email","OpeningBalance", "Address","CreateDate", "Id"
                );

                var (data, total, totalDisplay) = await _mediator.Send(query);

                var customers = new
                {
                    recordsTotal = total,
                    recordsFiltered = totalDisplay,
                    data = data.Select(record => new
                    {
                        id = record.Id,
                        imagePath = record.ImagePath,
                        customerID = record.CustomerID,
                        name = record.Name,
                        mobileNumber = record.MobileNumber,
                        address = record.Address,
                        email = record.Email,
                        openingBalance = record.OpeningBalance,
                        status = record.IsActive ? "Active" : "Inactive"
                    }).ToList()
                };

                return Json(customers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "There was a problem in getting customers");
                return Json(DataTables.EmptyResult);
            }
        }

        [HttpGet("SearchCustomers")]
        public async Task<JsonResult> SearchCustomers(string query)
       {
           var result = await _mediator.Send(new SearchCustomerQuery { Query = query });
            var data = result.Select(u => new
            {
                id = u.Id.ToString(),
                text = u.Name
            });
           return Json(data);
        }
    }
}
