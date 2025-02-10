using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGen : MonoBehaviour
{
    [Header("Prefab Reference")]
    // Assign your swingable prefab in the Inspector.
    public GameObject swingablePrefab;

    [Header("Spawn Settings")]
    // How far ahead of the camera to ensure objects are spawned.
    public float spawnDistanceAhead = 20f;
    // Minimum horizontal spacing between spawned objects.
    public float minSpawnSpacing = 5f;
    // Maximum horizontal spacing between spawned objects.
    public float maxSpawnSpacing = 10f;
    // Maximum vertical deviation from the camera's y position.
    public float verticalDeviation = 2f;

    // Reference to the main camera's transform.
    private Transform mainCamera;
    // The x-coordinate where the last prefab was spawned.
    private float lastSpawnX;

    void Start()
    {
        // Cache the main camera's transform.
        mainCamera = Camera.main.transform;
        
        // Initialize the spawn position to the camera's current x-position.
        lastSpawnX = mainCamera.position.x;
    }

    void Update()
    {
        // Get the current x position of the camera.
        float camX = mainCamera.position.x;

        // Continuously spawn objects as long as the last spawn is within the "ahead" region.
        while (lastSpawnX < camX + spawnDistanceAhead)
        {
            // Determine a random spacing between this object and the previous one.
            float spacing = Random.Range(minSpawnSpacing, maxSpawnSpacing);
            lastSpawnX += spacing;

            // Set the y position relative to the camera's y with some random deviation.
            float spawnY = mainCamera.position.y + Random.Range(-verticalDeviation, verticalDeviation);

            // Create the spawn position. (z is set to 0 for a 2D game.)
            Vector3 spawnPos = new Vector3(lastSpawnX, spawnY, 0f);

            // Instantiate the swingable prefab at the spawn position with no rotation.
            Instantiate(swingablePrefab, spawnPos, Quaternion.identity);
        }
    }
}
