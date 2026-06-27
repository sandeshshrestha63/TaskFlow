using TaskFlow.Models;

namespace TaskFlow.Interfaces
{
    public interface ICompanyService
    {
        Task<Company> AddCompanyAsyc(Company company);
        Task<List<Company>> GetAllCompaniesAsync();
        Task<Company?> GetCompanyByIdAsync(int id);
        Task UpdateCompanyAsync(Company company);
        Task DeleteCompanyAsync(int id);
    }
}
