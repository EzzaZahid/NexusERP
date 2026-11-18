
using Microsoft.EntityFrameworkCore;
using NexusERP_D2.Data;
using NexusERP_D2.Models;

namespace NexusERP_D2.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ApplicationDbContext _context;

        public EmployeeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Employee>> GetAllAsync()
        {
            return await _context.Employees
                .AsNoTracking()
                .OrderBy(e => e.FullName)
                .ToListAsync();
        }

        public async Task<Employee?> GetByIdAsync(int id)
        {
            return await _context.Employees
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task AddAsync(Employee employee)
        {
            await _context.Employees.AddAsync(employee);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Employee employee)
        {
            _context.Employees.Update(employee);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Employee employee)
        {
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> EmployeeNumberExistsAsync(
            string employeeNumber,
            int? excludeId = null)
        {
            return await _context.Employees.AnyAsync(e =>
                e.EmployeeNumber == employeeNumber &&
                (!excludeId.HasValue || e.Id != excludeId.Value));
        }
    }
}
