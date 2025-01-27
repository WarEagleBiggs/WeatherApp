using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeatherSpawner : MonoBehaviour
{
    public GameObject cloudPrefab; // The prefab for the cloud
    public float spawnXMin = -55f; // Minimum X position for spawning clouds
    public float spawnXMax = 55f; // Maximum X position for spawning clouds
    public float spawnY = 0f; // Fixed Y position for all clouds
    public Vector2 spawnZRange = new Vector2(-60f, 60f); // Range for random Z positioning (depth)
    public Slider timeSlider; // Reference to the slider that controls time
    public Button playButton; // The play button that starts cloud movement
    public int cloudCount = 10; // How many clouds to spawn
    public float transitionSpeed = 2f; // Speed at which clouds move to their start positions
    public float cloudSpeedMultiplier = 1f; // Multiplier to control the speed of cloud movement

    private GameObject[] clouds; // Array to hold all spawned clouds
    private Vector3[] cloudStartPositions; // Store the final starting X positions of each cloud
    private bool isPlaying = false; // Flag to track whether the play button has been pressed
    private float timeProgress = 0f; // Tracks the progress of time (0 to 1)
    private float timeAtStart = 0f; // Tracks the time when the play button was pressed

    public PathGeneration PathGenerationScript; // Reference to the PathGeneration script

    private void Start()
    {
        // Ensure the cloudPrefab, slider, and playButton are assigned
        if (cloudPrefab == null || timeSlider == null || playButton == null)
        {
            Debug.LogError("Cloud prefab, slider, or play button not assigned.");
            return;
        }

        // Start by spawning all clouds
        SpawnClouds();

        // Initially, update cloud positions based on the slider value
        UpdateCloudPositions(timeSlider.value);

        // Add listener for when the slider value changes (for previewing)
        timeSlider.onValueChanged.AddListener(OnSliderValueChanged);

        // Add listener for the play button
        playButton.onClick.AddListener(OnPlayButtonPressed);

        // Initialize the slider bounds and set the initial value
        InitializeSlider();
    }

    private void SpawnClouds()
    {
        clouds = new GameObject[cloudCount];
        cloudStartPositions = new Vector3[cloudCount];

        for (int i = 0; i < cloudCount; i++)
        {
            // Randomize the Z position (depth)
            float spawnZ = Random.Range(spawnZRange.x, spawnZRange.y);

            // Set the spawn position with the specified X, fixed Y, and random Z
            float spawnX = Random.Range(spawnXMin, spawnXMax);
            Vector3 spawnPosition = new Vector3(spawnX, spawnY, spawnZ);

            // Generate a unique Y-axis rotation for each cloud for visual effect
            float randomYRotation = Random.Range(0f, 360f);
            Quaternion randomRotation = Quaternion.Euler(0f, randomYRotation, 0f); // Unique rotation for each cloud

            // Instantiate the cloud at the spawn position with the random rotation
            clouds[i] = Instantiate(cloudPrefab, spawnPosition, randomRotation);

            // Store the final starting X position of the cloud for later use
            cloudStartPositions[i] = new Vector3(Random.Range(spawnXMin, spawnXMax), spawnY, spawnZ);
        }
    }

    // This method is called when the slider value changes (for previewing)
    private void OnSliderValueChanged(float value)
    {
        // Update the cloud positions based on the slider value
        UpdateCloudPositions(value);
    }

    private void UpdateCloudPositions(float sliderValue)
    {
        // Calculate the cloud's movement based on the slider value (0 to 24 representing hours)
        float timeProgress = Mathf.Lerp(0f, 1f, sliderValue / 24f); // Normalize slider value to a 0-1 range for 24 hours
        float groupXMovement = Mathf.Lerp(spawnXMin, spawnXMax, timeProgress); // Move group across X range

        // Update each cloud's position
        for (int i = 0; i < cloudCount; i++)
        {
            if (clouds[i] != null)
            {
                Vector3 currentPosition = cloudStartPositions[i];

                // Move clouds in one direction (rightward as time progresses)
                clouds[i].transform.position = new Vector3(currentPosition.x + groupXMovement, currentPosition.y, currentPosition.z);
            }
        }
    }

    private void OnPlayButtonPressed()
    {
        // Enable the play mode to start moving clouds continuously
        isPlaying = true;

        // Record the start time when the play button was pressed
        timeAtStart = Time.time;

        // Immediately move clouds to their start positions without transition
        MoveCloudsToStartPosition();

        // Start the movement coroutine
        StartCoroutine(MoveCloudsOverTime());
    }

    private void MoveCloudsToStartPosition()
    {
        // Instantly position clouds at their start positions
        for (int i = 0; i < cloudCount; i++)
        {
            if (clouds[i] != null)
            {
                clouds[i].transform.position = cloudStartPositions[i];
            }
        }
    }

    private IEnumerator MoveCloudsOverTime()
    {
        // Continuously move clouds based on time
        while (isPlaying)
        {
            // Calculate how much time has passed since the play button was pressed
            float elapsedTime = (Time.time - timeAtStart) * cloudSpeedMultiplier;

            // Normalize the time progress (between 0 and 1 for 24 hours)
            timeProgress = Mathf.Clamp01(elapsedTime / 24f);

            // Update the cloud positions based on the normalized time
            UpdateCloudPositions(timeProgress * 24f);

            // Update the slider value to reflect the current time
            timeSlider.value = timeProgress * 24f;

            // Wait until the next frame
            yield return null;
        }
    }

    private void InitializeSlider()
    {
        // Initialize slider value based on the starting position of the flight
        float flightDistance = Vector3.Distance(PathGenerationScript.StartingPoint.transform.position, PathGenerationScript.EndPoint.transform.position);
        float travelTime = Mathf.Clamp(flightDistance / 10f, 4f, 24f); // Travel time ranges from 4 to 24 hours

        // Set slider range and starting position
        timeSlider.minValue = 0f;
        timeSlider.maxValue = travelTime;

        // Set initial value for the slider (start time is based on slider position)
        timeSlider.value = 0f;
    }
}
