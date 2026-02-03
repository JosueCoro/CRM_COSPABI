using CRM_COSPABI.Models;

namespace CRM_COSPABI.Service.Interfaces
{
    public interface IClienteService
    {
        Task<IEnumerable<Cliente>> ListarAsync();
        Task<Cliente?> ObtenerPorIdAsync(int id);
        Task<Cliente> CrearAsync(Cliente cliente);
        Task<Cliente?> ActualizarAsync(int id, Cliente cliente);
        Task<bool> EliminarAsync(int id);
    }
}
