using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DevSkill.Inventory.Application.Features.Profile.Commands
{
    public class UpdateCompanyProfileCommand : IRequest<bool>
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

        public byte[] LogoFile { get; set; }
        public string LogoFileName { get; set; }

        public byte[] BgImage { get; set; }
        public string BgImageFileName { get; set; }

        public byte[] FavIcon { get; set; }
        public string FavIconFileName { get; set; }

        public byte[] Signature { get; set; }
        public string SignatureFileName { get; set; }
    }


}
