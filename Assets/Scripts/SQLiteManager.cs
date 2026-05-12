using UnityEngine;
using UnityEngine.SceneManagement;
using Mono.Data.Sqlite;
using System.Data;
using System.IO;
using TMPro;

public class SQLiteManager : MonoBehaviour
{
    private string caminhoDB;
    public TMP_Text textoListaAlunos;

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
        Debug.Log(caminhoDestino);
    }

    public void AdicionarAluno(string nome, string email, string senha)
    {
        using (var conexao = new SqliteConnection(caminhoDB))
        {
            conexao.Open();
            using (var comando = conexao.CreateCommand())
            {
                comando.CommandText = @"
            INSERT INTO Usuarios (nome, email, senha)
            SELECT @nome, @email, @senha
            WHERE NOT EXISTS (
                SELECT 1 FROM Usuarios WHERE email = @email
            );";

                comando.Parameters.Add(new SqliteParameter("@nome", nome));
                comando.Parameters.Add(new SqliteParameter("@email", email));
                comando.Parameters.Add(new SqliteParameter("@senha", senha));

                int linhas = comando.ExecuteNonQuery();

                if (linhas > 0)
                    Debug.Log("Aluno cadastrado");
                else
                    Debug.Log("Email já existe");
            }
        }
    }

    public void ListarAlunos()
    {
        textoListaAlunos.text = "";

        using (var conexao = new SqliteConnection(caminhoDB))
        {
            conexao.Open();

            using (var comando = conexao.CreateCommand())
            {
                comando.CommandText = "SELECT * FROM Usuarios;";

                using (IDataReader leitor = comando.ExecuteReader())
                {
                    while (leitor.Read())
                    {
                        string linha =
                            $"ID: {leitor["id"]} | " +
                            $"Nome: {leitor["nome"]} | " +
                            $"Email: {leitor["email"]}\n";

                        textoListaAlunos.text += linha;
                    }
                }
            }
        }
    }

    public void RemoverAluno(string email)
    {
        using (var conexao = new SqliteConnection(caminhoDB))
        {
            conexao.Open();
            using (var comando = conexao.CreateCommand())
            {
                comando.CommandText = "DELETE FROM Usuarios WHERE email = @email;";
                comando.Parameters.Add(new SqliteParameter("@email", email));

                int linhas = comando.ExecuteNonQuery();

                if (linhas > 0)
                    Debug.Log("Aluno removido");
                else
                    Debug.Log("Aluno não encontrado");
            }
        }
    }

    public void ValidarLogin(string email, string senha)
    {
        using (var conexao = new SqliteConnection(caminhoDB))
        {
            conexao.Open();

            using (var comando = conexao.CreateCommand())
            {
                comando.CommandText = @"
            SELECT * 
            FROM Usuarios 
            WHERE email = @email AND senha = @senha;";

                comando.Parameters.Add(new SqliteParameter("@email", email));
                comando.Parameters.Add(new SqliteParameter("@senha", senha));

                using (IDataReader leitor = comando.ExecuteReader())
                {
                    if (leitor.Read())
                    {
                        string nome = leitor["nome"].ToString();

                        DadosJogador.nome = nome;
                        DadosJogador.email = email;

                        Debug.Log("Login válido");
                        Debug.Log("Nome do jogador: " + nome);

                        if (email.EndsWith("@aluno.cps.sp.gov.br") || email == "a43")
                        {
                            Debug.Log("ALUNO");
                            SceneManager.LoadScene("TelaJogarEstudar");
                        }
                        else if (email.EndsWith("@cps.sp.gov.br") || email == "p43")
                        {
                            Debug.Log("PROFESSOR");
                            SceneManager.LoadScene("telaProfessor");
                        }
                        else
                        {
                            Debug.Log("Domínio desconhecido");
                        }
                    }
                    else
                    {
                        Debug.Log("Email ou senha inválidos");
                    }
                }
            }
        }
    }
}