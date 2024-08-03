using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToMenuButton : MonoBehaviour
{
    // Nome da cena do menu
    public string menuSceneName = "Menu";

    // Método chamado quando o botão é pressionado
    public void BackToMenu()
    {
        // Carrega a cena do menu
        SceneManager.LoadScene(menuSceneName);
    }
}
