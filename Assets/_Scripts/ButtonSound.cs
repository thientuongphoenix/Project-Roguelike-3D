using UnityEngine;
using UnityEngine.UI;

public class ButtonSound : MonoBehaviour
{
    public AudioClip soundEffect; // Reference to the sound effect for this button
    private AudioSource audioSource; // Reference to the Audio Source

    void Start()
    {
        // Get the Audio Source component
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = soundEffect; // Assign the sound effect to the Audio Source

        // Get the Button component and add a listener for the click event
        Button button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(PlaySound);
        }
    }

    void PlaySound()
    {
        // Play the sound effect
        if (audioSource != null && soundEffect != null)
        {
            audioSource.PlayOneShot(soundEffect); // Play the sound effect
        }
    }
}