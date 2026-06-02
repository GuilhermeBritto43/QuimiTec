using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Mono.Data.Sqlite;
using System.Data;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;

public class QuizManager : MonoBehaviour
{
    [Header("Fim de jogo")]
    public GameObject painelFimJogo;

    public TMP_Text textoNomeFinal;
    public TMP_Text textoPontosFinal;
    public TMP_Text textoRecordeFinal;

    [Header("Dica")]
    public GameObject painelDica;
    public TMP_Text textoDica;

    private string dicaAtual;

    [Header("UI")]
    public TMP_Text textoPergunta;
    public Button[] botoes;

    [Header("Pontuação")]
    public TMP_Text textoJogador;
    public TMP_Text textoPontuacao;

    [Header("Dificuldade")]
    public TMP_Text textoDificuldade;
    private int acertosNivel = 0;
    private string caminhoDB;

    private int pontuacao = 0;

    private string dificuldadeAtual = "facil";

    private string nomeJogador;

    private List<int> perguntasFaceis =
        new List<int>();

    private List<int> perguntasMedias =
        new List<int>();

    private List<int> perguntasDificeis =
        new List<int>();

    void Start()
    {
        caminhoDB = DatabaseManager.CaminhoDB;

        nomeJogador = DadosJogador.nome;

        AtualizarUI();

        CarregarPergunta();
    }

