using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadSimulatorScene()
    {
        SceneManager.LoadScene("Simulador");
    }
}