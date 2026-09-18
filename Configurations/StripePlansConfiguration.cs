using recrutAiDotnet.Enums;

namespace RecrutAiDotnet.Configurations;

public static class StripePlansConfiguration
{
    public static readonly IReadOnlyDictionary<PlanoEmpresa, string> Products =
        new Dictionary<PlanoEmpresa, string>
        {
            {
                PlanoEmpresa.Standart,
                "prod_VH1KzQcaExOIAg"
            },
            {
                 PlanoEmpresa.Premium,
                "prod_VH1LAtgKtFvU1S"
            }
        };
}