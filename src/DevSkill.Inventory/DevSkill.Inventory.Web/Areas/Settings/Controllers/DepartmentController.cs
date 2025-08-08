using AutoMapper;
using DevSkill.Inventory.Application.Features.Departments.Commands;
using DevSkill.Inventory.Application.Features.Departments.Queries;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Web.Areas.Admin.Models;
using DevSkill.Inventory.Web.Areas.Settings.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DevSkill.Inventory.Web.Areas.Settings.Controllers
{
    [Area("Settings")]
    [Route("Settings/Department")]
    public class DepartmentController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public DepartmentController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        public IActionResult IndexSP()
        {
            return View();
        }

        [HttpPost("GetDepartmentListJson")]
        public async Task<IActionResult> GetDepartmentListJson([FromBody] DepartmentListModel model)
        {
            var query = _mapper.Map<GetDepartmentListQuery>(model.SearchItem);
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
        [HttpGet("GetAllDepartments")]
        public async Task<IActionResult> GetAllDepartments()
        {
            var result = await _mediator.Send(new GetAllDepartmentsQuery());
            if (result == null)
                return NotFound();

            var response = result.Select(
                x => new 
                { id = x.Id, 
                  name = x.Name 
                }
              );

             return Ok(response);
        }

        [HttpGet("GetDepartment")]
        public async Task<IActionResult> GetDepartment(Guid id)
        {
            var department = await _mediator.Send(new GetDepartmentByIdQuery { Id = id });
            if (department == null)
                return NotFound();

            var command = new DepartmentUpdateCommand
            {
                Id = department.Id,
                Name = department.Name,
                IsActive = department.IsActive
            };

            return PartialView("~/Areas/Settings/Views/Shared/_EditDepartmentModal.cshtml", command);
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add(AddDepartmentCommand command)
        {
            await _mediator.Send(command);
            return Ok();
        }

        [HttpPost("Edit")]
        public async Task<IActionResult> Edit([FromBody] DepartmentUpdateCommand command)
        {

            Console.WriteLine("EDIT CALLED: " + command.Id); //
            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            bool updated = await _mediator.Send(command);
            if (!updated)
            {
                return NotFound(new { success = false, message = "Department not found" });
            }

            return Ok(new { success = true });
        }

        [HttpPost("Delete")]
        public async Task<IActionResult> Delete(DepartmentDeleteCommand command)
        {
            bool deleted = await _mediator.Send(command);
            if (!deleted)
                return NotFound();

            return RedirectToAction("IndexSP");
        }
    }
}
