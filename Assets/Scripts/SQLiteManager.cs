using UnityEngine;
using UnityEngine.SceneManagement;
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
                        Debug.Log($"ID: {leitor["id"]} | Nome: {leitor["nome"]} | Email: {leitor["email"]}");
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
                SELECT COUNT(*) 
                FROM Usuarios 
                WHERE email = @email AND senha = @senha;";

                comando.Parameters.Add(new SqliteParameter("@email", email));
                comando.Parameters.Add(new SqliteParameter("@senha", senha));

                int resultado = System.Convert.ToInt32(comando.ExecuteScalar());

                if (resultado > 0)
                {
                    Debug.Log("Login válido");

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
                        Debug.Log("Dominio desconhecido");
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