using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeController : MonoBehaviour
{
    [SerializeField] Slider timeSlider; // our time slider
    [SerializeField] Text timeScale; // our time scale in text

    // update ui
    void UpdateUI()
    {
        timeScale.text = "TimeScale: " + Time.timeScale.ToString();
    }

    // update time
    void UpdateTime()
    {
        Time.timeScale = timeSlider.value;
    }


    private void FixedUpdate()
    {
        // update our UI
        UpdateUI();
        // update our ui
        UpdateTime();
    }
}
