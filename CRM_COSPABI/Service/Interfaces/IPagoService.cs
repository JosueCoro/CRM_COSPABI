using CRM_COSPABI.DTOs;

namespace CRM_COSPABI.Service.Interfaces
{
    public interface IPagoService
    {
        Task<LibelulaDeudaResponseDto> IniciarPagoAsync(IniciarPagoDto pagoDto);
        Task<bool> ProcesarCallbackAsync(LibelulaCallbackDto callbackDto);
    }
}
