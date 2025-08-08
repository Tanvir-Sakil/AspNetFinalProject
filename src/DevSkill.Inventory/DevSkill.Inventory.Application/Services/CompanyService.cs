using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Services;
using DevSkill.Inventory.Application.Exceptions;
using DevSkill.Inventory.Domain.Dtos;

namespace DevSkill.Inventory.Application.Services
{
   public class CompanyService : ICompanyService
    {
     /*   private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        public CompanyService(IApplicationUnitOfWork applicationUnitOfWork)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
        }
        public void AddCompany(Company company)
        {
            if (!_applicationUnitOfWork.CompanyRepository.IsNameDuplicate(company.Name))
            {
                _applicationUnitOfWork.CompanyRepository.Add(company);
                _applicationUnitOfWork.Save();
            }
            else
                throw new DuplicateCompanyNameException();
        }
        public void DeleteCompany(Guid id)
        {
            _applicationUnitOfWork.CompanyRepository.Remove(id);
            _applicationUnitOfWork.Save();
        }

        public Company GetCompany(Guid id)
        {
            return _applicationUnitOfWork.CompanyRepository.GetById(id);
        }


        public void Update(Company company)
        {
            if (!_applicationUnitOfWork.CompanyRepository.IsNameDuplicate(company.Name, company.Id))
            {
                _applicationUnitOfWork.CompanyRepository.Update(company);
                _applicationUnitOfWork.Save();
            }
            else
                throw new DuplicateCompanyNameException();

        }
        public (IList<Company> data, int total, int totalDisplay) GetCompanies(int pageIndex,
            int pageSize, string? order, DataTablesSearch search)
        {
            return _applicationUnitOfWork.CompanyRepository.GetPagedCompanies(pageIndex, pageSize, order, search);
        }

        public async Task<(IList<Company> data, int total, int totalDisplay)> GetCompaniesSP(int pageIndex,
            int pageSize, string? order, CustomerSearchDto search)
        {
            return await _applicationUnitOfWork.GetCompaniesSP(pageIndex, pageSize, order, search);
        }*/
    }
}
