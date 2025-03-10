﻿using TMPro;
using UnityEngine;

public class FPS_Counter : MonoBehaviour
{
    public TextMeshProUGUI fpsText; // Text hiển thị FPS
    private float deltaTime = 0.0f;

    void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;
        fpsText.text = "FPS: " + Mathf.Ceil(fps);
    }
}
