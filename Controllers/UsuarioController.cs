using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using recrutAiDotnet.Helpers;
using recrutAiDotnet.Services;
using recrutAiDotnet.Models;

namespace recrutAiDotnet.Controllers;

[ApiController]
[Route("api")]

public class UsuarioController : BaseController
{
    private readonly UsuarioService _usuarioService;
    private readonly JwtService _jwtService;

    public UsuarioController(UsuarioService usuarioService, JwtService jwtService)
    {
        _usuarioService = usuarioService;
        _jwtService = jwtService;
    }

    [HttpPost("cadastro")]
    public IActionResult Cadastro([FromBody] CadastroModel model)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(
                    ResponsesHelper.BadRequest("Dados informados inválidos", ModelState)
                );
            }

            dynamic response = _usuarioService.NovoCadastroRecrutai(model);

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

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginModel model)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(
                    ResponsesHelper.BadRequest("Dados informados inválidos", ModelState)
                );
            }

            dynamic response = _usuarioService.LoginRecrutai(model);

            var usuario = response.details.usuario ?? null;
            
            if(response.statusCode == 200)
            {
                var token   = _jwtService.GenerateToken(usuario);
                usuario.Token = token;
            }

            return StatusCode(
                response.statusCode,
                new
                {
                    message = response.details.message,
                    usuario = usuario
                }
            );
        }
        catch (Exception ex)
        {
            return StatusCode(
                500,
                ResponsesHelper.Problem($"Não foi possível logar o novo usuario. Detalhes: {ex.Message}")
            );
        }
    }


}