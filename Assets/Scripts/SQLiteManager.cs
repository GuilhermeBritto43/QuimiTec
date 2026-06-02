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
    public GameObject Popup;


    void Start()
    {
        caminhoDB = DatabaseManager.CaminhoDB;
        Debug.Log(Application.persistentDataPath);
        Debug.Log("aaaa " + caminhoDB);
        AtualizarEstruturaBanco();
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

    void AtualizarEstruturaBanco()
    {
        using (var conexao = new SqliteConnection(caminhoDB))
        {
            conexao.Open();

            using (var comando = conexao.CreateCommand())
            {
                comando.CommandText = @"
            ALTER TABLE Usuarios
            ADD COLUMN recorde INTEGER DEFAULT 0;
            ";

                try
                {
                    comando.ExecuteNonQuery();
                    Debug.Log("Coluna recorde adicionada!");
                }
                catch
                {
                    Debug.Log("Coluna recorde já existe.");
                }
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
                        int recorde = int.Parse(leitor["recorde"].ToString());

                        DadosJogador.nome = nome;
                        DadosJogador.email = email;
                        DadosJogador.recorde = recorde;

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
                            MostrarPopup();
                        }
                    }
                    else
                    {
                        Debug.Log("Email ou senha inválidos");
                        MostrarPopup();
                    }
                }
            }
        }
    }
    public void MostrarPopup()
    {
        Popup.SetActive(true);
    }
    public void FecharPopup()
    {
        Popup.SetActive(false);
    }
    
}