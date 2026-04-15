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

        if (!File.Exists(caminhoDestino))
        {
            File.Copy(caminhoOrigem, caminhoDestino);
        }

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
                comando.CommandText = "INSERT INTO Alunos (nome) VALUES ('Guilherme4343');";
                comando.ExecuteNonQuery();
                Debug.Log("Dados inseridos com sucesso!");
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
                        Debug.Log($"ID: {leitor["id"]} | Nome: {leitor["nome"]}");
                    }
                }
            }
        }
    }
}