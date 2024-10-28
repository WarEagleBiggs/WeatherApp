using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

    public Slider curveSlider;  

    private List<Vector3> waypoints;  
    private int currentWaypointIndex = 0;  

    private void Start()
    {
        // Initialize
        int random = Random.Range(0, Points.Count);
        EndPoint.transform.position = Points[random].transform.position;
        EndPoint.SetActive(true);
        StartLocationTxt.SetText(Points[random].name);

        int random2 = Random.Range(0, Points.Count);
        if (random2 == random) random2 = Random.Range(0, Points.Count);

        StartingPoint.transform.position = Points[random2].transform.position;
        Plane.transform.position = StartingPoint.transform.position;
        Plane.transform.LookAt(EndPoint.transform);
        StartingPoint.SetActive(true);
        EndLocationTxt.SetText(Points[random2].name);

        lr.positionCount = 20;  
        waypoints = new List<Vector3>();  
    }

    private void Update()
    {
       
        DrawCurvedLine();

        if (isRunning && Plane.remainingDistance < 0.1f && !Plane.pathPending)
        {
            
            MoveToNextWaypoint();
        }
    }

    public void Run()
    {
        RunBtn.SetActive(false);
        ResetBtn.SetActive(true);
        if (Points.Count == 0) return;

        isRunning = true;
        currentWaypointIndex = 0;  
        UpdateWaypoints(); 
        Plane.SetDestination(waypoints[currentWaypointIndex]);  
    }

    public void ResetBtnClick()
    {
        SceneManager.LoadScene("Game");
    }

    private void DrawCurvedLine()
    {
        Vector3 startPos = StartingPoint.transform.position;
        Vector3 endPos = EndPoint.transform.position;

        float curveAmount = curveSlider.value * 10;  
        Vector3 midPoint = (startPos + endPos) / 2;

        
        Vector3 direction = (endPos - startPos).normalized;
        Vector3 perpendicular = new Vector3(-direction.z, direction.y, direction.x);
        midPoint += perpendicular * curveAmount;

        for (int i = 0; i < lr.positionCount; i++)
        {
            float t = i / (float)(lr.positionCount - 1); 
            Vector3 point = QuadraticBezier(startPos, midPoint, endPos, t);
            lr.SetPosition(i, point);
        }
    }

    private Vector3 QuadraticBezier(Vector3 a, Vector3 b, Vector3 c, float t)
    {
        return (1 - t) * (1 - t) * a + 2 * (1 - t) * t * b + t * t * c;
    }

    private void UpdateWaypoints()
    {
        waypoints.Clear(); 

        for (int i = 0; i < lr.positionCount; i++)
        {
            waypoints.Add(lr.GetPosition(i));
        }
    }

    private void MoveToNextWaypoint()
    {
        if (currentWaypointIndex < waypoints.Count - 1)
        {
            currentWaypointIndex++;
            Plane.SetDestination(waypoints[currentWaypointIndex]);  
        }
        else
        {
            isRunning = false;
        }
    }
}