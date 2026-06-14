using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TelaEstudarController : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI txtNomeObjeto;
    [SerializeField] private TextMeshProUGUI txtInfoObjeto;
    [SerializeField] private Button btnVoltar;

    [Header("3D Spawn")]
    [SerializeField] private Transform spawnPosition;

    private GameObject objetoInstanciado;

    void Start()
    {
        btnVoltar.onClick.AddListener(Voltar);

        if (QuimitecManager.Instance != null && QuimitecManager.Instance.ItemSelecionado != null)
        {
            QuimicaItemSO item = QuimitecManager.Instance.ItemSelecionado;
            txtNomeObjeto.text = item.nomeObjeto;
            txtInfoObjeto.text = item.infoObjeto;

            if (item.prefab3D != null)
            {
                objetoInstanciado = Instantiate(item.prefab3D, spawnPosition.position, Quaternion.identity);
            }
        }
    }

    private void Voltar()
    {
        if (objetoInstanciado != null) Destroy(objetoInstanciado);
        QuimitecManager.Instance.VoltarParaSelecao();
    }

    public GameObject GetObjetoAtual() => objetoInstanciado;
}