using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using recrutAiDotnet.Helpers;
using recrutAiDotnet.Services;
using recrutAiDotnet.Models;
using recrutAiDotnet.Enums;

namespace recrutAiDotnet.Controllers;

[Authorize]
[ApiController]
[Route("api/config")]

public class RecrutaiConfigController : BaseController
{
    private readonly StripeService _stripeService;

    public RecrutaiConfigController(StripeService stripeService)
    {
        _stripeService = stripeService;
    }

    [HttpPost("contratar_plano")]
    public async Task<IActionResult> ContratarPlano([FromBody] ContratarPlanoModel model)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(
                    ResponsesHelper.BadRequest("Dados informados inválidos", ModelState)
                );
            }

            dynamic response = await _stripeService.ContratarPlano(model, UsuarioId);

            return StatusCode(
                response.statusCode,
                response.details
            );
        }
        catch (Exception ex)
        {
            return StatusCode(
                500,
                ResponsesHelper.Problem($"Não foi possível cadastrar novo usuario. Detalhes: {ex.Message}")
            );
        }
    }

    [HttpPost("cadastrar_cartao_teste")]
    public async Task<IActionResult> CadastrarCartaoTeste()
    {
        var dados = await _stripeService.CadastrarCartaoTeste();
        return StatusCode(
               200,
               dados
        );
    }

}