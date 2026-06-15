using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public static class DatabaseManager
{
    private static string nomeBanco = "QuimiTec.db"; // <-- COLOQUE O NOME EXATO DO SEU ARQUIVO AQUI

    public static string CaminhoDB
    {
        get
        {
            string caminhoPasta = Application.persistentDataPath;
            string caminhoArquivo = Path.Combine(caminhoPasta, nomeBanco);

            // Se o arquivo não existir na pasta persistente (comum na primeira execução no mobile)
            if (!File.Exists(caminhoArquivo))
            {
                Debug.Log("Banco de dados não encontrado em persistentDataPath. Copiando...");

                // Caminho de origem na pasta StreamingAssets
                string caminhoOrigem = Path.Combine(Application.streamingAssetsPath, nomeBanco);

#if UNITY_EDITOR || UNITY_STANDALONE
                // No PC / Editor do Unity, podemos copiar diretamente
                if (File.Exists(caminhoOrigem))
                {
                    File.Copy(caminhoOrigem, caminhoArquivo);
                }
                else
                {
                    Debug.LogError($"O banco original não foi encontrado em: {caminhoOrigem}");
                }
#elif UNITY_ANDROID
                using (UnityWebRequest webRequest = UnityWebRequest.Get(caminhoOrigem))
                {
                    // Força a execução síncrona para garantir que o banco exista antes do Start do seu script
                    var operacao = webRequest.SendWebRequest();
                    while (!operacao.isDone) { } 

                    if (webRequest.result == UnityWebRequest.Result.Success)
                    {
                        // Grava os bytes extraídos do APK para a pasta segura do celular
                        File.WriteAllBytes(caminhoArquivo, webRequest.downloadHandler.data);
                        Debug.Log("Banco de dados copiado com sucesso para o Android!");
                    }
                    else
                    {
                        Debug.LogError("Erro ao extrair o banco de dados do APK: " + webRequest.error);
                    }
                }
#endif
            }

            // IMPORTANTE: O Android exige o prefixo "URI=file:" para abrir a conexão SQLite
            return "URI=file:" + caminhoArquivo;
        }
    }
}