using System.Collections;
using System.Collections.Generic;
using Pinwheel.Jupiter;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Master : MonoBehaviour
{
    public Slider timeSlider;

    public JDayNightCycle NightCycleProf;

    public TextMeshProUGUI timeTxt;

    public int PlaneHearts = 3;
    public GameObject Heart1;
    public GameObject Heart2;
    public GameObject Heart3;
    public PathGeneration PlaneMoveScript;
    public Plane PlaneScript;


    // Start is called before the first frame update
    void Start()
    {
        timeSlider.value = 10;
        
        // Display the initial time on the slider
        timeTxt.SetText("Time: " + timeSlider.value.ToString("0") + ":00 HRS");
    }

    // Update is called once per frame
    void Update()
    {
        // Ensure the night cycle uses the time from the slider
        NightCycleProf.Time = timeSlider.value;

        // Update the displayed time
        timeTxt.SetText("Time: " + timeSlider.value.ToString("0") + ":00 HRS");
    }
}
