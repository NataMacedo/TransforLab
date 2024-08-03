using UnityEngine;

public class VariacController : MonoBehaviour
{
    public Transform terminalPositivo;
    public Transform terminalNegativo;
    private float tensaoVariac = 0f;

    public float VPositivo
    {
        get { return tensaoVariac; }
    }

    public float VNegativo => 0f;

   
    void Update()
    {
        
        
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (scroll != 0f)
            {
                tensaoVariac += scroll > 0f ? 1f : -1f;
                tensaoVariac = Mathf.Clamp(tensaoVariac, 0f, 500f);
            }
        
    }
}
