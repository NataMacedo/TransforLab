using UnityEngine;
using TMPro;

public class WattimetroUIController : MonoBehaviour
{
    public WattimetroController wattimetroController;
    public TMP_Text potenciaText;
    private bool isActive = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            isActive = !isActive;
            potenciaText.gameObject.SetActive(isActive);
        }

        if (isActive)
        {
            potenciaText.text = $"Potência: {wattimetroController.Potencia:F2} W";
        }
    }
}
