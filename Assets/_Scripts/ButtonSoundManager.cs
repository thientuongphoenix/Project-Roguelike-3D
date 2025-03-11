using UnityEngine;
using UnityEngine.UI;

public class ButtonSoundManager : MonoBehaviour
{
    public AudioClip[] buttonSounds; // Assign your button sounds in the Inspector
    private AudioSource audioSource;
    public Button soundToggleButton; // Reference to the button
    private Text buttonText; // Reference to the button text
    private Image buttonImage; // Reference to the button's Image component
    private bool isSoundOn = true; // Track sound state

    // Define colors for the button states
    public Color defaultColor = Color.white; // Default color when music is on
    public Color offColor = Color.gray; // Color when music is off
    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        buttonText = soundToggleButton.GetComponentInChildren<Text>(); // Get the button text
        buttonImage = soundToggleButton.GetComponent<Image>(); // Get the button's Image component
        UpdateButtonText(); // Update the button text on start
        UpdateButtonColor(); // Set the initial button color
    }

    public void PlaySound(int soundIndex)
    {
        if (isSoundOn && soundIndex >= 0 && soundIndex < buttonSounds.Length)
        {
            audioSource.PlayOneShot(buttonSounds[soundIndex]);
        }
    }

    public void ToggleSound(Text buttonText)
    {
        isSoundOn = !isSoundOn; // Toggle sound state
        UpdateButtonText();
        UpdateButtonColor();
    }
    private void UpdateButtonText()
    {
        buttonText.text = isSoundOn ? "SFX On" : "SFX Off";
    }

    private void UpdateButtonColor()
    {
        buttonImage.color = isSoundOn ? defaultColor : offColor; // Change button color based on music state
    }
}