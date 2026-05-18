using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Collections;
using TMPro;

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance { get; private set; }
    [SerializeField] private List<Transform> spawnPoints = new List<Transform>();
    [SerializeField] private TextMeshProUGUI waveText;
    private List<Transform> activeSpawnPoints = new List<Transform>();
    private List<int> validSpawnPointIndices = new List<int>();
    [SerializeField] private List<GameObject> enemyPrefabs = new List<GameObject>();
    [SerializeField] private List<GameObject> activeEnemyPrefabs = new List<GameObject>();
    private float timeBetweenWaves = 4f;
    private int enemiesPerWave = 2;
    private int enemiesLeftToSpawn = 2;
    private float spawnInterval = 2f;
    private float spawnIntervalReduction = 0.2f;
    private float minimumSpawnInterval = 0.25f;
    private float timeSinceLastSpawn = 0f;
    private int currentWave = 0;
    private int maxWaves = 10;
    private int enemiesAlive = 0;
    private bool isSpawningWave = false;
    private int layerOrder = 0;

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
        currentWave++;
        waveText.text = currentWave.ToString() + " / " + maxWaves.ToString();
        yield return new WaitForSeconds(timeBetweenWaves);

        AddNewEnemy();
        isSpawningWave = true;
        UpdateActiveSpawnPoints();
    }

    private IEnumerator StartNewWave()
    {
        yield return new WaitForSeconds(timeBetweenWaves);
        int enemiesIncresePerWave = 3;

        currentWave++;
        AddNewEnemy();
        layerOrder = 0;
        isSpawningWave = true;
        enemiesPerWave = enemiesPerWave + (currentWave - 1) * enemiesIncresePerWave;
        spawnInterval = Mathf.Max(minimumSpawnInterval, spawnInterval - spawnIntervalReduction);
        enemiesLeftToSpawn = enemiesPerWave;
        UpdateActiveSpawnPoints();
        waveText.text = currentWave.ToString() + " / " + maxWaves.ToString();
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
        if (activeEnemyPrefabs.Count == 0)
        {
            return;
        }

        int spawnedSoFar = enemiesPerWave - enemiesLeftToSpawn;
        int segmentSize = Mathf.CeilToInt((float)enemiesPerWave / activeEnemyPrefabs.Count);
        int index = Mathf.Min(spawnedSoFar / segmentSize, activeEnemyPrefabs.Count - 1);

        GameObject enemyPrefab = activeEnemyPrefabs[index];
        Transform spawnPoint = activeSpawnPoints[Random.Range(0, activeSpawnPoints.Count)];
        GameObject enemyInstance = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
        SpriteRenderer spriteRenderer = enemyInstance.GetComponentInChildren<SpriteRenderer>();
        spriteRenderer.sortingOrder = layerOrder;
        layerOrder++;
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

    private void AddNewEnemy()
    {
        if ((currentWave % 3 == 0 || currentWave == 1) && activeEnemyPrefabs.Count < enemyPrefabs.Count)
        {
            activeEnemyPrefabs.Add(enemyPrefabs[activeEnemyPrefabs.Count]);
        }
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