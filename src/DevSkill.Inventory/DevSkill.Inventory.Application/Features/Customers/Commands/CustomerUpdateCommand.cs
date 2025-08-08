using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using System;
using System.ComponentModel.DataAnnotations;

namespace DevSkill.Inventory.Application.Features.Customers.Commands
{
        public class UpdateCustomerCommand : IRequest<bool>
        {
            public Guid Id { get; set; }

            public string CustomerID { get; set; }

            public string Name { get; set; }

            public string CompanyName { get; set; }

            public string? MobileNumber { get; set; }

            public string? Email { get; set; }

            public string? Address { get; set; }

            public decimal OpeningBalance { get; set; }

            public bool IsActive { get; set; }

            public string? ImagePath { get; set; }

            public byte[]? ImageFile { get; set; }
            public string? ImageFileName { get; set; }
    }
 }
