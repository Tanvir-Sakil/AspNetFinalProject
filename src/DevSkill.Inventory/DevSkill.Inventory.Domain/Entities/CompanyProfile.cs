using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain.Entities
{
    public class CompanyProfile : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public string CompanyName { get; set; }
        public string Address { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public decimal OpeningBalance { get; set; }
        public string Web { get; set; }
        public string Facebook { get; set; }
        public string VatReg { get; set; }
        public string LogoPath { get; set; }
        public string BackgroundImagePath { get; set; }
        public string FavIconPath { get; set; }
        public string SignaturePath { get; set; }
    }
}
