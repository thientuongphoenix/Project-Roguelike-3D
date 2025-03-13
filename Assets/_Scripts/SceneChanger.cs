using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public void StartGame()
    {
        // Load the loading scene
        SceneManager.LoadScene("LoadingScene");
    }
}