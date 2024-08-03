using UnityEngine;
using TMPro;

public class VoltimetroUIController : MonoBehaviour
{
    public VoltimetroController voltimetroController1;
    public VoltimetroController voltimetroController2;
    public VoltimetroController voltimetroController3;
    public TMP_Text tensaoText;
    private bool isActive = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            isActive = !isActive;
            tensaoText.gameObject.SetActive(isActive);
        }

        if (isActive)
        {
            tensaoText.text = $"Voltímetro 1: {voltimetroController1.TensaoVoltimetro:F2} V\n" +
                              $"Voltímetro 2: {voltimetroController2.TensaoVoltimetro:F2} V\n" +
                              $"Voltímetro 3: {voltimetroController3.TensaoVoltimetro:F2} V";
        }
    }
}
