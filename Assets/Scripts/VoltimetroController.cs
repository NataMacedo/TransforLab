using UnityEngine;

public class VoltimetroController : MonoBehaviour
{
    public Transform terminalPositivo;
    public Transform terminalNegativo;

    private float tensaoTerminalPositivo;
    private float tensaoTerminalNegativo;

    public float TensaoVoltimetro
    {
        get { return Mathf.Abs(tensaoTerminalPositivo - tensaoTerminalNegativo); }
    }

    public void ConectarTerminal(Transform terminal, float tensao)
    {
        if (terminal == terminalPositivo)
        {
            tensaoTerminalPositivo = tensao;
        }
        else if (terminal == terminalNegativo)
        {
            tensaoTerminalNegativo = tensao;
        }
    }

    public void DesconectarTerminal(Transform terminal)
    {
        if (terminal == terminalPositivo)
        {
            tensaoTerminalPositivo = 0f;
        }
        else if (terminal == terminalNegativo)
        {
            tensaoTerminalNegativo = 0f;
        }
    }
}
