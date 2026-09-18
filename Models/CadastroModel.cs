using recrutAiDotnet.Enums;
using recrutAiDotnet.Validators;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace recrutAiDotnet.Models;

public class CadastroModel 
{
    [Required(ErrorMessage = "O nome é obrigatório")]
    public string Nome { get; set; } = "";

    [Required(ErrorMessage =  "O email é obrigatório")]
    public string Email { get; set; } = "";

    [Required(ErrorMessage =  "A razão social é obrigatório")]
    public string RazaoSocial { get; set; } = "";

    [Required(ErrorMessage =  "A senha é obrigatória")]
    public string Senha { get; set; } = "";

    [Cnpj]
    public string Cnpj { get; set; } = "";

    public string CnpjFormatado => Regex.Replace(Cnpj, @"\D", "");
    public string EmailFormatado => Email.Trim();
}