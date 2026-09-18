using recrutAiDotnet.Enums;
using System.Text.RegularExpressions;

namespace recrutAiDotnet.Models;

public class ContratarPlanoModel 
{
    public PlanoEmpresa Plano { get; set; }
    public string PaymentMethodId { get; set; } = string.Empty;
}