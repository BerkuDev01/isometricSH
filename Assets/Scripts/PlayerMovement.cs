using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public CharacterController characterController;
    public float speed = 5f;
    public float gravity = -9.81f; // por si quieres usar gravedad realista

    [Header("Camera Reference")]
    [SerializeField] private Camera mainCamera;

    private Vector3 velocity; // para manejar la gravedad
    private float groundCheckDistance = 0.2f;

    void Update()
    {
        MoveRelativeToCamera();
        RotateTowardsMouse();
    }

    private void MoveRelativeToCamera()
    {
        // Ejes de entrada
        float x = Input.GetAxisRaw("Horizontal"); // Raw = sin suavizado (evita "flotado")
        float z = Input.GetAxisRaw("Vertical");

        // Si no hay input, no aplicar movimiento
        if (x == 0 && z == 0)
        {
            // Mantener en el suelo sin deslizar
            velocity.y += gravity * Time.deltaTime;
            characterController.Move(velocity * Time.deltaTime);
            return;
        }

        // Dirección de movimiento basada en la cámara
        Vector3 camForward = mainCamera.transform.forward;
        Vector3 camRight = mainCamera.transform.right;

        camForward.y = 0f;
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();

        // Calcular dirección final
        Vector3 move = (camForward * z + camRight * x).normalized;

        // Aplicar movimiento
        characterController.Move(move * speed * Time.deltaTime);

        // Aplicar gravedad si usas CharacterController
        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }

    private void RotateTowardsMouse()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 lookPoint = hit.point;
            lookPoint.y = transform.position.y;

            Quaternion targetRotation = Quaternion.LookRotation(lookPoint - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }
    }
}







