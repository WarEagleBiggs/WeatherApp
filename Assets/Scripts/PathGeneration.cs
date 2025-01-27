using System;
using System.Collections.Generic;
using Pinwheel.Jupiter;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

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
    public Slider timeSlider;

    private List<Vector3> waypoints;
    private int currentWaypointIndex;

    public GameObject StateScreen;
    public GameObject WinScreen;
    public GameObject LoseScreen;

    public Plane planeScript;
    public GameObject planeGhost;

    private const float MinDistance = 20f;

    private float planeSpeed = 2f; // Plane speed (units per hour)
    private float distance; // Distance between start and end point
    private float timeRequired; // Required time based on distance

    public JDayNightCycle NightCycleProf; // Reference to the day-night cycle script

    private void Start()
    {
        InitializePoints();
        lr.positionCount = 20;
        waypoints = new List<Vector3>();

        // Calculate the distance between the start and end points
        distance = Vector3.Distance(StartingPoint.transform.position, EndPoint.transform.position);

        // Calculate the time required based on the distance and speed
        timeRequired = distance / planeSpeed; // Time in hours

        // Set the slider's max value to the required time
        timeSlider.maxValue = timeRequired;

        // Set the slider's minimum value to 0 and the initial value to the calculated required time
        timeSlider.minValue = 0f;
        timeSlider.value = timeRequired;

        // Initialize the time slider
        timeSlider.onValueChanged.AddListener(OnTimeSliderValueChanged);
    }

    private void Update()
    {
        DrawCurvedLine();

        if (isRunning && Plane.remainingDistance < 0.1f && !Plane.pathPending)
        {
            MoveToNextWaypoint();
        }

        // Update the night cycle time to match the slider's value
        NightCycleProf.Time = timeSlider.value;
    }

    public AudioSource StartSFX;
    public void Run()
    {
        StartSFX.Play();
        planeGhost.SetActive(false);
        planeScript.isRunning = true;

        curveSlider.interactable = false;
        timeSlider.interactable = false;

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

    private void InitializePoints()
    {
        int startIndex, endIndex;

        // Ensure EndPoint and StartingPoint are not within MinDistance of each other
        do
        {
            startIndex = Random.Range(0, Points.Count);
            endIndex = Random.Range(0, Points.Count);
        } while (startIndex == endIndex || Vector3.Distance(Points[startIndex].position, Points[endIndex].position) < MinDistance);

        EndPoint.transform.position = Points[startIndex].position;
        EndPoint.SetActive(true);
        StartLocationTxt.SetText(Points[startIndex].name);

        StartingPoint.transform.position = Points[endIndex].position;
        Plane.transform.position = StartingPoint.transform.position;
        Plane.transform.LookAt(EndPoint.transform);
        StartingPoint.SetActive(true);
        EndLocationTxt.SetText(Points[endIndex].name);
    }

    private void DrawCurvedLine()
    {
        Vector3 startPos = StartingPoint.transform.position;
        Vector3 endPos = EndPoint.transform.position;

        float curveAmount = curveSlider.value * 10;
        Vector3 midPoint = (startPos + endPos) / 2;

        // Calculate perpendicular direction for the curve
        Vector3 direction = (endPos - startPos).normalized;
        Vector3 perpendicular = new Vector3(-direction.z, direction.y, direction.x);
        midPoint += perpendicular * curveAmount;

        // Generate positions for the LineRenderer
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

    public AudioSource winSFX;
    public AudioSource LandSfx;
    private void MoveToNextWaypoint()
    {
        if (currentWaypointIndex < waypoints.Count - 1)
        {
            currentWaypointIndex++;
            Plane.SetDestination(waypoints[currentWaypointIndex]);
        }
        else
        {
            LandSfx.Play();
            winSFX.Play();
            planeScript.isRunning = false;
            isRunning = false;
            StateScreen.SetActive(true);
            WinScreen.SetActive(true);
        }
    }

    // This method will be triggered when the time slider value changes
    private void OnTimeSliderValueChanged(float value)
    {
        // Ensure the daylight cycle reflects the current value of the slider
        NightCycleProf.Time = value;
    }
}
