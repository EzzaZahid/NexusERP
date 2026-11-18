using NexusERP_D2.Models;

namespace NexusERP_D2.Repositories
{
    public interface IClientRepository
    {
        Task<List<Client>> GetAllAsync(string? search = null);
        Task<Client?> GetByIdAsync(int id);
        Task AddAsync(Client client);
        Task UpdateAsync(Client client);
        Task DeleteAsync(Client client);
    }
}