using recrutAiDotnet.Repositories;
using recrutAiDotnet.Models;
using recrutAiDotnet.Helpers;
using recrutAiDotnet.Enums;
using Microsoft.AspNetCore.Identity;
using System.Reflection.Metadata.Ecma335;

namespace recrutAiDotnet.Services;

public class UsuarioService
{
    private readonly CadastroRepository _cadastroRepository;
    private readonly UsuarioRepository _usuarioRepository;
    private readonly EmpresaRepository _empresaRepository;

    public UsuarioService(CadastroRepository cadastroRepository,
                          EmpresaRepository empresaRepository,
                          UsuarioRepository usuarioRepository)
    {
        _cadastroRepository = cadastroRepository;
        _empresaRepository = empresaRepository;
        _usuarioRepository = usuarioRepository;
    }

    public object NovoCadastroRecrutai(CadastroModel model)
    {
        var empresaCadastrada = _empresaRepository.GetEmpresaByCnpj(model.CnpjFormatado);

        if (empresaCadastrada is not null)
        {
            return new
            {
                statusCode = 500,
                details = new
                {
                    message = "Empresa já cadastrada"
                }
            };
        }

        var usuarioCadastrado = _usuarioRepository.GetUsuarioByEmail(model.EmailFormatado);

        if (usuarioCadastrado is not null)
        {
            return new
            {
                statusCode = 500,
                details = new
                {
                    message = "Usuário já cadastrado."
                }
            };
        }

        var empresa = new EmpresaModel
        {
            RazaoSocial = model.RazaoSocial,
            Cnpj = model.CnpjFormatado,
            PlanoContratado = PlanoEmpresa.Gratuito,
            DataBloqueio = DateTime.Now.AddDays(7),
            Status = StatusEmpresa.Ativo
        };


        var hasher = new PasswordHasher<UsuarioModel>();

        var novoUsuario = new UsuarioModel
        {
            Nome = model.Nome,
            Email = model.EmailFormatado,
            Status = StatusUsuario.Ativo,
            Perfil = PerfilUsuario.Administrador
        };

        novoUsuario.Senha = hasher.HashPassword(novoUsuario, model.Senha);

        var usuarioId = _cadastroRepository.CadastrarEmpresaComUsuario(empresa, novoUsuario);

        UsuarioModel? usuario = null;
        if (usuarioId > 0)
        {
            usuario = _usuarioRepository.GetUsuarioById(usuarioId);
        }

        return new
        {
            statusCode = 200,
            details = new
            {
                message = "Usuário cadastrado com sucesso.",
                usuario = usuario
            }
        };
    }

    public object LoginRecrutai(LoginModel model)
    {

        var usuario = _usuarioRepository.GetUsuarioByEmail(model.EmailFormatado);
        var resultado = PasswordVerificationResult.Failed;

        if (usuario is null)
        {
            return new
            {
                statusCode = 404,
                details = new
                {
                    message = "Usuário não cadastrado."
                }
            };
        } else
        {
            var hasher = new PasswordHasher<UsuarioModel>();

            resultado = hasher.VerifyHashedPassword(
                usuario,
                usuario.Senha,
                model.Senha
            );
        }

        if((resultado != PasswordVerificationResult.Success))
        {
            return new
            {
                statusCode = 401,
                details = new
                {
                    message = "Usuário ou senha inválidos."
                }
            };
        }

        var empresa   = _empresaRepository.GetEmpresaById(usuario.EmpresaId);

        if (empresa is null)
        {
            return new
            {
                statusCode = 404,
                details = new
                {
                    message = "Empresa não encontrada."
                }
            };
        }

        if(empresa.Status != StatusEmpresa.Ativo)
        {
            return new
            {
                statusCode = 500,
                details = new
                {
                    message = "Empresa desativada/inativada."
                }
            };
        }

        return new
        {
            statusCode = 200,
            details = new
            {
                message = "Usurio encontrado",
                usuario = usuario,
            }
        };
    }
}