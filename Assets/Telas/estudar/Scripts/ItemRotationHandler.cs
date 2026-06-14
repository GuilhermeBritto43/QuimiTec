using UnityEngine;
using UnityEngine.EventSystems;

public class ItemRotationHandler : MonoBehaviour, IDragHandler
{
    [Header("Referências")]
    [SerializeField] private TelaEstudarController telaEstudarController;
    [SerializeField] private float velocidadeRotacao = 0.4f;

    public void OnDrag(PointerEventData eventData)
    {
        if (telaEstudarController == null) return;

        // Pega o fbx atual que nasceu no ponto de spawn
        GameObject objeto3D = telaEstudarController.GetObjetoAtual();

        if (objeto3D != null)
        {
            // Converte o movimento do mouse/dedo em eixos de rotação
            float rotX = eventData.delta.x * velocidadeRotacao;
            float rotY = eventData.delta.y * velocidadeRotacao;

            // Aplica as rotações de forma natural
            objeto3D.transform.Rotate(Vector3.up, -rotX, Space.World);
            objeto3D.transform.Rotate(Vector3.right, rotY, Space.World);
        }
    }
}