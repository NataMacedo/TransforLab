using UnityEngine;

//public WireConnectionHandler wireConnectionHandler;
//bool curtoSecundario = wireConnectionHandler.curtoCircuitoSecundario;
public class TransformerController : MonoBehaviour
{
    public Transform[] altaTerminais; // Array de terminais de alta tensão
    public Transform[] baixaTerminais; // Array de terminais de baixa tensão
    public Transform massa; // Transform que representa a massa do transformador


    public float VH1 { get; private set; } = 0f;
    public float VH2 { get; private set; } = 0f;
    public float VH3 { get; private set; } = 0f;
    public float VX0 { get; private set; } = 0f;
    public float VX1 { get; private set; } = 0f;
    public float VX2 { get; private set; } = 0f;
    public float VX3 { get; private set; } = 0f;
    public float AH1 { get; private set; } = 0f;
    public float AH2 { get; private set; } = 0f;
    public float AH3 { get; private set; } = 0f;
    public float AX0 { get; private set; } = 0f;
    public float AX1 { get; private set; } = 0f;
    public float AX2 { get; private set; } = 0f;
    public float AX3 { get; private set; } = 0f;

    private const float relacaoTransformacao1 = 63.2653f;
    private const float relacaoTransformacao2 = 62.7456f;
    private const float relacaoTransformacao3 = 62.7431f;



    public void ConectarTerminal(Transform terminal, float tensao)
    {
        if (terminal == altaTerminais[0]) VH1 = tensao;
        else if (terminal == altaTerminais[1]) VH2 = tensao;
        else if (terminal == altaTerminais[2]) VH3 = tensao;
        else if (terminal == baixaTerminais[0]) VX0 = tensao;
        else if (terminal == baixaTerminais[1]) VX1 = tensao;
        else if (terminal == baixaTerminais[2]) VX2 = tensao;
        else if (terminal == baixaTerminais[3]) VX3 = tensao;

        AtualizarTensoes();
    }

    public void DesconectarTerminal(Transform terminal)
    {
        if (terminal == altaTerminais[0]) VH1 = 0f;
        else if (terminal == altaTerminais[1]) VH2 = 0f;
        else if (terminal == altaTerminais[2]) VH3 = 0f;
        else if (terminal == baixaTerminais[0]) VX0 = 0f;
        else if (terminal == baixaTerminais[1]) VX1 = 0f;
        else if (terminal == baixaTerminais[2]) VX2 = 0f;
        else if (terminal == baixaTerminais[3]) VX3 = 0f;

        AtualizarTensoes();
    }


    public void AtualizarTensoes()
    {
        if (VX1 != 0 || VX2 != 0 || VX3 != 0)
        {
            if (VX1 != 0) VH1 = VX1 * relacaoTransformacao1;
            if (VX2 != 0) VH2 = VX2 * relacaoTransformacao2;
            if (VX3 != 0) VH3 = VX3 * relacaoTransformacao3;


            if (VX1 == 0) VH1 = 0;
            if (VX2 == 0) VH2 = 0;
            if (VX3 == 0) VH3 = 0;

            AX1 = VX1 != 0 ? 6.63e-4f * Mathf.Exp(0.0353f * VX1) : 0f;
            AX2 = VX2 != 0 ? 1.26e-3f * Mathf.Exp(0.0328f * VX2) : 0f;
            AX3 = VX3 != 0 ? 1.84e-3f * Mathf.Exp(0.0321f * VX3) : 0f;

        }
        // Calcular tensões de alta tensão (lado primário)
        if (AH1 != 0 || AH2 != 0 || AH3 != 0)
        {

            if (VH1 != 0) VX1 = VH1 / relacaoTransformacao1;
            if (VH2 != 0) VX2 = VH2 / relacaoTransformacao2;
            if (VH3 != 0) VX3 = VH3 / relacaoTransformacao3;


            //if (VA1 == 0) VX1 = 0;
            //if (VA2 == 0) VX2 = 0;
            //if (VA3 == 0) VX3 = 0;

            AH1 = VH1 != 0 ? AX1 / relacaoTransformacao1 : 0f;
            AH2 = VH2 != 0 ? AX2 / relacaoTransformacao2 : 0f;
            AH3 = VH3 != 0 ? AX3 / relacaoTransformacao3 : 0f;

            //if (curtoSecundario)
            //

            //AH2 = VH2 != 0 ? 0.0394f*VH2+0.142f : 0f;


            //}
            //else
            //{

            //}

        }

        // Calcular tensões de baixa tensão (lado secundário)



    }

    public float GetCorrenteDoTerminal(Transform terminal)
    {
        if (terminal == altaTerminais[0]) return AH1;
        if (terminal == altaTerminais[1]) return AH2;
        if (terminal == altaTerminais[2]) return AH3;
        if (terminal == baixaTerminais[0]) return AX0;
        if (terminal == baixaTerminais[1]) return AX1;
        if (terminal == baixaTerminais[2]) return AX2;
        if (terminal == baixaTerminais[3]) return AX3;

        return 0f;
    }
}