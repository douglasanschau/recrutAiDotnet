using recrutAiDotnet.Enums;

namespace recrutAiDotnet.Models;

public class UsuarioModel 
{
    public int Id { get; set; }
    public int EmpresaId { get; set; }
    public string Nome { get; set; } = "";
    public string Email { get; set; } = "";
    public StatusUsuario Status { get; set; }
    public PerfilUsuario Perfil { get; set; } 
    public string Senha { get; set; } = "";

    public string Token { get; set; } = "";

     public string EmailFormatado => Email.Trim();
}