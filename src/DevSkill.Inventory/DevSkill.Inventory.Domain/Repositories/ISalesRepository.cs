using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.Features.Sales.Queries;

namespace DevSkill.Inventory.Domain.Repositories
{
    public interface ISalesRepository : IRepository<Sale, Guid>
    {
        Task<Sale> GetByIdWithDetailsAsync(Guid id);

        Task<(IList<Sale> Data, int Total, int TotalDisplay)> GetAllSaleAsync(IGetSalesQuery request);

        Task<Sale> GetByInvoiceWithDetailsAsync(string invoice);

        Task<string> GenerateInvoiceAsync();
        Task AddSaleAsync(Sale sale);
    }
}
