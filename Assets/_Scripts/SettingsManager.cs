using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    public GameObject settingsPanel; // Reference to the settings panel

    public void ToggleSettings()
    {
        settingsPanel.SetActive(!settingsPanel.activeSelf); // Toggle the settings panel
    }
}
