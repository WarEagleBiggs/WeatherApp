using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plane : MonoBehaviour
{
    public PathGeneration Path;
    public bool isRunning;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Weather" && isRunning)
        {
            Debug.Log("Weather Hit");
            Path.Plane.gameObject.SetActive(false);
            Path.StateScreen.SetActive(true);
            Path.LoseScreen.SetActive(true);
        }
    }
}
