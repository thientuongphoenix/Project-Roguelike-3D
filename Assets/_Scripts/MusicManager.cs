using UnityEngine;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour
{
    private AudioSource audioSource;
    private bool isMusicOn = true;
    public Button musicToggleButton; // Reference to the button
    private Text buttonText; // Reference to the button text
    private Image buttonImage; // Reference to the button's Image component

    // Define colors for the button states
    public Color defaultColor = Color.white; // Default color when music is on
    public Color offColor = Color.gray; // Color when music is off

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.Play(); // Start playing music
        buttonText = musicToggleButton.GetComponentInChildren<Text>(); // Get the button text
        buttonImage = musicToggleButton.GetComponent<Image>(); // Get the button's Image component
        UpdateButtonText(); // Update the button text on start
        UpdateButtonColor(); // Set the initial button color
    }

    public void ToggleMusic()
    {
        isMusicOn = !isMusicOn; // Toggle the music state
        if (isMusicOn)
        {
            audioSource.Play(); // Play music
        }
        else
        {
            audioSource.Stop(); // Stop music
        }
        UpdateButtonText(); // Update the button text
        UpdateButtonColor(); // Update the button color
    }

    private void UpdateButtonText()
    {
        buttonText.text = isMusicOn ? "Music On" : "Music Off";
    }

    private void UpdateButtonColor()
    {
        buttonImage.color = isMusicOn ? defaultColor : offColor; // Change button color based on music state
    }
}
