using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TaskFlow.Data;
using TaskFlow.Exceptions;
using TaskFlow.Interfaces;
using TaskFlow.Models;

namespace TaskFlow.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly AppDbContext _context;

        public CompanyService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Company> AddCompanyAsyc(Company company)
        {
            if (string.IsNullOrWhiteSpace(company.Name))
                throw new ValidationException("Company name is required.");

            bool exists = await _context.Companies
                .AnyAsync(x => x.Name == company.Name);

            if (exists)
                throw new ValidationException("A company with this name already exists.");

            await _context.Companies.AddAsync(company);
            await _context.SaveChangesAsync();
            return company;
        }

        public async Task<List<Company>> GetAllCompaniesAsync()
        {
            return await _context.Companies.ToListAsync();
        }

        public async Task<Company?> GetCompanyByIdAsync(int id)
        {
            return await _context.Companies.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task UpdateCompanyAsync(Company company)
        {
            var existing = await _context.Companies.FirstOrDefaultAsync(x => x.Id == company.Id);

            if (existing != null)
            {
                existing.Name = company.Name;
                existing.Email = company.Email;
                existing.PhoneNumber = company.PhoneNumber;
                existing.Address = company.Address;
                existing.IsActive = company.IsActive;

                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteCompanyAsync(int id)
        {
            var company = await _context.Companies.FindAsync(id);
            if (company != null)
            {
                _context.Companies.Remove(company);
                await _context.SaveChangesAsync();
            }
        }
    }
}
