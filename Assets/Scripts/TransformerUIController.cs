using UnityEngine;
using TMPro;

public class TransformerUIController : MonoBehaviour
{
    public TransformerController transformerController; // Referência ao script TransformerController

    public TMP_Text infoText; // Referência ao componente TextMeshPro para exibir as informações

    void Update()
    {
        if (transformerController != null)
        {
            // Atualizar o texto com os valores das tensões e correntes dos terminais do transformador
            infoText.text = $"Tensões:\n" +
                            $"VH1: {transformerController.VH1:F2} V\n" +
                            $"VH2: {transformerController.VH2:F2} V\n" +
                            $"VH3: {transformerController.VH3:F2} V\n" +
                            $"VX0: {transformerController.VX0:F2} V\n" +
                            $"VX1: {transformerController.VX1:F2} V\n" +
                            $"VX2: {transformerController.VX2:F2} V\n" +
                            $"VX3: {transformerController.VX3:F2} V\n\n" +
                            $"Correntes:\n" +
                            $"AH1: {transformerController.AH1:F4} A\n" +
                            $"AH2: {transformerController.AH2:F4} A\n" +
                            $"AH3: {transformerController.AH3:F4} A\n" +
                            $"AX0: {transformerController.AX0:F4} A\n" +
                            $"AX1: {transformerController.AX1:F4} A\n" +
                            $"AX2: {transformerController.AX2:F4} A\n" +
                            $"AX3: {transformerController.AX3:F4} A";
        }
    }
}
