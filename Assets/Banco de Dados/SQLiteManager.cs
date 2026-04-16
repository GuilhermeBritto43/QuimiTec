using UnityEngine;
using Mono.Data.Sqlite;
using System.Data;
using System.IO;

public class SQLiteManager : MonoBehaviour
{
    private string caminhoDB;

    void Start()
    {
        string nomeBanco = "QuimiTec.db";
        string caminhoOrigem = Path.Combine(Application.streamingAssetsPath, nomeBanco);
        string caminhoDestino = Path.Combine(Application.persistentDataPath, nomeBanco);

        File.Copy(caminhoOrigem, caminhoDestino, true);

        caminhoDB = "URI=file:" + caminhoDestino;

        CriarDadosExemplo();
        LerDados();
    }

    public void CriarDadosExemplo()
    {
        using (var conexao = new SqliteConnection(caminhoDB))
        {
            conexao.Open();
            using (var comando = conexao.CreateCommand())
            {
                comando.CommandText = @"
                INSERT INTO Alunos (email, senha)
                SELECT 'guilherme43@cps.sp.gov.br', '4343'
                WHERE NOT EXISTS (
                    SELECT 1 FROM Alunos WHERE email = 'guilherme43@cps.sp.gov.br'
                );";

                comando.ExecuteNonQuery();
                Debug.Log("Inserção verificada (sem duplicar).");
            }
        }
    }

    public void LerDados()
    {
        using (var conexao = new SqliteConnection(caminhoDB))
        {
            conexao.Open();
            using (var comando = conexao.CreateCommand())
            {
                comando.CommandText = "SELECT * FROM Alunos;";
                using (IDataReader leitor = comando.ExecuteReader())
                {
                    while (leitor.Read())
                    {
                        Debug.Log($"ID: {leitor["id"]} | Email: {leitor["email"]} | Senha: {leitor["senha"]}");
                    }
                }
            }
        }
    }
}