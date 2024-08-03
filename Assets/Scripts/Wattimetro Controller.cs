using UnityEngine;

public class WattimetroController : MonoBehaviour
{
    public Transform terminalPositivo;
    public Transform terminalNegativo;
    private float potencia;

    public float Potencia
    {
        get { return potencia; }
        set { potencia = value; }
    }

    public void ConectarTerminal(Transform terminal, float valor)
    {
        if (terminal == terminalPositivo || terminal == terminalNegativo)
        {
            potencia = valor; // Simplificação para o exemplo
        }
    }

    public void DesconectarTerminal(Transform terminal)
    {
        if (terminal == terminalPositivo || terminal == terminalNegativo)
        {
            potencia = 0f;
        }
    }
}
