using UnityEngine;
using System.Collections.Generic;
using System.Linq;


public class WireConnectionHandler : MonoBehaviour
{
    public Camera mainCamera; // A c�mera principal que segue o personagem
    public Transform player; // O transform do jogador
    public GameObject wirePrefab; // Prefab do LineRenderer para representar o fio
    public float wireThickness = 0.02f; // A espessura dos fios
    public int segments = 20; // O n�mero de segmentos da linha para simular a curva parab�lica
    public float maxReachDistance = 1.5f; // Dist�ncia m�xima que o avatar pode alcan�ar para conectar o fio
    public TransformerController transformerController; // Refer�ncia ao script TransformerController
    public VariacController variacController; // Refer�ncia ao script VariacController
    public MegometroController megometroController; // Refer�ncia ao script MegometroController
    public VoltimetroController voltimetroController1; // Refer�ncia ao script VoltimetroController do primeiro mult�metro
    public VoltimetroController voltimetroController2; // Refer�ncia ao script VoltimetroController do segundo mult�metro
    public VoltimetroController voltimetroController3; // Refer�ncia ao script VoltimetroController do terceiro mult�metro
    public AmperimetroController amperimetroController1; // Refer�ncia ao script AmperimetroController do primeiro amper�metro
    public AmperimetroController amperimetroController2; // Refer�ncia ao script AmperimetroController do segundo amper�metro
    public AmperimetroController amperimetroController3; // Refer�ncia ao script AmperimetroController do terceiro amper�metro
   // public Animator characterAnimator; // Refer�ncia ao Animator do personagem
    //public float placeWireAnimationDuration = 1.5f; // Dura��o da anima��o "PlaceWire"


    private List<LineRenderer> wires = new List<LineRenderer>(); // Lista para armazenar os fios criados
    private List<(Transform, Transform)> wireConnections = new List<(Transform, Transform)>(); // Lista para armazenar as conex�es de fios
    private Vector3 initialPoint; // Ponto inicial do fio
    private bool isConnecting = false; // Flag para determinar se estamos conectando um fio
    private LineRenderer currentLineRenderer; // Refer�ncia ao LineRenderer do fio atual
    private Transform terminalInicial; // O terminal inicial conectado
    public bool curtoCircuitoPrimario = false;
    public bool curtoCircuitoSecundario = false;

