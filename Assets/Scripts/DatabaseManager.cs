using UnityEngine;
using System.IO;

public static class DatabaseManager
{
    public static string CaminhoDB
    {
        get
        {
            string nomeBanco = "QuimiTec.db";

            string caminhoOrigem =
                Path.Combine(Application.streamingAssetsPath, nomeBanco);

            string caminhoDestino =
                Path.Combine(Application.persistentDataPath, nomeBanco);

            if (!File.Exists(caminhoDestino))
            {
                File.Copy(caminhoOrigem, caminhoDestino);
            }

            return "URI=file:" + caminhoDestino;
        }
    }
}