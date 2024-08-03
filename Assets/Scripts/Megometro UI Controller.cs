using UnityEngine;
using TMPro;

public class MegometroUIController : MonoBehaviour
{
    public MegometroController megometroController;
    public TextMeshProUGUI megometroText;
    public WireConnectionHandler wireConnectionHandler;
    private bool isActive = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            isActive = !isActive;
            megometroText.text = $"";
        }

        if (isActive)
        {
            float tensao = megometroController.tensaoMegometro;
            if (tensao < 1000)
            {
                megometroText.text = $"Tensão: {tensao} V\nTensão Insuficiente para medição de resistência de isolamento";
            }
            else
            {
                bool curtoPrimario = wireConnectionHandler.curtoCircuitoPrimario;
                bool curtoSecundario = wireConnectionHandler.curtoCircuitoSecundario;
                if (curtoPrimario && curtoSecundario)
                {
                    float resistencia = megometroController.resistencia;
                    megometroText.text = $"Tensão: {tensao} V\nResistência: {resistencia} MΩ";
                }
                else
                {
                    megometroText.text = $"Tensão: {tensao} V\nConexões não realizadas corretamente.";
                }
            }
        }
    }
}
