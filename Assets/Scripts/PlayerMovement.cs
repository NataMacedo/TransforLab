using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float velocidade;
    public AudioSource passosAudioSource;
    public AudioClip somPassos;
    public float speed = 5.0f; // Ajuste conforme necessário
    public float rotationSpeed = 720.0f; // Velocidade de rotação
    public float rayDistance = 0.5f; // Distância do raycast para detecção de obstáculos
    private Animator animator;
    public WireConnectionHandler wireConnectionHandler; // Referência ao objeto WireConnectionHandler

    private bool isMovingToTarget = false;
    private Vector3 targetPosition;
    private string walkAnimationTrigger = "Walk";
    private float fixedY; // Coordenada Y fixa do avatar
    private Vector3 movement;

    void Start()
    {
        animator = GetComponent<Animator>();
        fixedY = transform.position.y; // Armazena a coordenada Y inicial
    }

    void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // Definir o movimento com base no input do jogador
        Vector3 inputMovement = new Vector3(moveHorizontal, 0.0f, moveVertical).normalized;

        // Parar o movimento se não houver input
        if (inputMovement == Vector3.zero)
        {
            movement = Vector3.zero;
            animator.SetFloat("Speed", 0f);
            passosAudioSource.Stop(); // Parar o áudio de passos
            return;
        }

        // Verificar se há um obstáculo em frente usando raycast
        if (!IsObstacleInFront(inputMovement))
        {
            movement = inputMovement * speed * Time.deltaTime;

            // Mover o personagem usando Transform
            Vector3 newPosition = transform.position + movement;
            newPosition.y = fixedY; // Manter a posição Y fixa
            transform.position = newPosition;

            // Rotacionar o personagem na direção do movimento
            Quaternion toRotation = Quaternion.LookRotation(inputMovement);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);

            animator.SetFloat("Speed", 1f); // Ativar a animação de andar

            // Reproduzir som de passos em loop se a velocidade for maior que zero
            if (velocidade > 0f && !passosAudioSource.isPlaying)
            {
                passosAudioSource.clip = somPassos;
                passosAudioSource.loop = true;
                passosAudioSource.Play();
            }
        }
        else
        {
            animator.SetFloat("Speed", 0f); // Parar a animação de andar se houver obstáculo
            passosAudioSource.Stop(); // Parar o áudio de passos se houver obstáculo
        }

        // Verificar clique do mouse para animação de interação
        if (Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("Interact");
        }

      

        // Mover o personagem para a posição alvo se necessário
        if (isMovingToTarget)
        {
            MoveToTarget();
        }
    }

    void LateUpdate()
    {
        // Garantir que a rotação nos eixos X e Z permaneça 0
        Vector3 rotation = transform.rotation.eulerAngles;
        rotation.x = 0;
        rotation.z = 0;
        transform.rotation = Quaternion.Euler(rotation);

        // Garantir que a posição Y permaneça fixa
        Vector3 position = transform.position;
        position.y = fixedY;
        transform.position = position;
    }

    public void MoveToPosition(Vector3 position, string walkTrigger)
    {
        targetPosition = position;
        walkAnimationTrigger = walkTrigger;
        isMovingToTarget = true;
        animator.SetTrigger(walkAnimationTrigger);
    }

    private void MoveToTarget()
    {
        Vector3 targetPositionAdjusted = new Vector3(targetPosition.x, fixedY, targetPosition.z);
        Vector3 direction = (targetPositionAdjusted - transform.position).normalized;

        if (!IsObstacleInFront(direction))
        {
            Vector3 movement = direction * speed * Time.deltaTime;
            Vector3 newPosition = transform.position + movement;
            newPosition.y = fixedY; // Manter a posição Y fixa
            transform.position = newPosition;

            // Rotacionar o personagem na direção do movimento
            if (direction != Vector3.zero)
            {
                Quaternion toRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
            }

            if (Vector3.Distance(transform.position, targetPositionAdjusted) < 0.1f)
            {
                isMovingToTarget = false;
                animator.ResetTrigger(walkAnimationTrigger);
            }
        }
    }

    private bool IsObstacleInFront(Vector3 direction)
    {
        RaycastHit hit;
        Vector3 rayOrigin = transform.position + Vector3.up * 0.5f; // Ajustar a origem do raycast para estar na altura do personagem
        if (Physics.Raycast(rayOrigin, direction, out hit, rayDistance))
        {
            return hit.collider != null;
        }
        return false;
    }
}
