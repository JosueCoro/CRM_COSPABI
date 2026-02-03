using CRM_COSPABI.DTOs;

namespace CRM_COSPABI.Service.Interfaces
{
    public interface IFacturaService
    {
        Task<FacturaDto> GenerarFacturaAsync(GenerarFacturaDto dto);
        Task<FacturaDto?> ObtenerPorIdAsync(int id);
        Task<IEnumerable<FacturaDto>> ListarAsync();
        Task<IEnumerable<FacturaDto>> ListarPorClienteAsync(int idCliente);
        Task<bool> AnularFacturaAsync(int id);
    }
}
