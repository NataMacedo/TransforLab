using UnityEngine;
using TMPro;

public class AmperimetroUIController : MonoBehaviour
{
    public AmperimetroController amperimetroController1;
    public AmperimetroController amperimetroController2;
    public AmperimetroController amperimetroController3;

    public TMP_Text amperimetroText;

    private bool isUIActive = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            isUIActive = !isUIActive;
            amperimetroText.gameObject.SetActive(isUIActive);
        }

        if (isUIActive)
        {
            amperimetroText.text = $"Amper�metro 1: {amperimetroController1.CorrenteAmperimetro:F4} A\n" +
                                   $"Amper�metro 2: {amperimetroController2.CorrenteAmperimetro:F4} A\n" +
                                   $"Amper�metro 3: {amperimetroController3.CorrenteAmperimetro:F4} A";
        }
    }
}
