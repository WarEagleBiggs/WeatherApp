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
        // Random start and end points on the NavMesh
        startPosition = GetRandomNavMeshPoint();
        endPosition = GetRandomNavMeshPointFarFrom(startPosition);
        
        // Teleport agent to the starting point
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
        } while (Vector3.Distance(start, point) < 30f);  // Ensure it's far away

        return point;
    }

    void UpdateSliderPreview(float value)
    {
        if (isSimulating) return; // Avoid updating during the simulation

        // Interpolate between start and end based on the slider value
        float t = Mathf.InverseLerp(sliderMin, sliderMax, value);
        Vector3 previewPosition = Vector3.Lerp(startPosition, endPosition, t);

        // Warp agent to the preview position (for visualization only)
        agent.Warp(previewPosition);
    }

    void OnRunClicked()
    {
        // Start the simulation and reset the slider to 10
        isSimulating = true;
        timeSlider.value = sliderMin;
        
        // Move the agent to the destination
        agent.SetDestination(endPosition);
    }
}
