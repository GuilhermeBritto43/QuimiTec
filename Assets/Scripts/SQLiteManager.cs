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
                        SceneManager.LoadScene("gerenciarAlunos"); 
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