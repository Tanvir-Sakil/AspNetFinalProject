using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace DevSkill.Inventory.Application.Features.Suppliers.Commands
{
    public class UpdateSupplierCommand : IRequest<bool>
    {
        public Guid Id { get; set; }

        public string SupplierCode { get; set; }
        public string SupplierName { get; set; }
        public string Mobile { get; set; }
        public string Company { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public decimal OpeningBalance { get; set; }
    }
}
