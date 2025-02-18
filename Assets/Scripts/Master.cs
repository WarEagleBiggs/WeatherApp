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

    void Start()
    {
        timeSlider.value = timeSlider.maxValue;
        timeTxt.SetText("Time: " + timeSlider.value.ToString("0") + ":00 HRS");
    }

    void Update()
    {
        NightCycleProf.Time = timeSlider.value;
        timeTxt.SetText("Time: " + timeSlider.value.ToString("0") + ":00 HRS");
    }
}