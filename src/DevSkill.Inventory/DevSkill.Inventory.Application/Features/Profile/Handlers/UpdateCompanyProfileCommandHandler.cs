using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Application.Features.Profile.Commands;
using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Utilities;
using MediatR;

namespace DevSkill.Inventory.Application.Features.Profile.Handlers
{
    public class UpdateCompanyProfileHandler : IRequestHandler<UpdateCompanyProfileCommand, bool>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        private readonly IFileUploader _fileUploader;

        public UpdateCompanyProfileHandler(IApplicationUnitOfWork applicationUnitOfWork, IFileUploader fileUploader)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
            _fileUploader = fileUploader;
        }

        public async Task<bool> Handle(UpdateCompanyProfileCommand request, CancellationToken cancellationToken)
        {
            var repo = _applicationUnitOfWork.CompanyProfileRepository;
            var entity = await repo.GetByIdAsync(request.Id);

            if (entity == null)
                throw new Exception("Company profile not found.");

            entity.CompanyName = request.CompanyName;
            entity.Address = request.Address;
            entity.Mobile = request.Mobile;
            entity.Email = request.Email;
            entity.OpeningBalance = request.OpeningBalance;
            entity.Web = request.Web;
            entity.Facebook = request.Facebook;
            entity.VatReg = request.VatReg;

            if (request.LogoFile != null)
            {
                var fileName = $"{Guid.NewGuid()}_{request.LogoFileName}";
                var savedPath = await _fileUploader.UploadAsync(request.LogoFile, fileName, "uploads/logos");
                entity.LogoPath = savedPath;
            }

            if (request.BgImage != null)
            {
                var fileName = $"{Guid.NewGuid()}_{request.BgImageFileName}";
                var savedPath = await _fileUploader.UploadAsync(request.BgImage, fileName, "uploads/Backgrounds");
                entity.BackgroundImagePath = savedPath;
            }

            if (request.FavIcon != null)
            {
                var fileName = $"{Guid.NewGuid()}_{request.FavIconFileName}";
                var savedPath = await _fileUploader.UploadAsync(request.FavIcon, fileName, "uploads/FavIcons");
                entity.FavIconPath = savedPath;
            }

            if (request.Signature != null)
            {
                var fileName = $"{Guid.NewGuid()}_{request.SignatureFileName}";
                var savedPath = await _fileUploader.UploadAsync(request.Signature, fileName, "uploads/Signatures");
                entity.SignaturePath = savedPath;
            }
             _applicationUnitOfWork.CompanyProfileRepository.Update(entity);

            await _applicationUnitOfWork.SaveChangesAsync();
            return true;
        }
    }

}