    void CarregarPergunta()
    {
        List<int> perguntasUsadas;

        if (dificuldadeAtual == "facil")
        {
            perguntasUsadas = perguntasFaceis;
        }
        else if (dificuldadeAtual == "medio")
        {
            perguntasUsadas = perguntasMedias;
        }
        else
        {
            perguntasUsadas = perguntasDificeis;
        }

        using (var conexao = new SqliteConnection(caminhoDB))
        {
            conexao.Open();

            int idPergunta = -1;

            string perguntaTexto = "";

            string dica = "";

            using (var comando = conexao.CreateCommand())
            {
                string idsUsados =
                    perguntasUsadas.Count > 0
                    ? string.Join(",", perguntasUsadas)
                    : "0";

                comando.CommandText = $@"
                SELECT *
                FROM Perguntas
                WHERE dificuldade = @dif
                AND id NOT IN ({idsUsados})
                ORDER BY RANDOM()
                LIMIT 1;";

                comando.Parameters.Add(
                    new SqliteParameter(
                        "@dif",
                        dificuldadeAtual));

                using (IDataReader leitor =
                    comando.ExecuteReader())
                {
                    if (leitor.Read())
                    {
                        idPergunta =
                            int.Parse(
                                leitor["id"].ToString());

                        perguntaTexto =
                            leitor["texto"].ToString();

                        dica =
                            leitor["dicas"].ToString();
                    }

                    dicaAtual = dica;
                }
            }

            if (idPergunta == -1)
            {
                Debug.Log(
                    "Não existem mais perguntas de " +
                    dificuldadeAtual);

                FinalizarJogo();

                return;
            }

            perguntasUsadas.Add(idPergunta);

            textoPergunta.text = perguntaTexto;

            List<(string img, bool correta)> alternativas =
                new List<(string, bool)>();

            using (var comando = conexao.CreateCommand())
            {
                comando.CommandText = @"
                SELECT *
                FROM Alternativas
                WHERE pergunta_id = @id
                AND correta = 1
                LIMIT 1;";

                comando.Parameters.Add(
                    new SqliteParameter(
                        "@id",
                        idPergunta));

                using (IDataReader leitor =
                    comando.ExecuteReader())
                {
                    if (leitor.Read())
                    {
                        string img =
                            leitor["imagem_path"].ToString();

                        alternativas.Add((img, true));
                    }
                }
            }

            using (var comando = conexao.CreateCommand())
            {
                comando.CommandText = @"
                SELECT *
                FROM Alternativas
                WHERE pergunta_id = @id
                AND correta = 0
                ORDER BY RANDOM()
                LIMIT 3;";

                comando.Parameters.Add(
                    new SqliteParameter(
                        "@id",
                        idPergunta));

                using (IDataReader leitor =
                    comando.ExecuteReader())
                {
                    while (leitor.Read())
                    {
                        string img =
                            leitor["imagem_path"].ToString();

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

            int quantidade =
                Mathf.Min(
                    botoes.Length,
                    alternativas.Count);

            for (int i = 0; i < quantidade; i++)
            {
                var alt = alternativas[i];

                Debug.Log(
                    "Tentando carregar: " +
                    alt.img);

                Sprite sprite =
                    Resources.Load<Sprite>(
                        alt.img);

                if (sprite == null)
                {
                    Debug.LogError(
                        "NÃO carregou: " +
                        alt.img);

                    continue;
                }

                Debug.Log(
                    "Carregou: " +
                    alt.img);

                botoes[i].image.sprite = sprite;

                bool resposta = alt.correta;

                Button botaoAtual = botoes[i];

                botoes[i]
                    .onClick
                    .RemoveAllListeners();

                botoes[i]
                    .onClick
                    .AddListener(() =>
                        VerificarResposta(
                            botaoAtual,
                            resposta));
            }
        }
    }

    void VerificarResposta(
        Button botaoClicado,
        bool correta)
    {
        if (correta)
        {
            Debug.Log("ACERTOU!");

            botaoClicado.image.color = Color.green;

            pontuacao += 10;

            acertosNivel++;

            AtualizarUI();

            VerificarMudancaDeNivel();
        }
        else
        {
            Debug.Log("ERROU!");

            botaoClicado.image.color =
                Color.red;
        }

        foreach (Button b in botoes)
        {
            b.interactable = false;
        }

        Invoke(
            "ProximaPergunta",
            1.5f);
    }

    void ProximaPergunta()
    {
        CarregarPergunta();
    }

    void AtualizarUI()
    {
        textoJogador.text =
            "Jogador: " + nomeJogador;

        textoPontuacao.text =
            "Pontos: " + pontuacao;

        textoDificuldade.text =
            "Dificuldade: " +
            dificuldadeAtual.ToUpper();
    }

    public void MostrarDica()
    {
        painelDica.SetActive(true);

        textoDica.text = dicaAtual;
    }

    public void FecharDica()
    {
        painelDica.SetActive(false);
    }

    void FinalizarJogo()
    {
        if (pontuacao > DadosJogador.recorde)
        {
            DadosJogador.recorde = pontuacao;

            AtualizarRecordeNoBanco();
        }

        painelFimJogo.SetActive(true);

        textoNomeFinal.text =
            "Jogador: " +
            DadosJogador.nome;

        textoPontosFinal.text =
            "Pontuação: " +
            pontuacao;

        textoRecordeFinal.text =
            "Recorde: " +
            DadosJogador.recorde;
    }

    void AtualizarRecordeNoBanco()
    {
        using (var conexao =
            new SqliteConnection(caminhoDB))
        {
            conexao.Open();

            using (var comando =
                conexao.CreateCommand())
            {
                comando.CommandText = @"
                UPDATE Usuarios
                SET recorde = @recorde
                WHERE email = @email;";

                comando.Parameters.Add(
                    new SqliteParameter(
                        "@recorde",
                        DadosJogador.recorde));

                comando.Parameters.Add(
                    new SqliteParameter(
                        "@email",
                        DadosJogador.email));

                comando.ExecuteNonQuery();
            }
        }
    }

    public void ReiniciarJogo()
    {
        SceneManager.LoadScene(
            SceneManager.GetActiveScene().name);
    }

    public void VoltarTelaInicial()
    {
        Debug.Log("434343434343434343434343" + DadosJogador.tipoUsuario);
        if (DadosJogador.tipoUsuario == "Professor")
        {
            SceneManager.LoadScene("telaProfessor");
        }
        else
        {
            SceneManager.LoadScene("TelaJogarEstudar");
        }
    }

    public void FecharJogo()
    {
        Debug.Log("FECHANDO JOGO");

        Application.Quit();
    }

    void VerificarMudancaDeNivel()
    {
        if (acertosNivel < 5)
            return;

        acertosNivel = 0;

        if (dificuldadeAtual == "facil")
        {
            dificuldadeAtual = "medio";

            Debug.Log("SUBIU PARA MÉDIO");

            AtualizarUI();
        }
        else if (dificuldadeAtual == "medio")
        {
            dificuldadeAtual = "dificil";

            Debug.Log("SUBIU PARA DIFÍCIL");

            AtualizarUI();
        }
        else
        {
            Debug.Log("JOGO CONCLUÍDO");

            FinalizarJogo();
        }
    }
}