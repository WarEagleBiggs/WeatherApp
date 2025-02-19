// WeatherSpawner.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeatherSpawner : MonoBehaviour
{
    public List<GameObject> cloudTypes;
    public float spawnXMin = -55f;
    public float spawnXMax = 55f;
    public float spawnY = 0f;
    public Vector2 spawnZRange = new Vector2(-60f, 60f);
    public Slider timeSlider;
    public Button playButton;
    public int cloudCount = 10;
    public float transitionSpeed = 2f;
    public float cloudSpeedMultiplier = 1f;
    public float simulationSpeed = 5f;
    private GameObject[] clouds;
    private Vector3[] cloudStartPositions;
    private bool isPlaying = false;
    private float timeProgress = 0f;
    private float timeAtStart = 0f;
    public PathGeneration PathGenerationScript;
    public float startTime = 7f;

    private void Start()
    {
        if (cloudTypes == null || cloudTypes.Count == 0 || timeSlider == null || playButton == null)
            return;
        SpawnClouds();
        UpdateCloudPositions(startTime);
        timeSlider.onValueChanged.AddListener(OnSliderValueChanged);
        playButton.onClick.AddListener(OnPlayButtonPressed);
        InitializeSlider();
    }

    private void SpawnClouds()
    {
        clouds = new GameObject[cloudCount];
        cloudStartPositions = new Vector3[cloudCount];
        for (int i = 0; i < cloudCount; i++)
        {
            int index = Random.Range(0, cloudTypes.Count);
            GameObject selectedCloud = cloudTypes[index];
            float spawnZ = Random.Range(spawnZRange.x, spawnZRange.y);
            float spawnX = Random.Range(spawnXMin, spawnXMax);
            Vector3 spawnPosition = new Vector3(spawnX, spawnY, spawnZ);
            float randomYRotation = Random.Range(0f, 360f);
            Quaternion randomRotation = Quaternion.Euler(0f, randomYRotation, 0f);
            GameObject cloudInstance = Instantiate(selectedCloud, spawnPosition, randomRotation);
            cloudInstance.SetActive(true);
            clouds[i] = cloudInstance;
            cloudStartPositions[i] = new Vector3(Random.Range(spawnXMin, spawnXMax), spawnY, spawnZ);
        }
    }

    private void OnSliderValueChanged(float value)
    {
        UpdateCloudPositions(value);
    }

    private void UpdateCloudPositions(float sliderValue)
    {
        float timeProgress = (sliderValue - timeSlider.minValue) / (timeSlider.maxValue - timeSlider.minValue);
        float groupXMovement = Mathf.Lerp(spawnXMin, spawnXMax, timeProgress);
        for (int i = 0; i < cloudCount; i++)
        {
            if (clouds[i] != null)
            {
                Vector3 currentPosition = cloudStartPositions[i];
                clouds[i].transform.position = new Vector3(currentPosition.x + groupXMovement, currentPosition.y, currentPosition.z);
            }
        }
    }

    private void OnPlayButtonPressed()
    {
        isPlaying = true;
        timeAtStart = Time.time;
        MoveCloudsToStartPosition();
        StartCoroutine(MoveCloudsOverTime());
    }

    private void MoveCloudsToStartPosition()
    {
        for (int i = 0; i < cloudCount; i++)
        {
            if (clouds[i] != null)
                clouds[i].transform.position = cloudStartPositions[i];
        }
    }

    private IEnumerator MoveCloudsOverTime()
    {
        while (isPlaying)
        {
            float elapsedTime = (Time.time - timeAtStart) * cloudSpeedMultiplier;
            timeProgress = Mathf.Clamp01(elapsedTime / ((timeSlider.maxValue - timeSlider.minValue) * simulationSpeed));
            float newTime = timeSlider.minValue + timeProgress * (timeSlider.maxValue - timeSlider.minValue);
            UpdateCloudPositions(newTime);
            timeSlider.value = newTime;
            yield return null;
        }
    }

    private void InitializeSlider()
    {
        float flightDistance = Vector3.Distance(PathGenerationScript.StartingPoint.transform.position, PathGenerationScript.EndPoint.transform.position);
        float travelTime = flightDistance / 10f;
        float finalTime = startTime + travelTime;
        if(finalTime > 23f)
            finalTime = 23f;
        timeSlider.minValue = startTime;
        timeSlider.maxValue = finalTime;
        timeSlider.value = startTime;
    }
}
