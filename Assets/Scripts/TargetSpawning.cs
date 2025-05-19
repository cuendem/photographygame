using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class SpawnablePrefab
{
    public GameObject prefab;
    public float spawnChance; // Chance to spawn this object
}

public class TargetSpawning : MonoBehaviour
{
    public List<SpawnablePrefab> spawnablePrefabs; // List of prefabs to spawn

    public float minTime = 0f; // Minimum time between spawns
    public float maxTime = 1f; // Maximum time between spawns

    private Vector2 xRange = new Vector2(0f, 0f);
    private Vector2 yRange = new Vector2(0f, 0f);

    private float nextSpawnTime = 0f;
    private float speedUpSpawnTimer = 0f;

    void Start()
    {
        // Set yRange dynamically based on screen size
        float screenHeight = Camera.main.orthographicSize * 2f;
        yRange = new Vector2(-screenHeight / 2f, screenHeight / 2f);

        // Set xRange dynamically based on screen width to ensure offscreen positions
        float screenWidth = Camera.main.orthographicSize * Camera.main.aspect * 2f;
        xRange = new Vector2(-screenWidth / 2f - 2f, screenWidth / 2f + 2f);

        ScheduleNextSpawn();
    }

    void Update()
    {
        if (GameManager.Instance.powerUpActive && GameManager.Instance.powerUp == "SpeedUp")
        {
            speedUpSpawnTimer += Time.deltaTime;
            if (speedUpSpawnTimer >= 0.1f)
            {
                SpawnThing();
                speedUpSpawnTimer = 0f;
            }
        }
        else
        {
            speedUpSpawnTimer = 0f; // Reset timer when not in SpeedUp
            if (Time.time >= nextSpawnTime)
            {
                SpawnThing();
                ScheduleNextSpawn();
            }
        }

        // Manual spawn (optional)
        // if (Input.GetKeyDown(KeyCode.Space))
        // {
        //     SpawnThing();
        // }
    }

    void ScheduleNextSpawn()
    {
        // Random delay between 0.5 and 5 seconds
        nextSpawnTime = Time.time + Random.Range(minTime, maxTime);
    }

    void SpawnThing()
    {
        GameObject selectedPrefab = GetRandomPrefab();
        if (selectedPrefab != null && !(GameManager.Instance.powerUpActive && GameManager.Instance.powerUp == "Flash"))
        {
            float randomY = Random.Range(yRange.x, yRange.y);
            float randomX = Random.value > 0.5f ? xRange.x : xRange.y;
            Vector3 spawnPosition = new Vector3(randomX, randomY, 0f);

            Instantiate(selectedPrefab, spawnPosition, Quaternion.identity);
        }
    }

    GameObject GetRandomPrefab()
    {
        float totalChance = 0f;
        foreach (var spawnable in spawnablePrefabs)
        {
            totalChance += spawnable.spawnChance;
        }

        float randomValue = Random.Range(0f, totalChance);
        float cumulativeChance = 0f;

        foreach (var spawnable in spawnablePrefabs)
        {
            cumulativeChance += spawnable.spawnChance;
            if (randomValue <= cumulativeChance)
            {
                return spawnable.prefab;
            }
        }

        return null;
    }
}
