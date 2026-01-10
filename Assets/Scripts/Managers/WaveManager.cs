using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private List<Transform> spawnPoints = new List<Transform>();
    [SerializeField] private GameObject zombiePrefab;
    private float timeBetweenWaves = 5f;
    private int initialZombiesPerWave = 5;
    private float initialSpawnInterval = 5f;
    private float spawnIntervalReduction = 1.5f;
    private float minimumSpawnInterval = 0.2f;
    private float currentSpawnInterval;
    private float difficultyMultiplier = 1.2f;
    private int currentWave = 0;
    private bool spawningWave = false;

    private void Start()
    {
        currentSpawnInterval = initialSpawnInterval;
        StartCoroutine(ManageWaves());
    }
    private IEnumerator ManageWaves()
    {
        while (true)
        {
            if (currentWave == 0)
            {
                yield return new WaitForSeconds(timeBetweenWaves);
            }

            // Start a new wave
            currentWave++;
            spawningWave = true;

            int zombiesToSpawn = Mathf.CeilToInt(initialZombiesPerWave * Mathf.Pow(difficultyMultiplier, currentWave - 1));
            yield return StartCoroutine(SpawnWave(zombiesToSpawn));

            spawningWave = false;
            currentSpawnInterval = Mathf.Max(currentSpawnInterval - spawnIntervalReduction, minimumSpawnInterval);

            // Wait for the next wave
            yield return new WaitForSeconds(timeBetweenWaves);
        }
    }

    private IEnumerator SpawnWave(int numberOfZombies)
    {
        for (int i = 0; i < numberOfZombies; i++)
        {
            SpawnZombie();
            yield return new WaitForSeconds(currentSpawnInterval);
        }
    }

    private void SpawnZombie()
    {
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
        Instantiate(zombiePrefab, spawnPoint.position, Quaternion.identity);
    }

    public int GetCurrentWave()
    {
        return currentWave;
    }

    public bool IsSpawningWave()
    {
        return spawningWave;
    }
}
