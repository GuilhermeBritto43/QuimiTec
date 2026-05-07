using UnityEngine;
using UnityEngine.InputSystem; // Adicione isso aqui!

public class OrbitCamera : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float sensitivity = 0.2f; // Sensibilidade costuma ser menor no novo sistema
    [SerializeField] private float minDistance = 1f;
    [SerializeField] private float maxDistance = 500f;
    private float distance;

    private float rotX = 0f;
    private float rotY = 0f;
    private bool active = true;

    void Start()
    {
        if (target != null)
        {
            distance = Vector3.Distance(transform.position, target.position);
            Vector3 angles = transform.eulerAngles;
            rotX = angles.y;
            rotY = angles.x;
        }
    }

    void Update()
    {
        // Tecla 'C' no sistema novo
        if (Keyboard.current.cKey.wasPressedThisFrame) active = !active;

        if (active && target != null)
        {
            // Botão esquerdo do mouse
            if (Mouse.current.leftButton.isPressed)
            {
                var mouseDelta = Mouse.current.delta.ReadValue();
                rotX += mouseDelta.x * sensitivity;
                rotY -= mouseDelta.y * sensitivity;
                rotY = Mathf.Clamp(rotY, -80f, 80f);
            }

            // Zoom com Scroll
            float scroll = Mouse.current.scroll.ReadValue().y;
            distance -= scroll * 0.01f; // Ajuste fino do zoom
            distance = Mathf.Clamp(distance, minDistance, maxDistance);

            Quaternion rotation = Quaternion.Euler(rotY, rotX, 0);
            transform.rotation = rotation;
            transform.position = target.position - (rotation * Vector3.forward * distance);
        }
    }
}