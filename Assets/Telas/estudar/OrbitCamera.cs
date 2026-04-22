using UnityEngine;

public class OrbitCamera : MonoBehaviour
{
    [SerializeField] private Transform lookAtTransform;
    [SerializeField] private float sensitivity = 5f;
    [SerializeField] private float maximumOrbitDistance = 50f;
    [SerializeField] private float minimumOrbitDistance = 10f;
    [SerializeField] private float defaultOrbitDistance = 25f; // Nova variável para distância inicial

    private float orbitRadius = 25f;

    private bool isOrbitCameraActive = false;
    private float mouseX = 0f;
    private float mouseY = 0f;

    private void Update()
    {
        // toggle orbit/cockpit camera
        if (Input.GetKeyUp(KeyCode.C))
        {
            isOrbitCameraActive = !isOrbitCameraActive;

            // reset everything if deactivated
            if (!isOrbitCameraActive)
            {
                transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
                transform.rotation = Quaternion.identity;
            }
            else
            {
                // Agora usa o valor que você definir no Inspector
                orbitRadius = defaultOrbitDistance;
            }
        }

        if (isOrbitCameraActive)
        {
            // only orbit when left mousebutton is down
            if (Input.GetMouseButton(0))
            {
                // lookat target
                transform.LookAt(lookAtTransform);

                // convert mouse axis to rotation
                mouseX = Input.GetAxis("Mouse X");
                mouseY = Input.GetAxis("Mouse Y");
                transform.eulerAngles += new Vector3(-mouseY * sensitivity, mouseX * sensitivity, 0);
            }

            // calc and clamp orbit radius and apply it to camera
            orbitRadius -= Input.mouseScrollDelta.y / sensitivity;
            orbitRadius = Mathf.Clamp(orbitRadius, minimumOrbitDistance, maximumOrbitDistance);

            transform.position = lookAtTransform.position - transform.forward * orbitRadius;
        }
    }
}