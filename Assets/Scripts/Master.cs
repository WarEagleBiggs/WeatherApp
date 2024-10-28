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
        
    }

    // Update is called once per frame
    void Update()
    {
        NightCycleProf.Time = timeSlider.value * 24;
        timeTxt.SetText("Time: " + NightCycleProf.Time.ToString("0") + ":00 HRS");
    }
}
