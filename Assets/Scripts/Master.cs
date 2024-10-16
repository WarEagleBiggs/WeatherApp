using System.Collections;
using System.Collections.Generic;
using Pinwheel.Jupiter;
using UnityEngine;
using UnityEngine.UI;

public class Master : MonoBehaviour
{
    public Slider timeSlider;

    public JDayNightCycle NightCycleProf;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        NightCycleProf.Time = timeSlider.value * 24;
    }
}
