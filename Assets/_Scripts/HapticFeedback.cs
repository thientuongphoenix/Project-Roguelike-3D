using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem; // Make sure to include this namespace

public class HapticFeedback : MonoBehaviour
{
    public Button button; // Assign your button in the Inspector
    public float leftMotorIntensity = 0.5f; // Intensity for left motor
    public float rightMotorIntensity = 0.5f; // Intensity for right motor
    public float duration = 0.1f; // Duration of haptic feedback

    void Start()
    {
        // Add a listener to the button to call the Haptic method when clicked
        button.onClick.AddListener(TriggerHapticFeedback);
    }

    private void TriggerHapticFeedback()
    {
        // Check if a gamepad is connected
        var gamepad = Gamepad.current;
        if (gamepad != null)
        {
            // Trigger haptic feedback
            gamepad.SetMotorSpeeds(leftMotorIntensity, rightMotorIntensity);
            Invoke("StopHapticFeedback", duration); // Stop after the specified duration
        }
        else
        {
            // For mobile devices
            Handheld.Vibrate();
        }
    }

    private void StopHapticFeedback()
    {
        var gamepad = Gamepad.current;
        if (gamepad != null)
        {
            gamepad.SetMotorSpeeds(0f, 0f); // Stop haptic feedback
        }
    }
}