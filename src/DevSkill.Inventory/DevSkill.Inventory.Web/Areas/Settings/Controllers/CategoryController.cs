using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using MediatR;
using DevSkill.Inventory.Domain.Entities;
using System.Collections.Generic;
using DevSkill.Inventory.Application.Features.Categories.Commands;
using DevSkill.Inventory.Application.Features.Categories.Queries;
using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Web.Areas.Admin.Models;
using AutoMapper;
using DevSkill.Inventory.Web.Areas.Settings.Models;

namespace DevSkill.Inventory.Web.Areas.Settings.Controllers
{
    [Area("Settings")]
    [Route("Settings/Category")]
    public class CategoryController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public CategoryController(IMediator mediator,IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet("")]
        public IActionResult IndexSP()
        {
            return View();
        }

        [HttpPost("GetCategoryListJson")]
        public async Task<IActionResult> GetCategoryListJson([FromBody] CategoryListModel model) 
        {    
            var query = _mapper.Map<GetCategoryListQuery>(model.SearchItem);
            if (!string.IsNullOrWhiteSpace(model.SearchItem.Status))
            {
                if (model.SearchItem.Status.Equals("Active", StringComparison.OrdinalIgnoreCase))
                    query.IsActive = true;
                else if (model.SearchItem.Status.Equals("Inactive", StringComparison.OrdinalIgnoreCase))
                    query.IsActive = false;
            }
            query.OrderBy = query.FormatSortExpression("Name", "IsActive", "CreateDate", "Id");

            query.Start = model.Start;
            query.Length = model.Length;

            var (data, total, totalDisplay) = await _mediator.Send(query);

            var result = new
            {
               
                recordsTotal = total,
                recordsFiltered = totalDisplay,
                data = data.Select((c, index) => new
                {
                    serialNumber = index + 1 + query.Start,
                    id = c.Id,
                    name = c.Name,
                    isActive = c.IsActive,
                    createdDate = c.CreatedDate.ToString("yyyy-MM-dd")
                })
            };

            return Json(result);
        }
        [HttpGet("GetCategory/{id}")]
        public async Task<IActionResult> GetCategory(Guid id)
        {
            var category = await _mediator.Send(new GetCategoryByIdQuery { Id = id });
            if (category == null)
                return NotFound();

        var command = new CategoryUpdateCommand
        {
            Id = category.Id,
            Name = category.Name,
            IsActive = category.IsActive
        };

        return PartialView("~/Areas/Settings/Views/Shared/_EditCategoryModal.cshtml", command);
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add(AddCategoryCommand command)
        {
            await _mediator.Send(command);
            return Ok();
        }

        [HttpPost("Edit")]
        public async Task<IActionResult> Edit([FromBody] CategoryUpdateCommand command)
        {

                Console.WriteLine("EDIT CALLED: " + command.Id);
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);


                bool updated = await _mediator.Send(command);
                if (!updated)
                {
                    return NotFound(new { success = false, message = "Category not found" });
                }

            return Ok(new { success = true });
        }

        [HttpPost("Delete")]
        public async Task<IActionResult> Delete(CategoryDeleteCommand command)
        {
            bool deleted = await _mediator.Send(command);
            if (!deleted)
                return NotFound();

            return RedirectToAction("Index");
        }
    }

}
