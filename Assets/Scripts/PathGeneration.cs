using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class PathGeneration : MonoBehaviour
{
    public GameObject StartingPoint;
    public GameObject EndPoint;
    public NavMeshAgent Plane;
    public List<Transform> Points;
    public LineRenderer lr;
    public bool isRunning;

    public GameObject RunBtn;
    public GameObject ResetBtn;

    public TextMeshProUGUI StartLocationTxt;
    public TextMeshProUGUI EndLocationTxt;

    private void Start()
    {
        //init info
        int random = Random.Range(0, Points.Count);
        EndPoint.transform.position = Points[random].transform.position;
        EndPoint.SetActive(true);
        
        StartLocationTxt.SetText(Points[random].name);
        
        int random2 = Random.Range(0, Points.Count);
        if (random2 == random)
        {
            random2 = Random.Range(0, Points.Count);
        }
        StartingPoint.transform.position = Points[random2].transform.position;
        Plane.transform.position = StartingPoint.transform.position;
        Plane.transform.LookAt(EndPoint.transform);
        StartingPoint.SetActive(true);
        
        EndLocationTxt.SetText(Points[random2].name);
    }

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
        RunBtn.SetActive(false);
        ResetBtn.SetActive(true);
        if (Points.Count == 0) return; 

        
        Plane.SetDestination(EndPoint.transform.position);
        isRunning = true;
        
    }

    public void ResetBtnClick()
    {
        SceneManager.LoadScene("Game");
    }
}

