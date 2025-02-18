using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plane : MonoBehaviour
{
    public PathGeneration Path;
    public bool isRunning;
    public Master MasterScript;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Weather") && isRunning)
        {
            MasterScript.PlaneHearts--;
            if (MasterScript.PlaneHearts == 2)
                MasterScript.Heart1.SetActive(false);
            if (MasterScript.PlaneHearts == 1)
                MasterScript.Heart2.SetActive(false);
            if (MasterScript.PlaneHearts == 0)
            {
                MasterScript.Heart3.SetActive(false);
                Path.Plane.gameObject.SetActive(false);
                Path.StateScreen.SetActive(true);
                Path.LoseScreen.SetActive(true);
            }
        }
    }
}