using UnityEngine;
using TMPro;

public class Gerenciador : MonoBehaviour
{
    public Item3DViewer item3DViewer;
    public GameObject popupProveta;
    public GameObject popupTuboEnsaio;
    public GameObject popupTuboVidro;
    public GameObject popupAlmofariz;
    public GameObject popupFunilAnalitico;
    public GameObject popupBureta;
    public GameObject popupTelaAmianto;
    public GameObject popupCondensador;
    public GameObject popupBalaoFundo;
    public GameObject popupBecker;
    public GameObject popupPipeta;
    public GameObject popupErlenmeyer;
    public GameObject popupSuporteUniversal;
    public GameObject popupVidroRelogio;
    public GameObject popupFunilBuchner;
    public GameObject popupDessecador;
    public GameObject popupBançaAnal;
    public GameObject popupBaseDeFerro;
    public GameObject popupMontagemAque;
    public GameObject popupBicoDebusen;
    private string itemName;
    public void OnClickInfo()
    {
        itemName = item3DViewer.itemName;
        Debug.Log(itemName);
        if (itemName == "proveta_correta")
            popupProveta.SetActive(true);
        else if(itemName == "tubo de ensaio")
            popupTuboEnsaio.SetActive(true);
        else if(itemName == "Tubo de vidro fino e longo com marcacoes de volume")
            popupTuboVidro.SetActive(true);
        else if (itemName == "Almofariz com Pistilo")
            popupAlmofariz.SetActive(true);
        else if (itemName == "BalãoDeFundoRedondo")
            popupBalaoFundo.SetActive(true);
        else if (itemName == "baseUniversal")
            popupBaseDeFerro.SetActive(true);
        else if (itemName == "becker1")
            popupBecker.SetActive(true);
        else if (itemName == "BicoDebusen")
            popupBicoDebusen.SetActive(true);
        else if (itemName == "Bureta")
            popupBureta.SetActive(true);
        else if (itemName == "Condensador")
            popupCondensador.SetActive(true);
        else if (itemName == "erlenmeyer")
            popupErlenmeyer.SetActive(true);
        else if (itemName == "Funil AnaliticoComum")
            popupFunilAnalitico.SetActive(true);
        else if (itemName == "pipeta")
            popupPipeta.SetActive(true);
        else if (itemName == "Tela de Amianto_Ceramica")
            popupTelaAmianto.SetActive(true);
        else if (itemName == "vidroDeRelogio")
            popupVidroRelogio.SetActive(true);
        else if (itemName == "Funil de Buchner e kitasato")
            popupFunilBuchner.SetActive(true);
        else if (itemName == "dessecador")
            popupDessecador.SetActive(true);
        else if (itemName == "Montagem de aquecimento pernas de ferro segurando a tela")
            popupMontagemAque.SetActive(true);
        else if (itemName == "balancaanalitica")
            popupBançaAnal.SetActive(true);
        else if (itemName == "Base de ferro pesada com uma haste metálica vertical")
            popupBaseDeFerro.SetActive(true);
    }
    
    public void Fechar()
        {
        popupProveta.SetActive(false);
        popupTuboEnsaio.SetActive(false);
        popupTuboVidro.SetActive(false);
        popupAlmofariz.SetActive(false);
        popupFunilAnalitico.SetActive(false);
        popupBureta.SetActive(false);
        popupTelaAmianto.SetActive(false);
        popupCondensador.SetActive(false);
        popupBalaoFundo.SetActive(false);
        popupBecker.SetActive(false);
        popupPipeta.SetActive(false);
        popupErlenmeyer.SetActive(false);
        popupSuporteUniversal.SetActive(false);
        popupVidroRelogio.SetActive(false);
        popupFunilBuchner.SetActive(false);
        popupDessecador.SetActive(false);
        popupBançaAnal.SetActive(false);
        popupBaseDeFerro.SetActive(false);
        popupMontagemAque.SetActive(false);
        popupBicoDebusen.SetActive(false);
    }
}