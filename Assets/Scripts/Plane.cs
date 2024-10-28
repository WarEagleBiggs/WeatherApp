using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plane : MonoBehaviour
{
    public PathGeneration Path;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Weather")
        {
            Debug.Log("Weather Hit");
            Path.Plane.gameObject.SetActive(false);
            Path.StateScreen.SetActive(true);
            Path.LoseScreen.SetActive(true);
        }
    }
}
