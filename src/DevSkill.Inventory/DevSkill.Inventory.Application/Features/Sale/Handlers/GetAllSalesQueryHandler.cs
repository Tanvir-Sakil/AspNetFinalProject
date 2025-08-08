using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Application.Features.Sale.Queries;
using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain;
using MediatR;
using DevSkill.Inventory.Domain.Enums;
using DevSkill.Application.Features.Products.Queries;
using System.Numerics;
using DevSkill.Inventory.Domain.Features.Sales.Queries;

namespace DevSkill.Inventory.Application.Features.Sale.Handlers
{
    public class GetAllSalesQueryHandler : IRequestHandler<GetAllSalesQuery, (IList<SaleDto>, int, int)>
    {
        private readonly IApplicationUnitOfWork _unitOfWork;

        public GetAllSalesQueryHandler(IApplicationUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<(IList<SaleDto>, int, int)> Handle(GetAllSalesQuery request, CancellationToken cancellationToken)
        {
            var (sales, total, totalDisplay) = await _unitOfWork.SalesRepository.GetAllSaleAsync(request);

            var result = sales.Select(s => new SaleDto
            {
                Id = s.Id,
                InvoiceNo = s.InvoiceNo,
                Date = s.Date,
                CustomerName = s.Customer?.Name ?? "Unknown",
                TotalAmount = s.TotalAmount,
                PaidAmount = s.PaidAmount,
                DueAmount = s.DueAmount,
                PaymentStatus = s.PaymentStatus == PaymentStatus.FullyPaid ? "Full Paid"
                            : s.PaymentStatus == PaymentStatus.PartiallyPaid ? "Partial Paid"
                            : "Due"
            }).ToList();

            return (result,total,totalDisplay);
        }
    }
}
