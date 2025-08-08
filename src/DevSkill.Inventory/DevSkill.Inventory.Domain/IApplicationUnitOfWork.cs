using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.Repositories;

namespace DevSkill.Inventory.Domain
{
    public interface IApplicationUnitOfWork : IUnitOfWork
    {
        public IProductRepository ProductRepository { get; }

        public ICustomerRepository CustomerRepository { get; }

        public ICategoryRepository CategoryRepository { get; }
        public IExpenseRepository ExpenseRepository { get; }
        public IUserRoleRepository UserRoleRepository { get; }
        public IDepartmentRepository DepartmentRepository { get; }

        public ISaleTypeRepository SaleTypesRepository { get; }
        public IUserRepository UserRepository { get; }
        public ISalesRepository SalesRepository { get; }
        public IStaffRepository StaffRepository { get; }
        public ISupplierRepository SupplierRepository { get; }

        public ICustomerLedgerRepository CustomerLedgerRepository { get; }

        public IAccountTypeRepository AccountTypesRepository { get; }
        public IAccountSearchRepository AccountSearchRepository { get; }

        public IUnitRepository UnitRepository { get; }
        public ICashAccountRepository CashAccountRepository { get; }
        public IBankAccountRepository BankAccountRepository { get; }
        public IMobileAccountRepository MobileAccountRepository { get; }
        public IBalanceTransferRepository BalanceTransferRepository { get; }

        public IUserPermissionRepository UserPermissionRepository { get; }
        IImageResizeQueueRepository ImageResizeQueueRepository { get; }

        public ICompanyProfileRepository CompanyProfileRepository { get; }

        public ISqsRepository SqsRepository { get; }
        public IS3Repository S3Repository { get; }

        Task<(IList<Customer> data, int total, int totalDisplay)> GetCutomersSP(int pageIndex,
        int pageSize, string? order, CustomerSearchDto search);

    }
}
