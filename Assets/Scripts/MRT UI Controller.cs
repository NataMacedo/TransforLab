using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MRTUIController : MonoBehaviour
{
    public TextMeshProUGUI relacaoRealText;
    public TextMeshProUGUI statusText;
    public GameObject mrtUIPanel;  // Painel que cont�m os textos da UI

    private float relacaoMedida1 = 62.7431f;
    private float relacaoReal = 62.7272f;

    private void Start()
    {
        mrtUIPanel.SetActive(false); // Inicia com a UI desativada
        UpdateUI();
    }

    private void Update()
    {
        // Atualiza��o da rela��o real com o scroll do mouse
        if (Input.GetAxis("Mouse ScrollWheel") > 0f) // Scroll up
        {
            relacaoReal += 0.0001f;
            UpdateUI();
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f) // Scroll down
        {
            relacaoReal -= 0.0001f;
            UpdateUI();
        }

        // Ativa��o ou desativa��o da UI com a tecla N
        if (Input.GetKeyDown(KeyCode.N))
        {
            mrtUIPanel.SetActive(!mrtUIPanel.activeSelf);  // Alternar visibilidade do painel da UI
        }
    }

    private void UpdateUI()
    {
        // Atualiza o texto da rela��o real
        relacaoRealText.text = "Rela��o de Transforma��o Medida: " + relacaoReal.ToString("F4");

        // Atualiza o status com base na compara��o da rela��o real e medida
        if (relacaoReal < relacaoMedida1)
        {
            statusText.text = " +";  // Rela��o real � menor, mostra +
        }
       
        else if(relacaoReal == relacaoMedida1)
        {
            statusText.text = "Equil�brio Alcan�ado";  // Rela��o real � igual, mostra =
        }
        else if (relacaoReal > relacaoMedida1)
        {
            statusText.text = "-";  // Rela��o real � maior, mostra -
        }
    }
}
