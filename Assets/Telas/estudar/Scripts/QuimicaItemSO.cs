using UnityEngine;

[CreateAssetMenu(fileName = "NovoObjetoQuimica", menuName = "Quimitec/Objeto")]
public class QuimicaItemSO : ScriptableObject
{
    public string nomeObjeto;
    [TextArea(3, 8)] public string infoObjeto;
    public GameObject prefab3D; // .fbx da pasta estudar/Objetos 3d
    public Sprite renderUI;     // .png da pasta telaSelecao/renders

    public enum TipoSecao { Secao1, Secao2, Secao3 }
    public TipoSecao secao;
}