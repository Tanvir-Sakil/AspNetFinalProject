using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace DevSkill.Inventory.Application.Features.Products.Commands
{
        public class ProductUpdateCommand : IRequest<bool>
        {
            [Required]
            public Guid Id { get; set; }

            [Required]
            public string Code { get; set; }

            [Required]
            public string Name { get; set; }

            [Required]
            public Guid CategoryId { get; set; }

            [Required]
            public Guid? UnitId { get; set; }

            public string? NewUnitName { get; set; }

            public decimal PurchasePrice { get; set; }

            public decimal MRP { get; set; }

            public decimal WholesalePrice { get; set; }

            public int Stock { get; set; }

            public int LowStock { get; set; }

            public int Demage { get; set; }
            public string? ImagePath { get; set; }

            public byte[]? ImageFile { get; set; }
            public string? ImageFileName { get; set; }
            public IFormFile? ProductImage { get; set; }
    }
}
