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
    public LineRenderer lr;
    public bool isRunning;

    void Update()
    {
        lr.SetPosition(0, StartingPoint.transform.position);
        lr.SetPosition(1, EndPoint.transform.position);

        if (!Plane.pathPending && Plane.remainingDistance <= 0.1f && Plane.velocity.sqrMagnitude < 0.01f && isRunning)
        {
            StartingPoint.transform.position = EndPoint.transform.position;
            EndPoint.SetActive(false);
            isRunning = false;
        }
    }

    public void Run()
    {
        if (Points.Count == 0) return; 

        int random = Random.Range(0, Points.Count);
        EndPoint.transform.position = Points[random].transform.position;

        Plane.SetDestination(EndPoint.transform.position);
        isRunning = true;
        EndPoint.SetActive(true);
    }
}

