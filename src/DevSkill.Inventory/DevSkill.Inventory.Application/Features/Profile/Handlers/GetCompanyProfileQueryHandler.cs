using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Application.Features.Profile.Queries;
using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain.Repositories;
using MediatR;
using AutoMapper;

namespace DevSkill.Inventory.Application.Features.Profile.Handlers
{
    public class GetCompanyProfileQueryHandler : IRequestHandler<GetCompanyProfileQuery, CompanyProfileViewDto>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        private readonly IMapper _mapper;

        public GetCompanyProfileQueryHandler(IApplicationUnitOfWork applicationUnitOfWork, IMapper mapper)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
            _mapper = mapper;
        }

        public async Task<CompanyProfileViewDto> Handle(GetCompanyProfileQuery request, CancellationToken cancellationToken)
        {
            var entity = await _applicationUnitOfWork.CompanyProfileRepository.GetCompanyProfileAsync();

            if (entity == null)
                return null;


            var dto = _mapper.Map<CompanyProfileViewDto>(entity);
            return dto;
        }
    }
}
