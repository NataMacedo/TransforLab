using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MRTUIController : MonoBehaviour
{
    public TextMeshProUGUI relacaoRealText;
    public TextMeshProUGUI statusText;
    public GameObject mrtUIPanel;  // Painel que contém os textos da UI

    private float relacaoMedida1 = 62.7431f;
    private float relacaoReal = 62.7272f;

    private void Start()
    {
        mrtUIPanel.SetActive(false); // Inicia com a UI desativada
        UpdateUI();
    }

    private void Update()
    {
        // Atualização da relação real com o scroll do mouse
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

        // Ativação ou desativação da UI com a tecla N
        if (Input.GetKeyDown(KeyCode.N))
        {
            mrtUIPanel.SetActive(!mrtUIPanel.activeSelf);  // Alternar visibilidade do painel da UI
        }
    }

    private void UpdateUI()
    {
        // Atualiza o texto da relação real
        relacaoRealText.text = "Relação de Transformação Medida: " + relacaoReal.ToString("F4");

        // Atualiza o status com base na comparação da relação real e medida
        if (relacaoReal < relacaoMedida1)
        {
            statusText.text = " +";  // Relação real é menor, mostra +
        }
       
        else if(relacaoReal == relacaoMedida1)
        {
            statusText.text = "Equilíbrio Alcançado";  // Relação real é igual, mostra =
        }
        else if (relacaoReal > relacaoMedida1)
        {
            statusText.text = "-";  // Relação real é maior, mostra -
        }
    }
}
