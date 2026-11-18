
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NexusERP_D2.Models;
using NexusERP_D2.Models.ViewModels;
using NexusERP_D2.Repositories;

namespace NexusERP_D2.Controllers
{
    [Authorize(Roles = "Admin")]
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository _repository;
        private readonly IDepartmentRepository _departmentRepository;

        public EmployeeController(
            IEmployeeRepository employeeRepository,
            IDepartmentRepository departmentRepository)
        {
            _repository = employeeRepository;
            _departmentRepository = departmentRepository;
        }

        // Employee listing and search
        public async Task<IActionResult> Index(
            string? search,
            string? department)
        {
            var employees = await _repository.GetAllAsync();

            if (!string.IsNullOrWhiteSpace(search))
            {
                employees = employees.Where(e =>
                    e.FullName.Contains(
                        search, StringComparison.OrdinalIgnoreCase) ||
                    e.EmployeeNumber.Contains(
                        search, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            if (!string.IsNullOrWhiteSpace(department))
            {
                employees = employees
                    .Where(e => e.Department == department)
                    .ToList();
            }

            ViewBag.Search = search;
            ViewBag.Department = department;

            // Load departments for the Index filter too.
            await LoadDepartmentsAsync(department);

            return View(employees);
        }

        // Display create form
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await LoadDepartmentsAsync();

            return View(new EmployeeViewModel
            {
                JoiningDate = DateTime.Today,
                EmploymentStatus = "Active"
            });
        }

        // Save new employee
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmployeeViewModel model)
        {
            if (ModelState.IsValid &&
                !await _departmentRepository.ExistsByNameAsync(model.Department))
            {
                ModelState.AddModelError(
                    "Department",
                    "Please select a valid department.");
            }

            if (!ModelState.IsValid)
            {
                await LoadDepartmentsAsync(model.Department);
                return View(model);
            }

            if (await _repository.EmployeeNumberExistsAsync(model.EmployeeNumber))
            {
                ModelState.AddModelError(
                    "EmployeeNumber",
                    "This employee number already exists.");

                await LoadDepartmentsAsync(model.Department);
                return View(model);
            }

            var employee = new Employee
            {
                EmployeeNumber = model.EmployeeNumber,
                FullName = model.FullName,
                Department = model.Department,
                Designation = model.Designation,
                JoiningDate = model.JoiningDate,
                EmploymentStatus = model.EmploymentStatus
            };

            await _repository.AddAsync(employee);

            TempData["Success"] = "Employee added successfully.";

            return RedirectToAction(nameof(Index));
        }

        // Display edit form
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var employee = await _repository.GetByIdAsync(id);

            if (employee == null)
                return NotFound();

            var model = new EmployeeViewModel
            {
                Id = employee.Id,
                EmployeeNumber = employee.EmployeeNumber,
                FullName = employee.FullName,
                Department = employee.Department,
                Designation = employee.Designation,
                JoiningDate = employee.JoiningDate,
                EmploymentStatus = employee.EmploymentStatus
            };

            await LoadDepartmentsAsync(model.Department);

            return View(model);
        }

        // Update employee
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EmployeeViewModel model)
        {
            if (ModelState.IsValid &&
                !await _departmentRepository.ExistsByNameAsync(model.Department))
            {
                ModelState.AddModelError(
                    "Department",
                    "Please select a valid department.");
            }

            if (!ModelState.IsValid)
            {
                await LoadDepartmentsAsync(model.Department);
                return View(model);
            }

            var employee = await _repository.GetByIdAsync(model.Id);

            if (employee == null)
                return NotFound();

            if (await _repository.EmployeeNumberExistsAsync(
                model.EmployeeNumber, model.Id))
            {
                ModelState.AddModelError(
                    "EmployeeNumber",
                    "This employee number already exists.");

                await LoadDepartmentsAsync(model.Department);
                return View(model);
            }

            employee.EmployeeNumber = model.EmployeeNumber;
            employee.FullName = model.FullName;
            employee.Department = model.Department;
            employee.Designation = model.Designation;
            employee.JoiningDate = model.JoiningDate;
            employee.EmploymentStatus = model.EmploymentStatus;

            await _repository.UpdateAsync(employee);

            TempData["Success"] = "Employee updated successfully.";

            return RedirectToAction(nameof(Index));
        }

        // Display delete confirmation
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var employee = await _repository.GetByIdAsync(id);

            if (employee == null)
                return NotFound();

            return View(employee);
        }

        // Confirm deletion
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employee = await _repository.GetByIdAsync(id);

            if (employee == null)
                return NotFound();

            await _repository.DeleteAsync(employee);

            TempData["Success"] = "Employee deleted successfully.";

            return RedirectToAction(nameof(Index));
        }

        // Populate department dropdowns
        private async Task LoadDepartmentsAsync(
            string? selectedDepartment = null)
        {
            var departments = await _departmentRepository.GetAllAsync();

            ViewBag.Departments = new SelectList(
                departments,
                "Name",
                "Name",
                selectedDepartment
            );
        }
    }
}
