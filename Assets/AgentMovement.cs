using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class AgentMovement : MonoBehaviour
{
    public NavMeshAgent agent;   // Reference to the NavMeshAgent
    public Slider timeSlider;    // Reference to the UI Slider
    public Button runButton;     // Reference to the Run Button
    public float sliderMin = 10f, sliderMax = 24f;  // Slider range

    private Vector3 startPosition; // Start point on the NavMesh
    private Vector3 endPosition;   // End point on the NavMesh
    private float journeyDistance; // Total distance between start and end points
    private bool isSimulating = false;

    void Start()
    {
        // Set the slider’s min and max values
        timeSlider.minValue = sliderMin;
        timeSlider.maxValue = sliderMax;

        // Attach the OnRunClicked function to the button click
        runButton.onClick.AddListener(OnRunClicked);

        // Pick random start and end positions
        PickRandomPositions();
        UpdateSliderPreview(timeSlider.value);

        // Add listener to update preview when slider value changes
        timeSlider.onValueChanged.AddListener(UpdateSliderPreview);
    }

    void PickRandomPositions()
    {
        startPosition = GetRandomNavMeshPoint();
        endPosition = GetRandomNavMeshPointFarFrom(startPosition);
        journeyDistance = Vector3.Distance(startPosition, endPosition);

        agent.Warp(startPosition);
    }

    Vector3 GetRandomNavMeshPoint()
    {
        Vector3 randomPoint = Random.insideUnitSphere * 50f;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 50f, NavMesh.AllAreas))
        {
            return hit.position;
        }
        return Vector3.zero;
    }

    Vector3 GetRandomNavMeshPointFarFrom(Vector3 start)
    {
        Vector3 point;
        do
        {
            point = GetRandomNavMeshPoint();
        } while (Vector3.Distance(start, point) < 30f);

        return point;
    }

    void UpdateSliderPreview(float value)
    {
        if (isSimulating) return; // Avoid updating during the simulation

        float t = Mathf.InverseLerp(sliderMin, sliderMax, value);
        Vector3 previewPosition = Vector3.Lerp(startPosition, endPosition, t);

        agent.Warp(previewPosition);
    }

    void OnRunClicked()
    {
        isSimulating = true;
        timeSlider.value = sliderMin;
        agent.SetDestination(endPosition);
    }

    void Update()
    {
        if (isSimulating && !agent.pathPending)
        {
            if (agent.remainingDistance > 0)
            {
                // Calculate the progress based on how far the agent has traveled
                float distanceTraveled = journeyDistance - agent.remainingDistance;
                float progress = distanceTraveled / journeyDistance;

                // Update the slider value based on the agent’s progress
                timeSlider.value = Mathf.Lerp(sliderMin, sliderMax, progress);
            }
            else
            {
                // Stop the simulation when the agent reaches the destination
                isSimulating = false;
            }
        }
    }
}
