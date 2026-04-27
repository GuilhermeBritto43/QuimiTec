using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Voltar : MonoBehaviour
{

    public void OnClickVoltarLogin()
    {
        SceneManager.LoadScene("TelaLogin");
    }

    public void OnClickVoltarTelaAluno()
    {
        SceneManager.LoadScene("TelaJogarEstudar");
    }

    public void OnClickVoltarTelaProfessor()
    {
        SceneManager.LoadScene("TelaProfessor");
    }
}