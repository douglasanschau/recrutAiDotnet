using recrutAiDotnet.Enums;
using System.Text.RegularExpressions;

namespace recrutAiDotnet.Models;

public class EmpresaModel 
{
    public int Id { get; set; }
    public string RazaoSocial { get; set; } = "";
    public string Cnpj { get; set; } = "";
    public PlanoEmpresa PlanoContratado { get; set; }
    public DateTime DataBloqueio { get; set; }
    public StatusEmpresa Status { get; set; }

    public string CnpjFormatado => Regex.Replace(Cnpj, @"\D", "");
}