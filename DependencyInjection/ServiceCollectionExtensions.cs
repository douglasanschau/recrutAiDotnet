using Microsoft.Extensions.DependencyInjection;

using recrutAiDotnet.Repositories;
using recrutAiDotnet.Services;
using recrutAiDotnet.Data;

namespace recrutAiDotnet.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        //Repository 
        services.AddScoped<CadastroRepository>();
        services.AddScoped<UsuarioRepository>();
        services.AddScoped<EmpresaRepository>();

        // Services
        services.AddScoped<JwtService>();
        services.AddScoped<StripeService>();
        services.AddScoped<UsuarioService>();

        // Data
        services.AddScoped<Database>();

        return services;
    }
}