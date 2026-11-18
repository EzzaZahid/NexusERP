
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NexusERP_D2.Models;
using NexusERP_D2.Models.ViewModels;

namespace NexusERP_D2.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminController(
            UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        // Display all users
        public async Task<IActionResult> Index()
        {
            var users = _userManager.Users.ToList();

            var model = new UserManagementViewModel();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);

                model.Users.Add(new UserRowViewModel
                {
                    Id = user.Id,
                    FullName = user.FullName,
                    Email = user.Email,
                    Department = user.Department,
                    Role = roles.FirstOrDefault() ?? "None"
                });
            }

            return View(model);
        }

        // Change user role
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeRole(
            string userId,
            string role)
        {
            if (string.IsNullOrWhiteSpace(userId) ||
                (role != "Admin" && role != "Employee"))
            {
                return BadRequest();
            }

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return NotFound();

            var currentRoles =
                await _userManager.GetRolesAsync(user);

            // Prevent removing the last administrator
            if (currentRoles.Contains("Admin") &&
                role == "Employee")
            {
                var admins =
                    await _userManager.GetUsersInRoleAsync("Admin");

                if (admins.Count <= 1)
                {
                    TempData["Error"] =
                        "You cannot remove the last administrator.";

                    return RedirectToAction(nameof(Index));
                }
            }

            if (currentRoles.Contains(role))
            {
                return RedirectToAction(nameof(Index));
            }

            var removeResult =
                await _userManager.RemoveFromRolesAsync(
                    user, currentRoles);

            if (!removeResult.Succeeded)
            {
                TempData["Error"] = "Could not update existing role.";

                return RedirectToAction(nameof(Index));
            }

            var addResult =
                await _userManager.AddToRoleAsync(user, role);

            if (addResult.Succeeded)
            {
                TempData["Success"] =
                    "User role updated successfully.";
            }
            else
            {
                // Restore previous roles if new assignment fails
                await _userManager.AddToRolesAsync(
                    user, currentRoles);

                TempData["Error"] =
                    string.Join(", ",
                        addResult.Errors.Select(e => e.Description));
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
