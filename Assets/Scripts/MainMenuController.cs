using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void StartGame()
    {
        // Carrega a cena do jogo
        SceneManager.LoadScene("Menu");
    }

    public void OpenExperimentGuide()
    {
        // Coloque aqui o código para abrir a tela de guia de experimentos
    }
}
