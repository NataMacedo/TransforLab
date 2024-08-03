using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToMenuButton : MonoBehaviour
{
    // Nome da cena do menu
    public string menuSceneName = "Menu";

    // M�todo chamado quando o bot�o � pressionado
    public void BackToMenu()
    {
        // Carrega a cena do menu
        SceneManager.LoadScene(menuSceneName);
    }
}
