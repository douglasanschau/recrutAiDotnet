using Dapper;
using System.Text;
using System.Data;
using recrutAiDotnet.Data;
using recrutAiDotnet.Models;

namespace recrutAiDotnet.Repositories;

public class UsuarioRepository
{
    private readonly Database _database;

    public UsuarioRepository(Database database)
    {
        _database = database;
    }

    public UsuarioModel? GetUsuarioByEmail(string email)
    {
        using var conn = _database.GetConnection();

        return conn.QueryFirstOrDefault<UsuarioModel>(
            @"SELECT *, empresa_id as empresaId FROM usuario
                WHERE email = @Email",
            new { Email = email }
        );
    }

    public UsuarioModel? GetUsuarioById(int id)
    {
        using var conn = _database.GetConnection();

        return conn.QueryFirstOrDefault<UsuarioModel>(
            @"SELECT id, nome, email, empresa_id as empresaId, perfil, senha, status FROM usuario
                WHERE id = @Id",
            new { Id = id }
        );
    }

    public int SaveUsuarioTransaction(UsuarioModel usuario, IDbConnection conn, IDbTransaction transaction)
    {
        return conn.ExecuteScalar<int>(
            @"INSERT INTO usuario
            (
                empresa_id,
                nome,
                email,
                status,
                perfil,
                senha
            )
            VALUES
            (
                @EmpresaId,
                @Nome,
                @Email,
                @Status,
                @Perfil,
                @Senha
            );

            SELECT LAST_INSERT_ID();",
            usuario, 
            transaction
        );
    }
}