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
    
    // Start is called before the first frame update
    void Start()
    {
        timeSlider.value = 10;
    }

    // Update is called once per frame
    void Update()
    {
        NightCycleProf.Time = timeSlider.value;
        timeTxt.SetText("Time: " + NightCycleProf.Time.ToString("0") + ":00 HRS");
    }
}
