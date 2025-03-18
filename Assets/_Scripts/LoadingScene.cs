using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class LoadingScene : MonoBehaviour
{
    public Slider loadingSlider; // Assign your slider in the Inspector
    public Text loadingText;

    private void Start()
    {
        // Start the loading simulation
        StartCoroutine(SimulateLoading());
    }

    private IEnumerator SimulateLoading()
    {
        // Simulate loading progress
        for (float progress = 0; progress <= 1; progress += 0.01f)
        {
            loadingSlider.value = progress; // Update the slider
            loadingText.text = Mathf.RoundToInt(progress * 100) + "%"; // Update the text
            yield return new WaitForSeconds(0.05f); // Wait for a short time to simulate loading
        }

        // After simulating, load the actual game scene
        LoadScene("Map00"); // Replace "GameScene" with the actual name of your game scene
    }

    public void LoadScene(string sceneName)
    {
        // Load the scene synchronously
        SceneManager.LoadScene(sceneName);
    }
}