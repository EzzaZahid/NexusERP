
using NexusERP_D2.Models;

namespace NexusERP_D2.Repositories
{
    public interface IDepartmentRepository
    {
        Task<List<Department>> GetAllAsync();
        Task<Department?> GetByIdAsync(int id);
        Task AddAsync(Department department);
        Task UpdateAsync(Department department, string oldName);
        Task DeleteAsync(Department department);
        Task<bool> ExistsByNameAsync(string name, int? excludeId = null);
        Task<bool> HasEmployeesAsync(string departmentName);
    }
}
