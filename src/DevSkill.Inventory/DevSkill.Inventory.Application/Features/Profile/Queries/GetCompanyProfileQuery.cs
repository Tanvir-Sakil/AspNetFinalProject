using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Domain.Dtos;
using MediatR;

namespace DevSkill.Inventory.Application.Features.Profile.Queries
{
    public class GetCompanyProfileQuery : IRequest<CompanyProfileViewDto> { }
}
