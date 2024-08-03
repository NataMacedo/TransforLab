using UnityEngine;
using UnityEngine.UI;

public class InspecaoVisualUI : MonoBehaviour
{
    public Transform avatar; // Referência ao Transform do avatar
    public Transform transformador; // Referência ao Transform do transformador
    public GameObject inspecaoVisualPanel; // Referência ao painel da UI
    public Image inspecaoVisualImage; // Referência ao componente Image
    public float distanciaAtivacao = 3f; // Distância para ativação da UI

    private bool isUIActive = false;

    void Start()
    {
        // Certifique-se de que a imagem está desativada no início
        if (inspecaoVisualPanel != null)
        {
            inspecaoVisualPanel.SetActive(false);
        }

        if (inspecaoVisualImage != null)
        {
            inspecaoVisualImage.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        float distancia = Vector3.Distance(avatar.position, transformador.position);

        // Verifica a distância e a entrada do teclado para ativar/desativar a UI
        if (distancia <= distanciaAtivacao && Input.GetKeyDown(KeyCode.I))
        {
            isUIActive = !isUIActive;
            if (inspecaoVisualPanel != null)
            {
                inspecaoVisualPanel.SetActive(isUIActive);
            }

            if (inspecaoVisualImage != null)
            {
                inspecaoVisualImage.gameObject.SetActive(isUIActive);
            }
        }

        // Desativa a UI se o avatar se afastar mais de 3 metros
        if (distancia > distanciaAtivacao && isUIActive)
        {
            isUIActive = false;
            if (inspecaoVisualPanel != null)
            {
                inspecaoVisualPanel.SetActive(false);
            }

            if (inspecaoVisualImage != null)
            {
                inspecaoVisualImage.gameObject.SetActive(false);
            }
        }
    }
}
