using Dapper;
using System.Text;
using System.Data;
using recrutAiDotnet.Enums;
using recrutAiDotnet.Data;
using recrutAiDotnet.Models;

namespace recrutAiDotnet.Repositories;

public class EmpresaRepository
{
    private readonly Database _database;

    public EmpresaRepository(Database database)
    {
        _database = database;
    }

    public int SaveEmpresaTransaction(EmpresaModel empresa, IDbConnection conn, IDbTransaction transaction)
    {

        return conn.ExecuteScalar<int>(
            @"INSERT INTO empresa
            (
                razao_social,
                cnpj,
                plano_contratado,
                data_bloqueio,
                status
            )
            VALUES
            (
                @RazaoSocial,
                @Cnpj,
                @PlanoContratado,
                @DataBloqueio,
                @Status
            );

            SELECT LAST_INSERT_ID();",
            empresa,
            transaction
        );
    }

    public EmpresaModel? GetEmpresaByCnpj(string cnpj)
    {
        using var conn = _database.GetConnection();

        return conn.QueryFirstOrDefault<EmpresaModel>(
            @"SELECT * FROM empresa
                WHERE cnpj = @Cnpj",
            new { Cnpj = cnpj }
        );
    }

    public EmpresaModel? GetEmpresaById(int id)
    {
        using var conn = _database.GetConnection();

        return conn.QueryFirstOrDefault<EmpresaModel>(
            @"SELECT * FROM empresa
                WHERE id = @Id",
            new { Id = id }
        );
    }

    public void UpdatePlano(int empresaId, PlanoEmpresa plano)
    {
        using var conn = _database.GetConnection();

        conn.Execute(
            @"UPDATE empresa
                SET plano_contratado = @Plano
                WHERE id = @EmpresaId",
            new
            {
                Plano = plano,
                EmpresaId = empresaId
            }
        );
    }

}