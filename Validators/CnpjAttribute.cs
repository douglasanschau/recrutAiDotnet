using System.ComponentModel.DataAnnotations;
using recrutAiDotnet.Helpers;

namespace recrutAiDotnet.Validators;

public class CnpjAttribute : ValidationAttribute
{

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if(value == null){
            return new ValidationResult("Documento é obrigatório");
        }
        
        if (!ValidatorHelper.GetDocumentoCnpj(value.ToString()!)){
            return new ValidationResult("CNPJ inválido");
        }

        return ValidationResult.Success;
    }
}