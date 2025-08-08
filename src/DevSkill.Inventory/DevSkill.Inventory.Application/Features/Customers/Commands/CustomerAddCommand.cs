using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace DevSkill.Inventory.Application.Features.Customers.Commands
{
    public class CustomerAddCommand : IRequest<Guid>
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public string CustomerID { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public string MobileNumber { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public decimal OpeningBalance { get; set; }

        public string? ImagePath { get; set; }

        public byte[]? ImageFile { get; set; }
        public string? ImageFileName { get; set; }

        public IFormFile? CustomerImage { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
