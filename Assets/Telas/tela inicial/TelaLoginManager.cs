using UnityEngine;
using UnityEngine.SceneManagement;
public class TelaLoginManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Entrar(){
        SceneManager.LoadScene("telaJogarEstudar");
    }
}
