using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PreviewPlaneMovement : MonoBehaviour
{
    public LineRenderer lr;               // Reference to the dynamic LineRenderer
    public Slider timeSlider;             // Slider that controls the preview time
    public Transform ghostPlane;          // The preview plane (child of the main plane)

    private List<Vector3> waypoints = new List<Vector3>();  // List to store path points

    private void Start()
    {
        // Initialize the waypoints based on the current LineRenderer path
        UpdateWaypoints();

        // Set the ghost plane to the starting position
        SetGhostPlaneToStartPosition();

        // Add listener to update ghost plane position as slider changes
        timeSlider.onValueChanged.AddListener(UpdateGhostPlanePosition);
    }

    private void Update()
    {
        // Ensure waypoints are refreshed if the LineRenderer path changes dynamically
        UpdateWaypoints();
    }

    private void UpdateWaypoints()
    {
        waypoints.Clear();

        // Populate waypoints from the LineRenderer's current positions
        for (int i = 0; i < lr.positionCount; i++)
        {
            waypoints.Add(lr.GetPosition(i));
        }
    }

    private void SetGhostPlaneToStartPosition()
    {
        if (waypoints.Count > 0)
        {
            // Set the ghost plane's position to the first point on the path
            ghostPlane.position = waypoints[0];
        }
    }

    private void UpdateGhostPlanePosition(float sliderValue)
    {
        if (waypoints.Count < 2) return;  // Ensure there are at least two waypoints

        // Calculate the progress percentage (0 to 1) based on the slider value
        float t = Mathf.InverseLerp(timeSlider.minValue, timeSlider.maxValue, sliderValue);

        // Get the interpolated position along the dynamic path
        Vector3 targetPosition = GetPositionAlongPath(t);

        // Move the ghost plane to the interpolated position
        ghostPlane.position = targetPosition;

        // Optionally, orient the ghost plane along the path direction
        OrientGhostPlaneAlongPath(t);
    }

    private Vector3 GetPositionAlongPath(float progress)
    {
        float totalDistance = GetTotalPathDistance();
        float distanceToTravel = totalDistance * progress;
        float traveled = 0f;

        // Iterate through waypoints to find the correct segment for interpolation
        for (int i = 0; i < waypoints.Count - 1; i++)
        {
            float segmentDistance = Vector3.Distance(waypoints[i], waypoints[i + 1]);

            if (traveled + segmentDistance >= distanceToTravel)
            {
                // Calculate the interpolation factor within the current segment
                float segmentProgress = (distanceToTravel - traveled) / segmentDistance;
                return Vector3.Lerp(waypoints[i], waypoints[i + 1], segmentProgress);
            }

            traveled += segmentDistance;
        }

        // If progress is complete, return the last waypoint
        return waypoints[waypoints.Count - 1];
    }

    private float GetTotalPathDistance()
    {
        float totalDistance = 0f;

        // Calculate the total distance along the path by summing segment distances
        for (int i = 0; i < waypoints.Count - 1; i++)
        {
            totalDistance += Vector3.Distance(waypoints[i], waypoints[i + 1]);
        }
        return totalDistance;
    }

    private void OrientGhostPlaneAlongPath(float progress)
    {
        float totalDistance = GetTotalPathDistance();
        float distanceToTravel = totalDistance * progress;
        float traveled = 0f;

        // Find the correct segment to orient the plane
        for (int i = 0; i < waypoints.Count - 1; i++)
        {
            float segmentDistance = Vector3.Distance(waypoints[i], waypoints[i + 1]);

            if (traveled + segmentDistance >= distanceToTravel)
            {
                // Calculate the direction and set the ghost plane's rotation
                Vector3 direction = (waypoints[i + 1] - waypoints[i]).normalized;
                ghostPlane.rotation = Quaternion.LookRotation(direction);
                return;
            }

            traveled += segmentDistance;
        }
    }
}
