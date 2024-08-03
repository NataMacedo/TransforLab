using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // O transform do personagem (Engenheiro)
    public Vector3 offset; // Offset da câmera em relação ao personagem
    public float smoothSpeed = 0.125f; // Velocidade de suavização da posição
    public float rotationSpeed = 5.0f; // Velocidade de rotação da câmera

    private Vector3 currentVelocity;
    private float mouseX;
    private float mouseY;

    void LateUpdate()
    {
        // Atualizar a posição da câmera
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref currentVelocity, smoothSpeed);
        transform.position = smoothedPosition;

        // Rotacionar a câmera com base na movimentação do mouse
        mouseX += Input.GetAxis("Mouse X") * rotationSpeed;
        mouseY -= Input.GetAxis("Mouse Y") * rotationSpeed;

        // Limitar a rotação vertical para evitar inversão
        mouseY = Mathf.Clamp(mouseY, -35, 60);

        // Aplicar a rotação com base no movimento do mouse
        Quaternion rotation = Quaternion.Euler(mouseY, mouseX, 0);
        transform.position = target.position + rotation * offset;
        transform.LookAt(target.position);
    }
}
