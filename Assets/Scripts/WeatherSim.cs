using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherSim : MonoBehaviour
{
    public Master MasterScript;

    public Transform StartPoint;

    public Transform EndPoint;
    
    [Range(0f, 1f)] // Slider in the inspector for convenience
    public float interpolationValue = 0f; // 0 = StartPoint, 1 = EndPoint

    // Update is called once per frame
    void Update()
    {
        interpolationValue = MasterScript.timeSlider.value;
        MoveObject(interpolationValue);
    }

    void MoveObject(float t)
    {
        // Interpolates the position between StartPoint and EndPoint based on the value of t (0 to 1)
        transform.position = Vector3.Lerp(StartPoint.position, EndPoint.position, t);
    }
}
