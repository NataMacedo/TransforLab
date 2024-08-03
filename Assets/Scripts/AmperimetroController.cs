using UnityEngine;

public class AmperimetroController : MonoBehaviour
{
    public Transform terminalPositivo;
    public Transform terminalNegativo;

    private float tensaoAmperimetro = 0f;
    private float correnteAmperimetro = 0f;

    public float CorrenteAmperimetro
    {
        get { return correnteAmperimetro; }
        set { correnteAmperimetro = value; }
    }

    public float TensaoAmperimetro
    {
        get { return tensaoAmperimetro; }
        set { tensaoAmperimetro = value; }
    }

    public void ConectarTerminal(Transform terminal, float tensao, float corrente)
    {
        if (terminal == terminalPositivo)
        {
            tensaoAmperimetro = tensao;
           
        }
        if (terminal == terminalNegativo)
        {
            tensaoAmperimetro = tensao;
            correnteAmperimetro = corrente;
        }
    }

    public void DesconectarTerminal(Transform terminal)
    {
        if (terminal == terminalPositivo || terminal == terminalNegativo)
        {
            tensaoAmperimetro = 0f;
            correnteAmperimetro = 0f;
        }
    }
}
