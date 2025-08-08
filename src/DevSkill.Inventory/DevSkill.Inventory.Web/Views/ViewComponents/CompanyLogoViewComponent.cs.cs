using DevSkill.Inventory.Application.Features.Profile.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;


namespace DevSkill.Inventory.Web.Views.ViewComponents
{
    public class CompanyLogoViewComponent : ViewComponent
    {
        private readonly IMediator _mediator;

        public CompanyLogoViewComponent(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var companyProfile = await _mediator.Send(new GetCompanyProfileQuery());
            var logoUrl = companyProfile.LogoPath ?? "/admin-lte-4/assets/img/default-logo.png";
            return View("Default", logoUrl);
        }
    }

}
