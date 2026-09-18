using Dapper;
using System.Text;
using recrutAiDotnet.Data;
using recrutAiDotnet.Models;
using recrutAiDotnet.Repositories;

namespace recrutAiDotnet.Repositories;

public class CadastroRepository
{
    private readonly Database _database;

    private readonly UsuarioRepository _usuarioRepository;
    private readonly EmpresaRepository _empresaRepository;


    public CadastroRepository(Database database, EmpresaRepository empresaRepository, UsuarioRepository usuarioRepository)
    {
        _database = database;

        _empresaRepository = empresaRepository;
        _usuarioRepository = usuarioRepository;

    }

    public int CadastrarEmpresaComUsuario(EmpresaModel empresa, UsuarioModel usuario)
    {
        using var conn = _database.GetConnection();

        conn.Open();
        
        using var transaction = conn.BeginTransaction();

        try
        {
            var empresaId = _empresaRepository.SaveEmpresaTransaction(
                empresa,
                conn,
                transaction
            );

            usuario.EmpresaId = empresaId;

            var usuarioId = _usuarioRepository.SaveUsuarioTransaction(
                usuario,
                conn,
                transaction
            );

            transaction.Commit();

            return usuarioId;
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }
}