
using Microsoft.EntityFrameworkCore;
using NexusERP_D2.Data;
using NexusERP_D2.Models;

namespace NexusERP_D2.Repositories
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly ApplicationDbContext _context;

        public DepartmentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Department>> GetAllAsync()
        {
            return await _context.Departments
                .AsNoTracking()
                .OrderBy(d => d.Name)
                .ToListAsync();
        }

        public async Task<Department?> GetByIdAsync(int id)
        {
            return await _context.Departments.FindAsync(id);
        }

        public async Task AddAsync(Department department)
        {
            _context.Departments.Add(department);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Department department, string oldName)
        {
            // Keep existing employee records consistent when a department is renamed.
            var employees = await _context.Employees
                .Where(e => e.Department == oldName)
                .ToListAsync();

            foreach (var employee in employees)
            {
                employee.Department = department.Name;
            }

            _context.Departments.Update(department);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Department department)
        {
            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsByNameAsync(
            string name, int? excludeId = null)
        {
            return await _context.Departments.AnyAsync(d =>
                d.Name.ToLower() == name.ToLower() &&
                (!excludeId.HasValue || d.Id != excludeId.Value));
        }

        public async Task<bool> HasEmployeesAsync(string departmentName)
        {
            return await _context.Employees
                .AnyAsync(e => e.Department == departmentName);
        }
    }
}
