using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DevSkill.Inventory.Application.Features.Products.Commands
{
    public class ProductAddCommand : IRequest<bool>
    {
        [Required]
        public string Code { get; set; } 

        [Required]
        public string Name { get; set; }

       
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

        //  [BindNever]
        ////  public List<SelectListItem> CategoryList { get; set; }

        //  [BindNever]
        ////  public List<SelectListItem> UnitList { get; set; }
    }
}
