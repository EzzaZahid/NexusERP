using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NexusERP_D2.Models;
using NexusERP_D2.Repositories;

namespace NexusERP_D2.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ClientController : Controller
    {
        private readonly IClientRepository _repository;

        public ClientController(IClientRepository repository)
        {
            _repository = repository;
        }

        public async Task<IActionResult> Index(string? search)
        {
            ViewBag.Search = search;
            var clients = await _repository.GetAllAsync(search);
            return View(clients);
        }

        public IActionResult Create()
        {
            return View(new Client());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Client client)
        {
            if (!ModelState.IsValid)
                return View(client);

            client.CreatedDate = DateTime.Today;

            await _repository.AddAsync(client);

            TempData["Success"] = "Client added successfully.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var client = await _repository.GetByIdAsync(id);

            if (client == null)
                return NotFound();

            return View(client);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Client client)
        {
            if (id != client.Id)
                return BadRequest();

            if (!ModelState.IsValid)
                return View(client);

            var existingClient = await _repository.GetByIdAsync(id);

            if (existingClient == null)
                return NotFound();

            existingClient.Name = client.Name;
            existingClient.ContactPerson = client.ContactPerson;
            existingClient.Email = client.Email;
            existingClient.Phone = client.Phone;
            existingClient.Address = client.Address;
            existingClient.ClientType = client.ClientType;

            await _repository.UpdateAsync(existingClient);

            TempData["Success"] = "Client updated successfully.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var client = await _repository.GetByIdAsync(id);

            if (client == null)
                return NotFound();

            return View(client);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var client = await _repository.GetByIdAsync(id);

            if (client == null)
                return NotFound();

            await _repository.DeleteAsync(client);

            TempData["Success"] = "Client deleted successfully.";
            return RedirectToAction(nameof(Index));
        }
    }
}