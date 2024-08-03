using UnityEngine;
using TMPro;

public class MegometroController : MonoBehaviour
{
    public Transform terminalPositivo;
    public Transform terminalNegativo;
    public TextMeshProUGUI megometroText;
    public WireConnectionHandler wireConnectionHandler;
    public TransformerController transformerController;
    public float tensaoMegometro;
    public float resistencia;
    private bool isMeasuring = false;
    private Transform terminalMassa;

    void Start()
    {
        if (!transformerController || !wireConnectionHandler)
        {
            Debug.LogError("Transformador ou WireConnectionHandler não atribuídos.");
            return;
        }

        terminalMassa = transformerController.massa;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            isMeasuring = !isMeasuring;
            megometroText.text = isMeasuring ? "Megômetro Ativado" : "Megômetro Desativado";
        }

        if (isMeasuring)
        {
            AtualizarTensao();
            AtualizarResistencia();
        }
    }

    public void ConectarTerminal(Transform terminal, Transform terminalConectado)
    {
        // Lógica de conexão de terminais do megômetro
        Debug.Log($"Conectando {terminal.name} com {terminalConectado.name}");
    }

    private void AtualizarTensao()
    {
        tensaoMegometro = Mathf.Clamp(tensaoMegometro + Input.mouseScrollDelta.y * 250, 0, 1000);
        Debug.Log("Tensão Megômetro: " + tensaoMegometro);
    }

    private void AtualizarResistencia()
    {
        Transform terminalPositivoConectado = wireConnectionHandler.GetTerminalConectado(terminalPositivo);
        Transform terminalNegativoConectado = wireConnectionHandler.GetTerminalConectado(terminalNegativo);

        if (terminalPositivoConectado != null && terminalNegativoConectado != null)
        {
            bool isAltaBaixa = (System.Array.Exists(transformerController.altaTerminais, t => t == terminalPositivoConectado) &&
                                System.Array.Exists(transformerController.baixaTerminais, t => t == terminalNegativoConectado)) ||
                               (System.Array.Exists(transformerController.altaTerminais, t => t == terminalNegativoConectado) &&
                                System.Array.Exists(transformerController.baixaTerminais, t => t == terminalPositivoConectado));

            bool isAltaMassa = (System.Array.Exists(transformerController.altaTerminais, t => t == terminalPositivoConectado) && terminalMassa == terminalNegativoConectado) ||
                               (System.Array.Exists(transformerController.altaTerminais, t => t == terminalNegativoConectado) && terminalMassa == terminalPositivoConectado);

            bool isBaixaMassa = (System.Array.Exists(transformerController.baixaTerminais, t => t == terminalPositivoConectado) && terminalMassa == terminalNegativoConectado) ||
                                (System.Array.Exists(transformerController.baixaTerminais, t => t == terminalNegativoConectado) && terminalMassa == terminalPositivoConectado);

            if (isAltaBaixa)
            {
                resistencia = 744;
            }
            else if (isAltaMassa)
            {
                resistencia = 295;
            }
            else if (isBaixaMassa)
            {
                resistencia = 580;
            }
            else
            {
                resistencia = 0;
            }

            if (tensaoMegometro >= 1000)
            {
                megometroText.text = "Tensão: " + tensaoMegometro + " V\nResistência: " + resistencia + " MΩ";
            }
            else
            {
                megometroText.text = "Tensão: " + tensaoMegometro + " V\nTensão Insuficiente para medição de resistência de isolamento";
            }
        }
        else
        {
            megometroText.text = "Conexões não realizadas corretamente.";
        }
    }
}
