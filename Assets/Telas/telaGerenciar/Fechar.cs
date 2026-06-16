using UnityEngine;

public class Fechar : MonoBehaviour
{
    public GameObject panelListar;

    public void FecharListar()
    {
        panelListar.SetActive(false);
    }

}