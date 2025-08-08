using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.Repositories;
using DevSkill.Inventory.Domain.Utilities;
using DevSkill.Inventory.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DevSkill.Inventory.Infrastructure
{
    public class ApplicationUnitOfWork : UnitOfWork,IApplicationUnitOfWork
    {

        public IProductRepository ProductRepository { get; private set; }

        public ICustomerRepository CustomerRepository { get; private set; }

        public ICategoryRepository CategoryRepository { get; private set; }
        public IExpenseRepository ExpenseRepository { get; private set; }
        public IDepartmentRepository DepartmentRepository { get; private set; }
        public IUnitRepository UnitRepository { get; private set; }

        public ISaleTypeRepository SaleTypesRepository { get; }
        public IUserRepository UserRepository { get; }
        public IStaffRepository StaffRepository { get; }
        public ISupplierRepository SupplierRepository { get; }

        public ISalesRepository SalesRepository { get; }
        public IAccountTypeRepository AccountTypesRepository { get; }
        public IAccountSearchRepository AccountSearchRepository { get; }

        public IUserRoleRepository UserRoleRepository { get; }
        public ICustomerLedgerRepository CustomerLedgerRepository { get; }


        public ICashAccountRepository CashAccountRepository { get; private set; }

        public IBankAccountRepository BankAccountRepository { get; private set; }
        public IMobileAccountRepository MobileAccountRepository { get; private set; }
        public IBalanceTransferRepository BalanceTransferRepository { get; private set; }

        public IUserPermissionRepository UserPermissionRepository { get; private set; }

        public ICompanyProfileRepository CompanyProfileRepository { get; private set; }

        public ISqsRepository SqsRepository { get; private set; }

        public IS3Repository S3Repository { get; private set; }


        private readonly ApplicationDbContext _dbContext;
        public ApplicationUnitOfWork(ApplicationDbContext dbContext,IProductRepository productRepository,
            ICustomerRepository customerRepository, ICategoryRepository categoryRepository,
            IUnitRepository unitRepository, ICashAccountRepository cashAccountRepository,
            ISaleTypeRepository saleTypesRepository, IAccountTypeRepository accountTypeRepository,
            IAccountSearchRepository accountSearchRepository, IBalanceTransferRepository balanceTransferRepository
            , IDepartmentRepository departmentRepository, ISalesRepository salesRepository,
            IUserRoleRepository userRoleRepository, IStaffRepository staffRepository, IUserRepository userRepository,
            ICustomerLedgerRepository customerLadgerRepository, IUserPermissionRepository userPermissionRepository
            , IBankAccountRepository bankAccountRepository, ISupplierRepository supplierRepository,
            IMobileAccountRepository mobileAccountRepository, IExpenseRepository expenseRepository,
            ICompanyProfileRepository companyProfileRepository,
            ISqsRepository sqsRepository,IS3Repository s3Repository) : base(dbContext)
        {

            ProductRepository = productRepository;
            CustomerRepository = customerRepository;
            CategoryRepository = categoryRepository;
            UnitRepository = unitRepository;
            CashAccountRepository = cashAccountRepository;
            SaleTypesRepository = saleTypesRepository;
            AccountTypesRepository = accountTypeRepository;
            _dbContext = dbContext;
            AccountSearchRepository = accountSearchRepository;
            BalanceTransferRepository = balanceTransferRepository;
            DepartmentRepository = departmentRepository;
            SalesRepository = salesRepository;
            UserRoleRepository = userRoleRepository;
            StaffRepository = staffRepository;
            UserRepository = userRepository;
            CustomerLedgerRepository = customerLadgerRepository;
            UserPermissionRepository = userPermissionRepository;
            BankAccountRepository = bankAccountRepository;
            SupplierRepository = supplierRepository;
            MobileAccountRepository = mobileAccountRepository;
            ExpenseRepository = expenseRepository;
            CompanyProfileRepository = companyProfileRepository;
            SqsRepository = sqsRepository;
            S3Repository = s3Repository;
        }

        public async Task<(IList<Customer> data, int total, int totalDisplay)> GetCutomersSP(int pageIndex,
        int pageSize, string? order, CustomerSearchDto search)
        {
            var procedureName = "GetCompanies";

            var result = await SqlUtility.QueryWithStoredProcedureAsync<Customer>(procedureName,
                new Dictionary<string, object>
                {
                    { "PageIndex", pageIndex },
                    { "PageSize", pageSize },
                    { "OrderBy", order },
                    { "RatingFrom", search.RatingFrom },
                    { "RatingTo", search.RatingTo },
                    { "Name", string.IsNullOrEmpty(search.Name) ? null : search.Name },
                    { "Description", string.IsNullOrEmpty(search.Description) ? null : search.Description }
                },
                new Dictionary<string, Type>
                {
                    { "Total", typeof(int) },
                    { "TotalDisplay", typeof(int) },
                });

            return  (result.result, (int)result.outValues["Total"], (int)result.outValues["TotalDisplay"]);
        }



        public IImageResizeQueueRepository ImageResizeQueueRepository =>
           _imageResizeQueueRepository ??= new ImageResizeQueueRepository(_dbContext);

        public IAccountTypeRepository AccountTypeRepository => throw new NotImplementedException();

       // public IS3Repository S3epository => throw new NotImplementedException();

        // public ICustomerLedgerRepository customerLedgerRepository => throw new NotImplementedException();

        private IImageResizeQueueRepository _imageResizeQueueRepository;


    }
}
