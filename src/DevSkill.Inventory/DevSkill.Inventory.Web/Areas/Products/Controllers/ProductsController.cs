using System.Drawing;
using System.Drawing.Imaging;
using System.Net.NetworkInformation;
using System.Web;
using AutoMapper;
using DevSkill.Application.Features.Products.Queries;
using DevSkill.Inventory.Application.Features.Categories.Queries;
using DevSkill.Inventory.Application.Features.Companies.Queries;
using DevSkill.Inventory.Application.Features.Customers.Queries;
using DevSkill.Inventory.Application.Features.Products.Commands;
using DevSkill.Inventory.Application.Features.Products.Queries;
using DevSkill.Inventory.Application.Features.Units.Queries;
using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.Features.Products.Commands;
using DevSkill.Inventory.Domain.Services;
using DevSkill.Inventory.Domain.Utilities;
using DevSkill.Inventory.Infrastructure;
using DevSkill.Inventory.Web.Areas.Admin.Models;
using DevSkill.Inventory.Web.Areas.Products.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NuGet.ProjectModel;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace DevSkill.Inventory.Web.Areas.Products.Controllers
{
    [Area("Products")]
    [Route("Products")]
    public class ProductsController(ILogger<ProductsController> logger, IMediator mediator, IWebHostEnvironment env,IApplicationUnitOfWork applicationUnitOfWork) : Controller
    {
        private readonly ILogger<ProductsController> _logger = logger;
        private readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork;

        private readonly IMediator _mediator = mediator;
        private readonly IWebHostEnvironment _env = env;

        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("Add")]
        
        public IActionResult Add()
        {
            var model = new ProductAddCommand(); 
            return View(model);
        }

        [HttpGet("SearchCategories")]
        public async Task<JsonResult> SearchCategories(string query)
        {
            var result = await _mediator.Send(new SearchCategoryQuery { Query = query });

            var response = result.Select(u => new
            {
                id = u.Id.ToString(),
                text = u.Name
            }).
            Cast<Object>().
            ToList();
            return Json(response);
        }

        [HttpGet("SearchUnits")]
        public async Task<JsonResult> SearchUnits(string query)
        {
            var result = await _mediator.Send(new SearchUnitQuery { Query = query });

            var response = result.Select(u => new
            {
                id = u.Id.ToString(),
                text = u.Name
            }).
            Cast<Object>().
            ToList();
            return Json(response);
        }




        [HttpGet("SearchAsync")]
        public async Task<JsonResult> SearchAsync(string query)
        {

            var result = await _mediator.Send(new SearchCustomerQuery { Query = query });
            var response = result.Select(c => new
            {
                id = c.Id,
                text = c.Name
            });

            return Json(response);
        }

        [HttpPost("Add")]
        [Authorize(Policy = Permissions.Product.Add)]
        public async Task<IActionResult> Add(ProductAddCommand productAddCommand)
        {
            if (productAddCommand.ProductImage != null && productAddCommand.ProductImage.Length > 0)
            {
                var productImageBytes = await ConvertToBytes.ConvertFileToBytes(productAddCommand.ProductImage);
                productAddCommand.ImageFile = productImageBytes;
                productAddCommand.ImageFileName = productAddCommand.ProductImage.FileName;
            }

                if (productAddCommand.UnitId == Guid.Empty && !string.IsNullOrWhiteSpace(productAddCommand.NewUnitName))
                {
                    productAddCommand.UnitId = null;
                    ModelState["UnitId"]?.Errors.Clear();
                }

                if (!ModelState.IsValid)
                {
                    foreach (var error in ModelState)
                    {
                        foreach (var subError in error.Value.Errors)
                        {
                            Console.WriteLine($"{error.Key}: {subError.ErrorMessage}");
                        }
                    }
                    return View(productAddCommand);
                }
            var result = await _mediator.Send(productAddCommand);

            if (!result)
            {
                ModelState.AddModelError("", "Failed to add product.");
                return View(productAddCommand);
            }

            TempData["SuccessMessage"] = "Product added successfully.";
            return RedirectToAction("Index");
        }

        [HttpPost("GetProductsJsonDataSP")]
        public async Task<JsonResult> GetProductsJsonDataSPAsync([FromBody] GetProductsQuery productQuery)
        {

            productQuery.OrderBy = productQuery.FormatSortExpression(
                "Name", "Code", "Category", "PurchasePrice", "MRPPrice", "WholesalePrice",
                "Stock", "LowStock", "DamageStock", "Id"
            );

            var (data, total, totalDisplay) = await _mediator.Send(productQuery);

            var products = new
            {
                recordsTotal = total,
                recordsFiltered = totalDisplay,
                data = data.Select(record => new
                {
                    imagePath = record.ImagePath,
                    code = record.Code,
                    name = record.Name,
                    categoryName = record.Category?.Name ?? "",
                    purchasePrice = record.PurchasePrice.ToString("0.00"),
                    mrpPrice = record.MRP.ToString("0.00"),
                    wholesalePrice = record.WholesalePrice.ToString("0.00"),
                    stock = record.Stock,
                    lowStock = record.LowStock,
                    damageStock = record.Damage,
                    id = record.Id
                })
            };
            return Json(products);


            //catch (Exception ex)
            //{
            //    _logger.LogError(ex, "There was a problem in getting product");
            //    return Json(DataTables.EmptyResult);
            //}
        }

        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var product = await _mediator.Send(new GetProductByIdQuery { Id = id });
            if (product == null) return NotFound();

            var command = new ProductUpdateCommand
            {
                Id = product.Id,
                Code = product.Code,
                Name = product.Name,
                CategoryId = product.CategoryId,
                UnitId = product.UnitId,
                PurchasePrice = product.PurchasePrice,
                MRP = product.MRP,
                WholesalePrice = product.WholesalePrice,
                Stock = product.Stock,
                LowStock = product.LowStock,
                Demage = product.Damage,
                ImagePath = product.ImagePath
            };

            return PartialView("_EditProductModal", command);
        }

        [HttpPost("Update")]
        public async Task<IActionResult> Update(ProductUpdateCommand productUpdatecommand)
        {
            if (productUpdatecommand.ProductImage != null && productUpdatecommand.ProductImage.Length > 0)
            {
                var productImageBytes = await ConvertToBytes.ConvertFileToBytes(productUpdatecommand.ProductImage);
                productUpdatecommand.ImageFile = productImageBytes;
                productUpdatecommand.ImageFileName = productUpdatecommand.ProductImage.FileName;
            }

            if (!ModelState.IsValid)
            {
                return PartialView("_EditProductModal", productUpdatecommand);
            }

            var result = await _mediator.Send(productUpdatecommand);

            if (!result)
            {
                ModelState.AddModelError("", "Product update failed.");
                return PartialView("_EditProductModal", productUpdatecommand);
            }

            return RedirectToAction("Index");
        }

        [HttpGet("View/{id}")]
        public async Task<IActionResult> View(Guid id)
        {
            var product = await _mediator.Send(new GetProductByIdQuery { Id = id });
            if (product == null)
                return NotFound();

            var productDto = new ProductDto
            {
                Name = product.Name,
                Code = product.Code,
                CategoryName = product.Category.Name != null ?product.Category.Name  : null,
                PurchasePrice = product.PurchasePrice,
                MRPPrice = product.MRP,
                WholesalePrice = product.WholesalePrice,
                LowStock = product.LowStock,
                Stock = product.Stock,
                Unit = product.Unit != null ? product.Unit.Name  : null,
                ImagePath = product.ImagePath
            };


            return View("View", productDto); 
        }
        [HttpPost("Delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _mediator.Send(new DeleteProductCommand(id));

            if (!result)
            {
                TempData["ErrorMessage"] = "Product not found or could not be deleted.";
                return NotFound(); 
            }

            TempData["SuccessMessage"] = "Product deleted successfully.";
            return RedirectToAction("Index");
        }

        [HttpPost("AddStoreProduct")]
        public async Task<IActionResult> AddStoreProduct([FromForm] AddStoreProductCommand command)
        {
            var result = await _mediator.Send(command);
            if (!result)
                return BadRequest("Store update failed.");

            return Ok("Store updated successfully.");
        }

        [HttpPost("AddDamageProduct")]
        public async Task<IActionResult> AddDamageProduct([FromForm] AddDamageProductCommand command)
        {
            var result = await _mediator.Send(command);
            if (!result)
                return BadRequest("Damage update failed.");

            return Ok("Damage updated successfully.");
        }

        [HttpGet("SearchProductsForDropdown")]
        public async Task<IActionResult> SearchProductsForDropdown(string query)
       {
            var result = await _mediator.Send(new SearchProductDropdownQuery { Query = query });

            var response = result.Select(p => new
            {
                id = p.Id,
                text = $"{p.Name} ({p.Code})"
            });

            return Json(response);
        }

        [HttpGet("Barcode/{id}")]
        public async Task<IActionResult> Barcode(Guid id)
        {
            var product = await _mediator.Send(new GetProductByIdQuery { Id = id });
            if (product == null) return NotFound();

            var model = new BarcodePrintViewModel
            {
               
                Code = product.Code,
                Name = product.Name,
                Price = product.WholesalePrice 
            };

            return View("Barcode", model); 
        }


        [HttpGet("GenerateBarcode")]
        public IActionResult GenerateBarcode(string code)
        {
            if (string.IsNullOrEmpty(code)) return BadRequest();

            var writer = new ZXing.BarcodeWriterPixelData
            {
                Format = ZXing.BarcodeFormat.CODE_128,
                Options = new ZXing.Common.EncodingOptions
                {
                    Height = 80,
                    Width = 300,
                    Margin = 1
                }
            };

            var pixelData = writer.Write(code);
            using var bitmap = new Bitmap(pixelData.Width, pixelData.Height, PixelFormat.Format32bppRgb);
            var bitmapData = bitmap.LockBits(new Rectangle(0, 0, pixelData.Width, pixelData.Height),
                ImageLockMode.WriteOnly, PixelFormat.Format32bppRgb);
            System.Runtime.InteropServices.Marshal.Copy(pixelData.Pixels, 0, bitmapData.Scan0, pixelData.Pixels.Length);
            bitmap.UnlockBits(bitmapData);

            using var stream = new MemoryStream();
            bitmap.Save(stream, ImageFormat.Png);
            stream.Seek(0, SeekOrigin.Begin);

            return File(stream.ToArray(), "image/png");
        }

        [HttpPost("RenderBarcodeLabels")]
        public IActionResult RenderBarcodeLabels([FromForm] string name, [FromForm] string code, [FromForm] decimal price, [FromForm] int count)
        {
            ViewBag.PrintCount = count;
            ViewBag.Name = name;
            ViewBag.Code = code;
            ViewBag.Price = price;
            return PartialView("~/Areas/Products/Views/Shared/_BarcodeLabelsPartial.cshtml");
        }



    }
}
