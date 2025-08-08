using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using MediatR;
using DevSkill.Inventory.Domain.Entities;
using System.Collections.Generic;
using DevSkill.Inventory.Application.Features.Units.Commands;
using DevSkill.Inventory.Application.Features.Units.Queries;
using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Web.Areas.Admin.Models;
using AutoMapper;
using DevSkill.Inventory.Web.Areas.Settings.Models;

namespace DevSkill.Inventory.Web.Areas.Settings.Controllers
{
        [Area("Settings")]
        [Route("Settings/Unit")]
        public class UnitController : Controller
        {
            private readonly IMediator _mediator;
            private readonly IMapper _mapper;

        public UnitController(IMediator mediator,IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet("")]
        public IActionResult IndexSP()
        {
            return View();
        }

        [HttpPost("GetUnitListJson")]
        public async Task<IActionResult> GetUnitListJson([FromBody] UnitListModel model) 
        {    
            var query = _mapper.Map<GetUnitListQuery>(model.SearchItem);
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


        [HttpGet("GetUnit/{id}")]
        public async Task<IActionResult> GetUnit(Guid id)
        {
            var unit = await _mediator.Send(new GetUnitByIdQuery { Id = id });
            if (unit == null)
                return NotFound();

                var command = new UnitUpdateCommand
                {
                    Id = unit.Id,
                    Name = unit.Name,
                    IsActive = unit.IsActive
                };

                return PartialView("~/Areas/Settings/Views/Shared/_EditUnitModal.cshtml", command);
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add(AddUnitCommand command)
        {
            await _mediator.Send(command);
            return Ok();
        }

     
        [HttpPost("Edit")]
        public async Task<IActionResult> Edit([FromBody] UnitUpdateCommand command)
        {

                Console.WriteLine("EDIT CALLED: " + command.Id); 
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);


                bool updated = await _mediator.Send(command);
                if (!updated)
                {
                    return NotFound(new { success = false, message = "Unit not found" });
                }

            return Ok(new { success = true });
        }

        [HttpPost("Delete")]
        public async Task<IActionResult> Delete(UnitDeleteCommand command)
        {
            bool deleted = await _mediator.Send(command);
            if (!deleted)
                return NotFound();

            return RedirectToAction("Index");
        }
     }
}
