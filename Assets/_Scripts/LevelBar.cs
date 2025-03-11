using UnityEngine;
using UnityEngine.UI;

public class LevelBar : MonoBehaviour
{
    public Slider progressBar; // Reference to the Slider component
    private float currentProgress = 0f; // Current progress value

    void Start()
    {
        // Initialize the progress bar
        if (progressBar != null)
        {
            progressBar.value = currentProgress;
        }
    }

    void Update()
    {
        // Simulate progress for demonstration purposes
        // In a real game, you would update this based on actual level progress
        if (currentProgress < 1f)
        {
            currentProgress += Time.deltaTime * 0.1f; // Adjust speed as needed
            progressBar.value = currentProgress;
        }
    }

    // Call this method to update the progress bar from other scripts
    public void UpdateProgress(float progress)
    {
        currentProgress = Mathf.Clamp01(progress); // Ensure progress is between 0 and 1
        progressBar.value = currentProgress;
    }
}