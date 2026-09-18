using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Linq;

namespace recrutAiDotnet.Helpers;

public class ValidatorHelper 
{
    public static bool GetDocumentoCnpj(string documento)
    {
        documento = new string(documento.Where(char.IsDigit).ToArray());
        return (documento.Length == 14);
    }
}