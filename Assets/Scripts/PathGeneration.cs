using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.AI;

public class PathGeneration : MonoBehaviour
{
    public GameObject StartingPoint;
    public GameObject EndPoint;
    public NavMeshAgent Plane;

    public List<Transform> Points;

    public bool doReset = true;

    public LineRenderer lr;
    
    

    void Update()
    {
        lr.SetPosition(0, StartingPoint.transform.position);
        lr.SetPosition(1, EndPoint.transform.position);

        if (doReset)
        {
            doReset = false;
            int random = Random.Range(0, 51);
            EndPoint.transform.position = Points[random].transform.position;
            Plane.SetDestination(EndPoint.transform.position);
        }
    }

    public void Run()
    {
        doReset = true;
    }
    
    
    
}
