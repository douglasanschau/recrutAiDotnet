using recrutAiDotnet.Enums;

namespace recrutAiDotnet.Models;

public class LoginModel 
{
    public string Email { get; set; } = "";
    public string Senha { get; set; } = "";

     public string EmailFormatado => Email.Trim();
}