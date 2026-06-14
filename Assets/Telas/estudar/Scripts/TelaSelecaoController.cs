using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class TelaSelecaoController : MonoBehaviour
{
    [Header("Banco de Dados")]
    [SerializeField] private List<QuimicaItemSO> todosOsItens;

    [Header("Componentes da UI")]
    [SerializeField] private Image espaçoItemCarrossel;
    [SerializeField] private Button btnSetaEsquerda;
    [SerializeField] private Button btnSetaDireita;
    [SerializeField] private Button btnConfirmarSelecao;

    private List<QuimicaItemSO> itensFiltrados = new List<QuimicaItemSO>();
    private int indexAtual = 0;

    void Start()
    {
        btnSetaEsquerda.onClick.AddListener(ItemAnterior);
        btnSetaDireita.onClick.AddListener(ProximoItem);
        btnConfirmarSelecao.onClick.AddListener(AbrirInspecao);

        FiltrarPorSecao(0);
    }

    public void FiltrarPorSecao(int secaoIndex)
    {
        QuimicaItemSO.TipoSecao secaoAlvo = (QuimicaItemSO.TipoSecao)secaoIndex;
        itensFiltrados = todosOsItens.FindAll(item => item.secao == secaoAlvo);
        indexAtual = 0;
        AtualizarCarrossel();
    }

    private void AtualizarCarrossel()
    {
        if (itensFiltrados.Count == 0) return;
        espaçoItemCarrossel.sprite = itensFiltrados[indexAtual].renderUI;
    }

    private void ProximoItem()
    {
        if (itensFiltrados.Count == 0) return;
        indexAtual = (indexAtual + 1) % itensFiltrados.Count;
        AtualizarCarrossel();
    }

    private void ItemAnterior()
    {
        if (itensFiltrados.Count == 0) return;
        indexAtual--;
        if (indexAtual < 0) indexAtual = itensFiltrados.Count - 1;
        AtualizarCarrossel();
    }

    private void AbrirInspecao()
    {
        if (itensFiltrados.Count > 0)
        {
            QuimitecManager.Instance.IrParaInspecao(itensFiltrados[indexAtual]);
        }
    }
}