using UnityEngine;

public class OrbitCamera : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float sensitivity = 5f;
    
    // Aumentei os limites para caber o Becker gigante
    [SerializeField] private float minDistance = 1f;
    [SerializeField] private float maxDistance = 500f; 
    private float distance;

    private float rotX = 0f;
    private float rotY = 0f;
    private bool active = true; // Começa ativo para facilitar

    void Start()
    {
        if (target != null)
        {
            // 1. Calcula a distância atual que você deixou na cena
            distance = Vector3.Distance(transform.position, target.position);
            
            // 2. Ajusta os ângulos iniciais para a câmera não dar um "pulo"
            Vector3 angles = transform.eulerAngles;
            rotX = angles.y;
            rotY = angles.x;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C)) active = !active;

        if (active && target != null)
        {
            if (Input.GetMouseButton(0))
            {
                rotX += Input.GetAxis("Mouse X") * sensitivity;
                rotY -= Input.GetAxis("Mouse Y") * sensitivity;
                rotY = Mathf.Clamp(rotY, -80f, 80f);
            }

            distance -= Input.mouseScrollDelta.y * 0.5f; // Zoom mais rápido
            distance = Mathf.Clamp(distance, minDistance, maxDistance);

            Quaternion rotation = Quaternion.Euler(rotY, rotX, 0);
            transform.rotation = rotation;
            transform.position = target.position - (rotation * Vector3.forward * distance);
        }
    }
}