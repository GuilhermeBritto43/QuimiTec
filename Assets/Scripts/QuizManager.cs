using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Mono.Data.Sqlite;
using System.Data;
using System.IO;
using System.Collections.Generic;

public class QuizManager : MonoBehaviour
{
    public TMP_Text textoPergunta;

    public Button[] botoes; 

    private string caminhoDB;

    void Start()
    {
        string nomeBanco = "QuimiTec.db";
        string caminhoOrigem = Path.Combine(Application.streamingAssetsPath, nomeBanco);
        string caminhoDestino = Path.Combine(Application.persistentDataPath, nomeBanco);

        File.Copy(caminhoOrigem, caminhoDestino, true);

        caminhoDB = "URI=file:" + caminhoDestino;

        CarregarPergunta("facil");
    }

    void CarregarPergunta(string dificuldade)
    {
        using (var conexao = new SqliteConnection(caminhoDB))
        {
            conexao.Open();

            int idPergunta = -1;
            string texto = "";

            using (var comando = conexao.CreateCommand())
            {
                comando.CommandText = @"
                SELECT * FROM Perguntas
                WHERE dificuldade = @dif
                ORDER BY RANDOM()
                LIMIT 1;";

                comando.Parameters.Add(new SqliteParameter("@dif", dificuldade));

                using (IDataReader leitor = comando.ExecuteReader())
                {
                    if (leitor.Read())
                    {
                        idPergunta = int.Parse(leitor["id"].ToString());
                        texto = leitor["texto"].ToString();
                    }
                }
            }

            textoPergunta.text = texto;

            List<(string img, bool correta)> alternativas = new List<(string, bool)>();

            using (var comando = conexao.CreateCommand())
            {
                comando.CommandText = @"
                SELECT * FROM Alternativas
                WHERE pergunta_id = @id
                ORDER BY RANDOM();";

                comando.Parameters.Add(new SqliteParameter("@id", idPergunta));

                using (IDataReader leitor = comando.ExecuteReader())
                {
                    while (leitor.Read())
                    {
                        string img = leitor["imagem_path"].ToString();
                        bool correta = int.Parse(leitor["correta"].ToString()) == 1;

                        alternativas.Add((img, correta));
                    }
                }
            }

            int quantidade = Mathf.Min(botoes.Length, alternativas.Count);

            for (int i = 0; i < quantidade; i++)
            {
                var alt = alternativas[i];

                //Debug.Log("Tentando carregar: " + alt.img);

                Sprite sprite = Resources.Load<Sprite>(alt.img);

                //if (sprite == null)
                //{
                //    Debug.LogError("NÃO carregou: " + alt.img);
                //}
                //else
                //{
                //    Debug.Log("Carregou: " + alt.img);
                //}

                botoes[i].image.sprite = sprite;

                bool resposta = alt.correta;

                botoes[i].onClick.RemoveAllListeners();
                botoes[i].onClick.AddListener(() => VerificarResposta(resposta));
            }
        }
    }

    void VerificarResposta(bool correta)
    {
        if (correta)
        {
            Debug.Log("ACERTOU!");
        }
        else
        {
            Debug.Log("ERROU!");
        }

        CarregarPergunta("facil");
    }
}