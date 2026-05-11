using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Mono.Data.Sqlite;
using System.Data;
using System.IO;
using System.Collections.Generic;
using System.Linq;

public class QuizManager : MonoBehaviour
{
    [Header("UI")]
    public TMP_Text textoPergunta;
    public Button[] botoes;

    private string caminhoDB;

    private List<int> perguntasUsadas = new List<int>();

    [Header("Pontuação")]
    public TMP_Text textoJogador;
    public TMP_Text textoPontuacao;

    private int pontuacao = 0;

    private string nomeJogador;

    void Start()
    {
        nomeJogador = DadosJogador.nome;

        string nomeBanco = "QuimiTec.db";

        string caminhoOrigem = Path.Combine(Application.streamingAssetsPath, nomeBanco);
        string caminhoDestino = Path.Combine(Application.persistentDataPath, nomeBanco);

        File.Copy(caminhoOrigem, caminhoDestino, true);

        caminhoDB = "URI=file:" + caminhoDestino;

        AtualizarUI();

        CarregarPergunta("facil");
    }

    void CarregarPergunta(string dificuldade)
    {
        using (var conexao = new SqliteConnection(caminhoDB))
        {
            conexao.Open();

            int idPergunta = -1;
            string perguntaTexto = "";

            using (var comando = conexao.CreateCommand())
            {
                string idsUsados = perguntasUsadas.Count > 0
                    ? string.Join(",", perguntasUsadas)
                    : "0";

                comando.CommandText = $@"
                SELECT * FROM Perguntas
                WHERE dificuldade = @dif
                AND id NOT IN ({idsUsados})
                ORDER BY RANDOM()
                LIMIT 1;";

                comando.Parameters.Add(new SqliteParameter("@dif", dificuldade));

                using (IDataReader leitor = comando.ExecuteReader())
                {
                    if (leitor.Read())
                    {
                        idPergunta = int.Parse(leitor["id"].ToString());
                        perguntaTexto = leitor["texto"].ToString();
                    }
                }
            }

            if (idPergunta == -1)
            {
                Debug.Log("Todas as perguntas já foram usadas!");
                return;
            }

            perguntasUsadas.Add(idPergunta);

            textoPergunta.text = perguntaTexto;

            List<(string img, bool correta)> alternativas =
                new List<(string, bool)>();

            using (var comando = conexao.CreateCommand())
            {
                comando.CommandText = @"
                SELECT * FROM Alternativas
                WHERE pergunta_id = @id
                AND correta = 1
                LIMIT 1;";

                comando.Parameters.Add(new SqliteParameter("@id", idPergunta));

                using (IDataReader leitor = comando.ExecuteReader())
                {
                    if (leitor.Read())
                    {
                        string img = leitor["imagem_path"].ToString();

                        alternativas.Add((img, true));
                    }
                }
            }

            using (var comando = conexao.CreateCommand())
            {
                comando.CommandText = @"
                SELECT * FROM Alternativas
                WHERE pergunta_id = @id
                AND correta = 0
                ORDER BY RANDOM()
                LIMIT 3;";

                comando.Parameters.Add(new SqliteParameter("@id", idPergunta));

                using (IDataReader leitor = comando.ExecuteReader())
                {
                    while (leitor.Read())
                    {
                        string img = leitor["imagem_path"].ToString();

                        alternativas.Add((img, false));
                    }
                }
            }

            alternativas = alternativas
                .OrderBy(x => Random.value)
                .ToList();

            foreach (Button b in botoes)
            {
                b.image.color = Color.white;
                b.interactable = true;
            }

            int quantidade = Mathf.Min(botoes.Length, alternativas.Count);

            for (int i = 0; i < quantidade; i++)
            {
                var alt = alternativas[i];

                Debug.Log("Tentando carregar: " + alt.img);

                Sprite sprite = Resources.Load<Sprite>(alt.img);

                if (sprite == null)
                {
                    Debug.LogError("NÃO carregou: " + alt.img);
                    continue;
                }

                Debug.Log("Carregou: " + alt.img);

                botoes[i].image.sprite = sprite;

                bool resposta = alt.correta;

                Button botaoAtual = botoes[i];

                botoes[i].onClick.RemoveAllListeners();

                botoes[i].onClick.AddListener(() =>
                    VerificarResposta(botaoAtual, resposta));
            }
        }
    }

    void VerificarResposta(Button botaoClicado, bool correta)
    {
        if (correta)
        {
            Debug.Log("ACERTOU!");

            botaoClicado.image.color = Color.green;

            pontuacao += 10;

            AtualizarUI();
        }
        else
        {
            Debug.Log("ERROU!");
            botaoClicado.image.color = Color.red;
        }

        foreach (Button b in botoes)
        {
            b.interactable = false;
        }

        Invoke("ProximaPergunta", 1.5f);
    }

    void ProximaPergunta()
    {
        CarregarPergunta("facil");
    }

    void AtualizarUI()
    {
        textoJogador.text = "Jogador: " + nomeJogador;
        textoPontuacao.text = "Pontos: " + pontuacao;
    }
}