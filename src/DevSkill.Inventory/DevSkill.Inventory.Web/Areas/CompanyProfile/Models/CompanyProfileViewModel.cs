namespace DevSkill.Inventory.Web.Areas.CompanyProfile.Models
{
    public class CompanyProfileViewModel
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
        public IFormFile LogoFile { get; set; }
        public IFormFile BgImage { get; set; }
        public IFormFile FavFile { get; set; }
        public IFormFile SignatureFile { get; set; }
        public string ExistingLogoPath { get; set; }
        public string ExistingBgImagePath { get; set; }
        public string ExistingFavIconPath { get; set; }
        public string ExistingSignaturePath { get; set; }
    }
}
