using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.Features.Units.Queries;
using MediatR;

namespace DevSkill.Inventory.Application.Features.Units.Queries
{
    public class GetUnitListQuery : DataTables, 
        IRequest<(IList<Domain.Entities.Unit> Data, int Total, int TotalDisplay)>, 
        IGetUnitListQuery
    {
        public string? Name { get; set; }

        public bool? IsActive { get; set; }


        public DateTime? CreateDateFrom { get; set; }

        public DateTime? CreateDateTo {  get; set; }

        public string? OrderBy { get; set; }
       
    }
}