    void Update()
    {
        // Detectar clique do mouse para iniciar ou finalizar a conex�o do fio
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (Vector3.Distance(hit.point, player.position) <= maxReachDistance)
                {
                    if (!isConnecting)
                    {
                        // Iniciar a conex�o do fio no ponto de conex�o pr�-definido
                        initialPoint = hit.collider.bounds.center; // Usar o centro do colisor como ponto inicial
                        StartConnectingWire(initialPoint);

                        // Verificar se o ponto inicial � um terminal do Variac
                        if (hit.collider.transform == variacController.terminalPositivo)
                        {
                            Debug.Log("Conectando fio ao terminal + do Variac");
                            terminalInicial = hit.collider.transform;
                        }
                        else if (hit.collider.transform == variacController.terminalNegativo)
                        {
                            Debug.Log("Conectando fio ao terminal - do Variac");
                            terminalInicial = hit.collider.transform;
                        }
                        // Verificar se o ponto inicial � um terminal do transformador
                        else if (IsTransformadorTerminal(hit.collider.transform))
                        {
                            Debug.Log($"Conectando ao terminal {hit.collider.transform.name} do Transformador");
                            terminalInicial = hit.collider.transform;
                        }
                        // Verificar se o ponto inicial � um terminal do volt�metro
                        else if (IsVoltimetroTerminal(hit.collider.transform))
                        {
                            Debug.Log($"Conectando ao terminal {hit.collider.transform.name} do Volt�metro");
                            terminalInicial = hit.collider.transform;
                        }
                        // Verificar se o ponto inicial � um terminal do amper�metro
                        else if (IsAmperimetroTerminal(hit.collider.transform))
                        {
                            Debug.Log($"Conectando ao terminal {hit.collider.transform.name} do Amper�metro");
                            terminalInicial = hit.collider.transform;
                        }
                        // Verificar se o ponto inicial � um terminal do meg�metro
                        else if (IsMegometroTerminal(hit.collider.transform))
                        {
                            Debug.Log($"Conectando ao terminal {hit.collider.transform.name} do Meg�metro");
                            terminalInicial = hit.collider.transform;
                        }
                        // Verifique a conex�o com a massa
                        else if (hit.collider.transform == transformerController.massa.transform)
                        {
                            Debug.Log("Iniciando conex�o na massa.");
                            terminalInicial = hit.collider.transform;
                        }
                    }
                    else
                    {
                        // Finalizar a conex�o do fio no ponto clicado
                        Vector3 endPoint = hit.collider.bounds.center; // Usar o centro do colisor como ponto final
                        UpdateLineRenderer(currentLineRenderer, initialPoint, endPoint);
                        isConnecting = false;

                        // Verificar se o ponto final � um terminal do Variac
                        if (hit.collider.transform == variacController.terminalPositivo)
                        {
                            Debug.Log("Conectando fio ao terminal + do Variac");
                            transformerController.ConectarTerminal(terminalInicial, variacController.VPositivo);
                            transformerController.ConectarTerminal(hit.collider.transform, variacController.VPositivo);
                            ConectarTerminalVoltimetros(terminalInicial, variacController.VPositivo);
                            ConectarTerminalVoltimetros(hit.collider.transform, variacController.VPositivo);
                            ConectarTerminalAmperimetros(terminalInicial, hit.collider.transform, variacController.VPositivo);
                            wireConnections.Add((terminalInicial, hit.collider.transform));
                        }
                        else if (hit.collider.transform == variacController.terminalNegativo)
                        {
                            Debug.Log("Conectando fio ao terminal - do Variac");
                            transformerController.ConectarTerminal(terminalInicial, variacController.VNegativo);
                            transformerController.ConectarTerminal(hit.collider.transform, variacController.VNegativo);
                            ConectarTerminalVoltimetros(terminalInicial, variacController.VNegativo);
                            ConectarTerminalVoltimetros(hit.collider.transform, variacController.VNegativo);
                            ConectarTerminalAmperimetros(terminalInicial, hit.collider.transform, variacController.VNegativo);
                            wireConnections.Add((terminalInicial, hit.collider.transform));
                        }
                        else if (hit.collider.transform == transformerController.massa.transform)
                        {
                            Debug.Log("Finalizando conex�o na massa.");
                            ConectarTerminalMegometro(terminalInicial, hit.collider.transform);
                            wireConnections.Add((terminalInicial, hit.collider.transform));
                        }
                        // Verificar se o ponto final � um terminal do transformador
                        else if (IsTransformadorTerminal(hit.collider.transform))
                        {
                            Debug.Log($"Conectando ao terminal {hit.collider.transform.name} do Transformador");
                            float tensaoTerminalInicial = GetPotencialDoTerminal(terminalInicial);
                            transformerController.ConectarTerminal(hit.collider.transform, tensaoTerminalInicial);
                            ConectarTerminalVoltimetros(hit.collider.transform, tensaoTerminalInicial);
                            ConectarTerminalAmperimetros(terminalInicial, hit.collider.transform, tensaoTerminalInicial);
                            wireConnections.Add((terminalInicial, hit.collider.transform));
                        }
                        // Verificar se o ponto final � um terminal do volt�metro
                        else if (IsVoltimetroTerminal(hit.collider.transform))
                        {
                            Debug.Log($"Conectando ao terminal {hit.collider.transform.name} do Volt�metro");
                            float tensaoTerminalInicial = GetPotencialDoTerminal(terminalInicial);
                            ConectarTerminalVoltimetros(hit.collider.transform, tensaoTerminalInicial);
                            ConectarTerminalAmperimetros(terminalInicial, hit.collider.transform, tensaoTerminalInicial);
                            wireConnections.Add((terminalInicial, hit.collider.transform));
                        }
                        // Verificar se o ponto final � um terminal do amper�metro
                        else if (IsAmperimetroTerminal(hit.collider.transform))
                        {
                            Debug.Log($"Conectando ao terminal {hit.collider.transform.name} do Amper�metro");
                            float tensaoTerminalInicial = GetPotencialDoTerminal(terminalInicial);
                            ConectarTerminalAmperimetros(terminalInicial, hit.collider.transform, tensaoTerminalInicial);
                            wireConnections.Add((terminalInicial, hit.collider.transform));
                        }
                        // Verificar se o ponto final � um terminal do meg�metro
                        else if (IsMegometroTerminal(hit.collider.transform))
                        {
                            Debug.Log($"Conectando ao terminal {hit.collider.transform.name} do Meg�metro");
                            float tensaoTerminalInicial = GetPotencialDoTerminal(terminalInicial);
                            ConectarTerminalMegometro(terminalInicial, hit.collider.transform);
                            wireConnections.Add((terminalInicial, hit.collider.transform));
                        }
                        currentLineRenderer = null;
                    }
                }
            }
        }

        // Atualizar a posi��o do segundo ponto do fio enquanto est� conectando
        if (isConnecting && currentLineRenderer != null)
        {
            Vector3 playerPosition = player.position + Vector3.up * 1.0f; // Adicionar offset de 1 unidade no eixo Y
            UpdateLineRenderer(currentLineRenderer, initialPoint, playerPosition);
        }

        // Atualizar as tens�es dos terminais em tempo real
        UpdateWireConnections();

        // Detectar a tecla K para remover o �ltimo fio criado
        if (Input.GetKeyDown(KeyCode.K))
        {
            RemoveLastWire();
        }

        // Detectar curto-circuitos
        DetectarCurtoCircuito();
    }

    private void StartConnectingWire(Vector3 startPoint)
    {
        //characterAnimator.SetTrigger("PlaceWire");
        GameObject wireObject = Instantiate(wirePrefab); // Instanciar o prefab do fio
        currentLineRenderer = wireObject.GetComponent<LineRenderer>();
        currentLineRenderer.positionCount = segments + 1; // Ajustar o n�mero de segmentos
        currentLineRenderer.startWidth = wireThickness; // Ajustar a espessura do fio
        currentLineRenderer.endWidth = wireThickness; // Ajustar a espessura do fio
        currentLineRenderer.material = new Material(Shader.Find("Sprites/Default")); // Usar um material padr�o para cores
        currentLineRenderer.startColor = GetColor(terminalInicial); // Atribuir uma cor baseada no terminal inicial
        currentLineRenderer.endColor = currentLineRenderer.startColor; // Atribuir a mesma cor ao final do fio
        currentLineRenderer.SetPosition(0, startPoint);
        isConnecting = true;
        wires.Add(currentLineRenderer);
    }

   /* public void ToggleWireConnection()
    {
        // Ativar o gatilho "PlaceWireTrigger" no Animator do personagem
        characterAnimator.SetTrigger("PlaceWireTrigger");

        // Iniciar a Coroutine para retornar para "Idle" ap�s a anima��o "PlaceWire"
        StartCoroutine(ReturnToIdleAfterAnimation());
    }

    // Coroutine para retornar para "Idle" ap�s a anima��o "PlaceWire"
    IEnumerator ReturnToIdleAfterAnimation()
    {
        // Aguardar o tempo da dura��o da anima��o "PlaceWire"
        yield return new WaitForSeconds(placeWireAnimationDuration);

        // Voltar para "Idle" automaticamente
        characterAnimator.SetTrigger("IdleTrigger");
    }*/

    private void UpdateLineRenderer(LineRenderer lineRenderer, Vector3 startPoint, Vector3 endPoint)
    {
        if (lineRenderer == null) return; // Verificar se o LineRenderer ainda existe

        float distance = Vector3.Distance(startPoint, endPoint);

        if (distance > 1.5f)
        {
            // Criar fio com forma parab�lica
            lineRenderer.positionCount = segments + 1;
            for (int i = 0; i <= segments; i++)
            {
                float t = i / (float)segments;
                Vector3 point = Vector3.Lerp(startPoint, endPoint, t);
                float parabolicHeight = Mathf.Sin(Mathf.PI * t) * (distance / 2); // Simular gravidade com a dist�ncia real
                point.y -= parabolicHeight;
                if (point.y < 0.5f) point.y = 0.5f; // Garantir que a altura m�nima seja 0.5
                lineRenderer.SetPosition(i, point);
            }
        }
        else
        {
            // Criar fio reto
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, startPoint);
            lineRenderer.SetPosition(1, endPoint);
            for (int i = 0; i < lineRenderer.positionCount; i++)
            {
                Vector3 position = lineRenderer.GetPosition(i);
                if (position.y < 0.5f) position.y = 0.5f; // Garantir que a altura m�nima seja 0.5
                lineRenderer.SetPosition(i, position);
            }
        }
    }

    private Color GetColor(Transform terminal)
    {
        if (terminal == variacController.terminalPositivo || terminal == voltimetroController1.terminalPositivo || terminal == voltimetroController2.terminalPositivo || terminal == voltimetroController3.terminalPositivo || terminal == amperimetroController1.terminalPositivo || terminal == amperimetroController2.terminalPositivo || terminal == amperimetroController3.terminalPositivo)
        {
            return Color.red;
        }
        else if (terminal == variacController.terminalNegativo || terminal == voltimetroController1.terminalNegativo || terminal == voltimetroController2.terminalNegativo || terminal == voltimetroController3.terminalNegativo || terminal == amperimetroController1.terminalNegativo || terminal == amperimetroController2.terminalNegativo || terminal == amperimetroController3.terminalNegativo)
        {
            return Color.black;
        }
        else
        {
            return Color.green; // Fios conectados aos terminais do transformador
        }
    }

    private void RemoveLastWire()
    {
        if (isConnecting)
        {
            // Cancelar o fio incompleto
            isConnecting = false;
            if (currentLineRenderer != null)
            {
                wires.Remove(currentLineRenderer);
                Destroy(currentLineRenderer.gameObject);
                currentLineRenderer = null;
            }
        }
        else if (wires.Count > 0)
        {
            LineRenderer wireToRemove = wires[wires.Count - 1];
            wires.RemoveAt(wires.Count - 1);

            // Remover a conex�o correspondente
            if (wireConnections.Count > 0)
            {
                (Transform terminalA, Transform terminalB) = wireConnections[wireConnections.Count - 1];
                wireConnections.RemoveAt(wireConnections.Count - 1);

                // Atualizar a tens�o do terminal removido
                transformerController.DesconectarTerminal(terminalA);
                transformerController.DesconectarTerminal(terminalB);
                DesconectarTerminalVoltimetros(terminalA);
                DesconectarTerminalVoltimetros(terminalB);
                DesconectarTerminalAmperimetros(terminalA, terminalB);
            }

            Destroy(wireToRemove.gameObject);
        }
    }

    private bool IsTransformadorTerminal(Transform terminal)
    {
        foreach (Transform t in transformerController.altaTerminais)
        {
            if (t == terminal) return true;
        }
        foreach (Transform t in transformerController.baixaTerminais)
        {
            if (t == terminal) return true;
        }
        return false;
    }

    private bool IsVoltimetroTerminal(Transform terminal)
    {
        return terminal == voltimetroController1.terminalPositivo || terminal == voltimetroController1.terminalNegativo ||
               terminal == voltimetroController2.terminalPositivo || terminal == voltimetroController2.terminalNegativo ||
               terminal == voltimetroController3.terminalPositivo || terminal == voltimetroController3.terminalNegativo;
    }

    private bool IsAmperimetroTerminal(Transform terminal)
    {
        return terminal == amperimetroController1.terminalPositivo || terminal == amperimetroController1.terminalNegativo ||
               terminal == amperimetroController2.terminalPositivo || terminal == amperimetroController2.terminalNegativo ||
               terminal == amperimetroController3.terminalPositivo || terminal == amperimetroController3.terminalNegativo;
    }

    private bool IsMegometroTerminal(Transform terminal)
    {
        return terminal == megometroController.terminalPositivo || terminal == megometroController.terminalNegativo;
    }

    private float GetPotencialDoTerminal(Transform terminal)
    {
        if (terminal == variacController.terminalPositivo)
        {
            return variacController.VPositivo;
        }
        if (terminal == variacController.terminalNegativo)
        {
            return variacController.VNegativo;
        }

        if (IsTransformadorTerminal(terminal))
        {
            if (terminal == transformerController.altaTerminais[0]) return transformerController.VH1;
            if (terminal == transformerController.altaTerminais[1]) return transformerController.VH2;
            if (terminal == transformerController.altaTerminais[2]) return transformerController.VH3;
            if (terminal == transformerController.baixaTerminais[0]) return transformerController.VX0;
            if (terminal == transformerController.baixaTerminais[1]) return transformerController.VX1;
            if (terminal == transformerController.baixaTerminais[2]) return transformerController.VX2;
            if (terminal == transformerController.baixaTerminais[3]) return transformerController.VX3;
        }

        return 0f;
    }

    private void ConectarTerminalVoltimetros(Transform terminal, float tensao)
    {
        if (terminal == voltimetroController1.terminalPositivo || terminal == voltimetroController1.terminalNegativo)
        {
            voltimetroController1.ConectarTerminal(terminal, tensao);
        }
        if (terminal == voltimetroController2.terminalPositivo || terminal == voltimetroController2.terminalNegativo)
        {
            voltimetroController2.ConectarTerminal(terminal, tensao);
        }
        if (terminal == voltimetroController3.terminalPositivo || terminal == voltimetroController3.terminalNegativo)
        {
            voltimetroController3.ConectarTerminal(terminal, tensao);
        }
    }

    private void DesconectarTerminalVoltimetros(Transform terminal)
    {
        if (terminal == voltimetroController1.terminalPositivo || terminal == voltimetroController1.terminalNegativo)
        {
            voltimetroController1.DesconectarTerminal(terminal);
        }
        if (terminal == voltimetroController2.terminalPositivo || terminal == voltimetroController2.terminalNegativo)
        {
            voltimetroController2.DesconectarTerminal(terminal);
        }
        if (terminal == voltimetroController3.terminalPositivo || terminal == voltimetroController3.terminalNegativo)
        {
            voltimetroController3.DesconectarTerminal(terminal);
        }
    }

    public Transform GetTerminalConectado(Transform terminal)
    {
        foreach (var connection in wireConnections)
        {
            if (connection.Item1 == terminal) return connection.Item2;
            if (connection.Item2 == terminal) return connection.Item1;
        }
        return null;
    }

    private void ConectarTerminalAmperimetros(Transform terminalInicial, Transform terminalFinal, float tensao)
    {
        if (IsAmperimetroTerminal(terminalInicial) && IsTransformadorTerminal(terminalFinal))
        {
            AmperimetroController amperimetro = GetAmperimetroController(terminalInicial);
            if (amperimetro != null)
            {
                amperimetro.ConectarTerminal(terminalInicial, tensao, transformerController.GetCorrenteDoTerminal(terminalFinal));
            }
        }
        else if (IsAmperimetroTerminal(terminalFinal) && IsTransformadorTerminal(terminalInicial))
        {
            AmperimetroController amperimetro = GetAmperimetroController(terminalFinal);
            if (amperimetro != null)
            {
                amperimetro.ConectarTerminal(terminalFinal, tensao, transformerController.GetCorrenteDoTerminal(terminalInicial));
            }
        }
    }

    private void DesconectarTerminalAmperimetros(Transform terminalA, Transform terminalB)
    {
        if (IsAmperimetroTerminal(terminalA))
        {
            AmperimetroController amperimetro = GetAmperimetroController(terminalA);
            if (amperimetro != null)
            {
                amperimetro.DesconectarTerminal(terminalA);
            }
        }
        if (IsAmperimetroTerminal(terminalB))
        {
            AmperimetroController amperimetro = GetAmperimetroController(terminalB);
            if (amperimetro != null)
            {
                amperimetro.DesconectarTerminal(terminalB);
            }
        }
    }

    private void ConectarTerminalMegometro(Transform terminalA, Transform terminalB)
    {
        if (IsMegometroTerminal(terminalA) || IsMegometroTerminal(terminalB))
        {
            Debug.Log($"Conectando Meg�metro entre {terminalA.name} e {terminalB.name}");
            megometroController.ConectarTerminal(terminalA, terminalB);
        }
    }

    private AmperimetroController GetAmperimetroController(Transform terminal)
    {
        if (terminal == amperimetroController1.terminalPositivo || terminal == amperimetroController1.terminalNegativo)
        {
            return amperimetroController1;
        }
        if (terminal == amperimetroController2.terminalPositivo || terminal == amperimetroController2.terminalNegativo)
        {
            return amperimetroController2;
        }
        if (terminal == amperimetroController3.terminalPositivo || terminal == amperimetroController3.terminalNegativo)
        {
            return amperimetroController3;
        }
        return null;
    }

    private void UpdateWireConnections()
    {
        foreach (var connection in wireConnections)
        {
            Transform terminalA = connection.Item1;
            Transform terminalB = connection.Item2;

            float potencialA = GetPotencialDoTerminal(terminalA);
            float potencialB = GetPotencialDoTerminal(terminalB);

            if (terminalA == variacController.terminalPositivo || terminalB == variacController.terminalPositivo)
            {
                if (IsTransformadorTerminal(terminalA))
                {
                    transformerController.ConectarTerminal(terminalA, variacController.VPositivo);
                }
                if (IsTransformadorTerminal(terminalB))
                {
                    transformerController.ConectarTerminal(terminalB, variacController.VPositivo);
                }
                ConectarTerminalVoltimetros(terminalA, variacController.VPositivo);
                ConectarTerminalVoltimetros(terminalB, variacController.VPositivo);
                ConectarTerminalAmperimetros(terminalA, terminalB, variacController.VPositivo);
            }
            else if (terminalA == variacController.terminalNegativo || terminalB == variacController.terminalNegativo)
            {
                if (IsTransformadorTerminal(terminalA))
                {
                    transformerController.ConectarTerminal(terminalA, variacController.VNegativo);
                }
                if (IsTransformadorTerminal(terminalB))
                {
                    transformerController.ConectarTerminal(terminalB, variacController.VNegativo);
                }
                ConectarTerminalVoltimetros(terminalA, variacController.VNegativo);
                ConectarTerminalVoltimetros(terminalB, variacController.VNegativo);
                ConectarTerminalAmperimetros(terminalA, terminalB, variacController.VNegativo);
            }
            else
            {
                // Atualizar conex�es entre terminais do transformador e volt�metro/amper�metro
                if (IsTransformadorTerminal(terminalA))
                {
                    if (IsVoltimetroTerminal(terminalB))
                    {
                        voltimetroController1.ConectarTerminal(terminalB, potencialA);
                        voltimetroController2.ConectarTerminal(terminalB, potencialA);
                        voltimetroController3.ConectarTerminal(terminalB, potencialA);
                    }
                    if (IsAmperimetroTerminal(terminalB))
                    {
                        ConectarTerminalAmperimetros(terminalB, terminalA, potencialA);
                    }
                }
                if (IsTransformadorTerminal(terminalB))
                {
                    if (IsVoltimetroTerminal(terminalA))
                    {
                        voltimetroController1.ConectarTerminal(terminalA, potencialB);
                        voltimetroController2.ConectarTerminal(terminalA, potencialB);
                        voltimetroController3.ConectarTerminal(terminalA, potencialB);
                    }
                    if (IsAmperimetroTerminal(terminalA))
                    {
                        ConectarTerminalAmperimetros(terminalA, terminalB, potencialB);
                    }
                }
            }
        }

        // Atualizar tens�es em tempo real
        transformerController.AtualizarTensoes();
    }

    private void DetectarCurtoCircuito()
    {
        curtoCircuitoPrimario = wireConnections.Any(connection =>
            transformerController.altaTerminais.Contains(connection.Item1) &&
            transformerController.altaTerminais.Contains(connection.Item2) &&
            wireConnections.Any(otherConnection =>
                otherConnection != connection &&
                transformerController.altaTerminais.Contains(otherConnection.Item1) &&
                transformerController.altaTerminais.Contains(otherConnection.Item2))
        );

        curtoCircuitoSecundario = wireConnections.Any(connection =>
            transformerController.baixaTerminais.Contains(connection.Item1) &&
            transformerController.baixaTerminais.Contains(connection.Item2) &&
            wireConnections.Any(otherConnection =>
                otherConnection != connection &&
                transformerController.baixaTerminais.Contains(otherConnection.Item1) &&
                transformerController.baixaTerminais.Contains(otherConnection.Item2))
        );

        if (curtoCircuitoPrimario)
        {
            Debug.Log("Curto-circuito no prim�rio detectado!");
        }

        if (curtoCircuitoSecundario)
        {
            Debug.Log("Curto-circuito no secund�rio detectado!");
        }
    }
}

