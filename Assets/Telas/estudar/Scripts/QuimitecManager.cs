using UnityEngine;
using UnityEngine.SceneManagement;

public class QuimitecManager : MonoBehaviour
{
    public static QuimitecManager Instance;
    public QuimicaItemSO ItemSelecionado { get; set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void IrParaInspecao(QuimicaItemSO item)
    {
        ItemSelecionado = item;
        SceneManager.LoadScene("estudar");
    }

    public void VoltarParaSelecao()
    {
        SceneManager.LoadScene("telaSelecao");
    }
}