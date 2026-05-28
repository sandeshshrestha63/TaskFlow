using TaskFlow.Data;
using TaskFlow.Models;

namespace TaskFlow.Services
{
    public class CompanyService
    {
        private readonly AppDbContext _context;

        public CompanyService(AppDbContext context)
        {
            _context = context;
        }

        public void AddCompany(Company company)
        {
            _context.Companies.Add(company);
            _context.SaveChanges();
        }

        public List<Company> GetAllCompanies()
        {
            return _context.Companies.ToList();
        }

        public Company? GetCompanyById(int id)
        {
            return _context.Companies.FirstOrDefault(x => x.Id == id);
        }

        public void UpdateCompany(Company company)
        {
            var existing = _context.Companies.FirstOrDefault(x => x.Id == company.Id);

            if (existing != null)
            {
                existing.Name = company.Name;
                existing.Email = company.Email;
                existing.PhoneNumber = company.PhoneNumber;
                existing.Address = company.Address;
                existing.IsActive = company.IsActive;

                _context.SaveChanges();
            }
        }

        public void DeleteCompany(int id)
        {
            var company = _context.Companies.Find(id);
            if (company != null)
            {
                _context.Companies.Remove(company);
                _context.SaveChanges();
            }
        }
    }
}
