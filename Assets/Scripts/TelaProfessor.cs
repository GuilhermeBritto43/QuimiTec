using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TelaProfessor : MonoBehaviour
{ 

    public void OnClickGerenciar()
    {
        SceneManager.LoadScene("telaGerenciar");
    }

    public void OnClickEditar()
    {
        SceneManager.LoadScene("telaEditarPerguntas");
    }

    public void OnClickJogar()
    {
        SceneManager.LoadScene("Jogo");
    }
}