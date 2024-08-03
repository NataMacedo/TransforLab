using UnityEngine;

public class MRTController : MonoBehaviour
{
    // Vari�veis para os terminais do MRT
    public GameObject terminalX1;
    public GameObject terminalX2;
    public GameObject terminalH1;
    public GameObject terminalH2;

    private void Start()
    {
        // Certifique-se de que todos os terminais foram atribu�dos
        if (terminalX1 == null || terminalX2 == null || terminalH1 == null || terminalH2 == null)
        {
            Debug.LogError("Um ou mais terminais do MRT n�o foram atribu�dos.");
            return;
        }
    }

    private void Update()
    {
        // Verifica se o bot�o esquerdo do mouse foi pressionado
        if (Input.GetMouseButtonDown(0))
        {
            // Lan�a um raio a partir da posi��o do mouse na dire��o do plano XY
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                // Verifica se o objeto atingido � um dos terminais
                if (hit.collider.gameObject == terminalX1)
                {
                    Debug.Log("Fio conectado ao terminal X1MRT.");
                }
                else if (hit.collider.gameObject == terminalX2)
                {
                    Debug.Log("Fio conectado ao terminal X2MRT.");
                }
                else if (hit.collider.gameObject == terminalH1)
                {
                    Debug.Log("Fio conectado ao terminal H1MRT.");
                }
                else if (hit.collider.gameObject == terminalH2)
                {
                    Debug.Log("Fio conectado ao terminal H2MRT.");
                }
            }
        }
    }
}
