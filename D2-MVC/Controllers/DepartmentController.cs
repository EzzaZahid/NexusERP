
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NexusERP_D2.Models;
using NexusERP_D2.Repositories;

namespace NexusERP_D2.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DepartmentController : Controller
    {
        private readonly IDepartmentRepository _repository;

        public DepartmentController(IDepartmentRepository repository)
        {
            _repository = repository;
        }

        public async Task<IActionResult> Index()
        {
            var departments = await _repository.GetAllAsync();
            return View(departments);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Department department)
        {
            if (!ModelState.IsValid)
                return View(department);

            if (await _repository.ExistsByNameAsync(department.Name))
            {
                ModelState.AddModelError("Name", "This department already exists.");
                return View(department);
            }

            await _repository.AddAsync(department);

            TempData["Success"] = "Department created successfully.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var department = await _repository.GetByIdAsync(id);

            if (department == null)
                return NotFound();

            return View(department);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Department department)
        {
            if (id != department.Id)
                return BadRequest();

            if (!ModelState.IsValid)
                return View(department);

            var existing = await _repository.GetByIdAsync(id);

            if (existing == null)
                return NotFound();

            if (await _repository.ExistsByNameAsync(department.Name, id))
            {
                ModelState.AddModelError("Name", "This department already exists.");
                return View(department);
            }

            string oldName = existing.Name;

            existing.Name = department.Name;
            existing.Description = department.Description;

            await _repository.UpdateAsync(existing, oldName);

            TempData["Success"] = "Department updated successfully.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var department = await _repository.GetByIdAsync(id);

            if (department == null)
                return NotFound();

            return View(department);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var department = await _repository.GetByIdAsync(id);

            if (department == null)
                return NotFound();

            if (await _repository.HasEmployeesAsync(department.Name))
            {
                TempData["Error"] =
                    "Cannot delete this department because employees are assigned to it.";

                return RedirectToAction(nameof(Index));
            }

            await _repository.DeleteAsync(department);

            TempData["Success"] = "Department deleted successfully.";
            return RedirectToAction(nameof(Index));
        }
    }
}
