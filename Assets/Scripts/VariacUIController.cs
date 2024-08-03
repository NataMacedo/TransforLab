using UnityEngine;
using TMPro;

public class VariacUIController : MonoBehaviour
{
    public VariacController variacController;
    public TMP_Text tensaoText;
    private bool isActive = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            isActive = !isActive;
            tensaoText.gameObject.SetActive(isActive);
        }

        if (isActive)
        {
            tensaoText.text = $"Tensão Variac: {variacController.VPositivo:F2} V";
        }
    }
}
