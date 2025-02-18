using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PreviewPlaneMovement : MonoBehaviour
{
    public LineRenderer lr;
    public Slider timeSlider;
    public Transform ghostPlane;
    private List<Vector3> waypoints = new List<Vector3>();

    private void Start()
    {
        //UpdateWaypoints();
        SetGhostPlaneToStartPosition();
        timeSlider.onValueChanged.AddListener(UpdateGhostPlanePosition);
    }

    private void Update()
    {
        UpdateWaypoints();
    }

    private void UpdateWaypoints()
    {
        waypoints.Clear();
        for (int i = 0; i < lr.positionCount; i++)
            waypoints.Add(lr.GetPosition(i));
    }

    public GameObject nullPos;
    private void SetGhostPlaneToStartPosition()
    {
        if (waypoints.Count > 0)
        {
            ghostPlane.transform.localPosition = nullPos.transform.localPosition;
            //ghostPlane.position = waypoints[0];
        }

    }

    private void UpdateGhostPlanePosition(float sliderValue)
    {
        if (waypoints.Count < 2)
            return;
        float t = Mathf.InverseLerp(timeSlider.minValue, timeSlider.maxValue, sliderValue);
        Vector3 targetPosition = GetPositionAlongPath(t);
        ghostPlane.position = targetPosition;
        OrientGhostPlaneAlongPath(t);
    }

    private Vector3 GetPositionAlongPath(float progress)
    {
        float totalDistance = GetTotalPathDistance();
        float distanceToTravel = totalDistance * progress;
        float traveled = 0f;
        for (int i = 0; i < waypoints.Count - 1; i++)
        {
            float segmentDistance = Vector3.Distance(waypoints[i], waypoints[i + 1]);
            if (traveled + segmentDistance >= distanceToTravel)
            {
                float segmentProgress = (distanceToTravel - traveled) / segmentDistance;
                return Vector3.Lerp(waypoints[i], waypoints[i + 1], segmentProgress);
            }
            traveled += segmentDistance;
        }
        return waypoints[waypoints.Count - 1];
    }

    private float GetTotalPathDistance()
    {
        float totalDistance = 0f;
        for (int i = 0; i < waypoints.Count - 1; i++)
            totalDistance += Vector3.Distance(waypoints[i], waypoints[i + 1]);
        return totalDistance;
    }

    private void OrientGhostPlaneAlongPath(float progress)
    {
        float totalDistance = GetTotalPathDistance();
        float distanceToTravel = totalDistance * progress;
        float traveled = 0f;
        for (int i = 0; i < waypoints.Count - 1; i++)
        {
            float segmentDistance = Vector3.Distance(waypoints[i], waypoints[i + 1]);
            if (traveled + segmentDistance >= distanceToTravel)
            {
                Vector3 direction = (waypoints[i + 1] - waypoints[i]).normalized;
                ghostPlane.rotation = Quaternion.LookRotation(direction);
                return;
            }
            traveled += segmentDistance;
        }
    }
}
