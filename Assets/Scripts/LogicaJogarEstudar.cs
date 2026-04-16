using UnityEngine;
using UnityEngine.SceneManagement;
public class TelaJogarManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Estudar(){
        SceneManager.LoadScene("estudar");
    }
    public void Jogar(){
        SceneManager.LoadScene("Jogo");
    }
}
