using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Collections;

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance { get; private set; }
    [SerializeField] private List<Transform> spawnPoints = new List<Transform>();
    private List<Transform> activeSpawnPoints = new List<Transform>();
    private List<int> validSpawnPointIndices = new List<int>();
    [SerializeField] private List<GameObject> enemyPrefabs = new List<GameObject>();
    [SerializeField] private List<GameObject> activeEnemyPrefabs = new List<GameObject>();
    private GameObject enemyPrefab;
    private int enemyIndex = 0;
    private float timeBetweenWaves = 4f;
    private int enemiesPerWave = 3;
    private int enemiesLeftToSpawn = 3;
    private float spawnInterval = 2f;
    private float spawnIntervalReduction = 0.4f;
    private float minimumSpawnInterval = 0.1f;
    private float difficultyMultiplier = 1.3f;
    private float timeSinceLastSpawn = 0f;
    private int currentWave = 1;
    private int enemiesAlive = 0;
    private bool isSpawningWave = false;

    private void Awake()
    {
        // Ensure a single instance
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        validSpawnPointIndices = Enumerable.Range(0, spawnPoints.Count).ToList();
        StartCoroutine(StartFirstWave());
    }

    private void Update()
    {
        ManageWaves();
    }

    private IEnumerator StartFirstWave()
    {
        yield return new WaitForSeconds(timeBetweenWaves);

        enemyPrefab = enemyPrefabs[enemyIndex];
        isSpawningWave = true;
        UpdateActiveSpawnPoints();
    }

    private IEnumerator StartNewWave()
    {
        yield return new WaitForSeconds(timeBetweenWaves);

        enemyIndex = 0;
        enemyPrefab = enemyPrefabs[enemyIndex];
        currentWave++;
        isSpawningWave = true;
        enemiesPerWave = Mathf.CeilToInt(enemiesPerWave * Mathf.Pow(difficultyMultiplier, currentWave - 1));
        spawnInterval = Mathf.Max(minimumSpawnInterval, spawnInterval - spawnIntervalReduction);
        enemiesLeftToSpawn = enemiesPerWave;
        UpdateActiveSpawnPoints();
    }

    private void EndWave()
    {
        isSpawningWave = false;
        timeSinceLastSpawn = 0f;
        StartCoroutine(StartNewWave());
    }

    private void ManageWaves()
    {
        if (isSpawningWave)
        {
            timeSinceLastSpawn += Time.deltaTime;

            if (timeSinceLastSpawn >= spawnInterval && enemiesLeftToSpawn > 0)
            {
                timeSinceLastSpawn = 0f;
                SpawnEnemy();
            }
        }
        if (enemiesAlive <= 0 && enemiesLeftToSpawn <= 0 && isSpawningWave)
        {
            EndWave();
        }
    }

    private void SpawnEnemy()
    {
        if (activeEnemyPrefabs.Count > 1)
        {

        }

        Transform spawnPoint = activeSpawnPoints[Random.Range(0, activeSpawnPoints.Count)];
        Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
        enemiesAlive++;
        enemiesLeftToSpawn--;
    }

    private void UpdateActiveSpawnPoints()
    {
        if (currentWave > spawnPoints.Count)
        {
            return;
        }

        if (currentWave == 1 || currentWave == 2 || currentWave == 4 || currentWave == 7)
        {
            AddRandomSpawnPoint();
        }
    }

    private void ChooseEnemyToSpawn()
    {

    }

    private void AddNewEnemy()
    {

    }

    private void AddRandomSpawnPoint()
    {
        int indexAdded = validSpawnPointIndices[Random.Range(0, validSpawnPointIndices.Count)];
        activeSpawnPoints.Add(spawnPoints[indexAdded]);
        validSpawnPointIndices.Remove(indexAdded);
    }

    public void OnEnemyDefeated()
    {
        enemiesAlive--;
    }
}