using CRM_COSPABI.DTOs;

namespace CRM_COSPABI.Service.Interfaces
{
    public interface ILecturaService
    {
        Task<LecturaResponseDto> RegistrarLecturaAsync(LecturaCreateDto lecturaDto, int idUsuarioAdmin); // Requires admin ID who took the reading
        Task<List<LecturaResponseDto>> ObtenerLecturasPorClienteAsync(int idCliente);
        Task<LecturaResponseDto> ObtenerLecturaAsync(int idLectura);
    }
}
