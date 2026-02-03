namespace CRM_COSPABI.DTOs
{
    public class LoginResponseDto
    {
        public string Token { get; set; } = null!;
        public string Usuario { get; set; } = null!;
        public string Rol { get; set; } = null!;
        public List<string> Permisos { get; set; } = new List<string>();
    }
}
