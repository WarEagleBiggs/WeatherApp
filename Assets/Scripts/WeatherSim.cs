using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class WeatherSim : MonoBehaviour
{
    public NavMeshAgent agent;      // Reference to the NavMeshAgent
    public Slider durationSlider;   // Reference to the duration slider (0 to 1 for progress)
    public Button runButton;        // Reference to the Run button

    private Vector3[] pathCorners;  // Stores the corners of the path
    private bool pathCalculated = false; // Check if the path is ready

    private void Start()
    {
        if (agent == null) agent = GetComponent<NavMeshAgent>();

        // Add listeners to UI elements
        durationSlider.onValueChanged.AddListener(OnSliderValueChanged);
        runButton.onClick.AddListener(StartSimulation);

        // Calculate the path initially
        CalculateRandomPath();
    }

    // Called when the slider value changes to update the agent's position along the path
    private void OnSliderValueChanged(float value)
    {
        if (pathCalculated && pathCorners.Length > 1)
        {
            MoveAgentAlongPath(value);
        }
    }

    // Move the agent smoothly along the path based on slider value (0 to 1)
    private void MoveAgentAlongPath(float t)
    {
        float totalDistance = 0;
        float targetDistance = t * GetPathLength();

        for (int i = 0; i < pathCorners.Length - 1; i++)
        {
            float segmentDistance = Vector3.Distance(pathCorners[i], pathCorners[i + 1]);
            if (totalDistance + segmentDistance >= targetDistance)
            {
                float segmentT = (targetDistance - totalDistance) / segmentDistance;
                Vector3 position = Vector3.Lerp(pathCorners[i], pathCorners[i + 1], segmentT);
                agent.transform.position = position; // Move the agent visually
                return;
            }
            totalDistance += segmentDistance;
        }
    }

    // Start the simulation by setting the agent's destination to the final position
    private void StartSimulation()
    {
        if (pathCalculated && pathCorners.Length > 1)
        {
            agent.SetDestination(pathCorners[pathCorners.Length - 1]);
        }
    }

    // Calculate a random path on the NavMesh and store its corners
    private void CalculateRandomPath()
    {
        Vector3 randomPosition = GetRandomNavMeshPosition(30f); // Adjust radius as needed

        NavMeshPath path = new NavMeshPath();
        if (agent.CalculatePath(randomPosition, path))
        {
            pathCorners = path.corners;
            pathCalculated = true;
        }
    }

    // Generate a random valid NavMesh position within the given radius
    private Vector3 GetRandomNavMeshPosition(float radius)
    {
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += transform.position;

        if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, radius, NavMesh.AllAreas))
        {
            return hit.position;
        }
        return Vector3.zero;
    }

    // Draw the path in the Scene view for debugging
    private void OnDrawGizmos()
    {
        if (pathCalculated && pathCorners != null)
        {
            Gizmos.color = Color.cyan;
            for (int i = 0; i < pathCorners.Length - 1; i++)
            {
                Gizmos.DrawLine(pathCorners[i], pathCorners[i + 1]);
            }
        }
    }

    // Calculate the total length of the path
    private float GetPathLength()
    {
        float length = 0;
        for (int i = 0; i < pathCorners.Length - 1; i++)
        {
            length += Vector3.Distance(pathCorners[i], pathCorners[i + 1]);
        }
        return length;
    }
}
