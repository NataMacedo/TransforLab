using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // O transform do personagem (Engenheiro)
    public Vector3 offset; // Offset da c�mera em rela��o ao personagem
    public float smoothSpeed = 0.125f; // Velocidade de suaviza��o da posi��o
    public float rotationSpeed = 5.0f; // Velocidade de rota��o da c�mera

    private Vector3 currentVelocity;
    private float mouseX;
    private float mouseY;

    void LateUpdate()
    {
        // Atualizar a posi��o da c�mera
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref currentVelocity, smoothSpeed);
        transform.position = smoothedPosition;

        // Rotacionar a c�mera com base na movimenta��o do mouse
        mouseX += Input.GetAxis("Mouse X") * rotationSpeed;
        mouseY -= Input.GetAxis("Mouse Y") * rotationSpeed;

        // Limitar a rota��o vertical para evitar invers�o
        mouseY = Mathf.Clamp(mouseY, -35, 60);

        // Aplicar a rota��o com base no movimento do mouse
        Quaternion rotation = Quaternion.Euler(mouseY, mouseX, 0);
        transform.position = target.position + rotation * offset;
        transform.LookAt(target.position);
    }
}
