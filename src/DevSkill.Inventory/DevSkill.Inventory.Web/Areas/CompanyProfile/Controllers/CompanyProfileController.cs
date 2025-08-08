using DevSkill.Inventory.Application.Features.Profile.Commands;
using DevSkill.Inventory.Application.Features.Profile.Queries;
using DevSkill.Inventory.Web.Areas.CompanyProfile.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DevSkill.Inventory.Web.Areas.CompanyProfile.Controllers
{
    [Area("CompanyProfile")]
    public class CompanyProfileController : Controller
    {
        private readonly IMediator _mediator;

        public CompanyProfileController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var dto = await _mediator.Send(new GetCompanyProfileQuery());

            if (dto == null)
                return NotFound();

            var model = new CompanyProfileViewModel
            {
                Id = dto.Id,
                CompanyName = dto.CompanyName,
                Address = dto.Address,
                Mobile = dto.Mobile,
                Email = dto.Email,
                OpeningBalance = dto.OpeningBalance,
                Web = dto.Web,
                Facebook = dto.Facebook,
                VatReg = dto.VatReg,
                ExistingLogoPath = dto.LogoPath,
                ExistingBgImagePath = dto.BackgroundImagePath,
                ExistingFavIconPath = dto.FavIconPath,
                ExistingSignaturePath = dto.SignaturePath
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCompanyProfile([FromForm] CompanyProfileViewModel model)
        {
            var logoBytes = await ConvertToBytes(model.LogoFile);
            var bgBytes = await ConvertToBytes(model.BgImage);
            var favBytes = await ConvertToBytes(model.FavFile);
            var signBytes = await ConvertToBytes(model.SignatureFile);

            var command = new UpdateCompanyProfileCommand
            {
                Id = model.Id,
                CompanyName = model.CompanyName,
                Address = model.Address,
                Mobile = model.Mobile,
                Email = model.Email,
                OpeningBalance = model.OpeningBalance,
                Web = model.Web,
                Facebook = model.Facebook,
                VatReg = model.VatReg,
                LogoFile = logoBytes,
                LogoFileName = model.LogoFile?.FileName,
                BgImage = bgBytes,
                BgImageFileName = model.BgImage?.FileName,
                FavIcon = favBytes,
                FavIconFileName = model.FavFile?.FileName,
                Signature = signBytes,
                SignatureFileName = model.SignatureFile?.FileName
            };

            await _mediator.Send(command);
            return RedirectToAction("Index");
        }

        private async Task<byte[]> ConvertToBytes(IFormFile file)
        {
            if (file == null) return null;
            using var ms = new MemoryStream();
            await file.CopyToAsync(ms);
            return ms.ToArray();
        }

    }

}
