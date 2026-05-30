using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance { get; private set; }
    [SerializeField] private List<Transform> spawnPoints = new List<Transform>();
    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private GameObject EnenmyEntranceAnimationPrefab;
    private List<Transform> activeSpawnPoints = new List<Transform>();
    private List<int> validSpawnPointIndices = new List<int>();
    [SerializeField] private List<GameObject> enemyPrefabs = new List<GameObject>();
    [SerializeField] private List<GameObject> activeEnemyPrefabs = new List<GameObject>();
    [SerializeField] private List<GameObject> entranceAnimations = new List<GameObject>();
    private List<int> spawnQueue = new List<int>();
    private List<int> laneQueue = new List<int>();
    //private float timeBetweenWaves = 4f;
    private int enemiesPerWave = 3;
    private int enemiesLeftToSpawn = 3;
    private float spawnInterval = 2f;
    private float spawnIntervalReduction = 0.2f;
    private float minimumSpawnInterval = 0.25f;
    private float timeSinceLastSpawn = 0f;
    private int currentWave = 0;
    private int maxWaves = 10;
    private int enemiesAlive = 0;
    private bool isSpawningWave = false;
    private int layerOrder = 0;
    private int maxLanes = 4;
    private bool firstFourLaneWave = false;

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
        SetupFirstWave();
        //StartCoroutine(StartFirstWave());
    }

    private void Update()
    {
        ManageWave();
    }

    private void SetupFirstWave()
    {
        currentWave++;
        waveText.text = currentWave.ToString() + " / " + maxWaves.ToString();
        UpdateActiveSpawnPoints();
        SpawnEntranceAnimation();
    }

    private void SpawnEntranceAnimation()
    {
        foreach (Transform spawnPoint in activeSpawnPoints)
        {
            Vector3 spawnPosition = new Vector3(spawnPoint.position.x - 1.5f, spawnPoint.position.y, spawnPoint.position.z);
            GameObject entranceAnimation = Instantiate(EnenmyEntranceAnimationPrefab, spawnPosition, Quaternion.identity);
            entranceAnimations.Add(entranceAnimation);
        }
    }

    private void DestroyEntranceAnimation()
    {
        foreach (GameObject entranceAnimation in entranceAnimations)
        {
            Destroy(entranceAnimation);
        }
    }

    public void StartFirstWave()
    {
        // yield return new WaitForSeconds(timeBetweenWaves);

        AddNewEnemy();
        BuildSpawnQueue();
        BuildLaneQueue();
        isSpawningWave = true;
        DestroyEntranceAnimation();
    }

    public void StartNewWave()
    {
        //yield return new WaitForSeconds(timeBetweenWaves);
        int enemiesIncresePerWave = 2 * (currentWave - 1);

        AddNewEnemy();
        layerOrder = 0;
        isSpawningWave = true;
        enemiesPerWave += enemiesIncresePerWave;
        spawnInterval = Mathf.Max(minimumSpawnInterval, spawnInterval - spawnIntervalReduction);
        enemiesLeftToSpawn = enemiesPerWave;
        BuildSpawnQueue();
        BuildLaneQueue();
        DestroyEntranceAnimation();
    }

    private void EndWave()
    {
        isSpawningWave = false;
        timeSinceLastSpawn = 0f;
        currentWave++;
        UpdateActiveSpawnPoints();
        waveText.text = currentWave.ToString() + " / " + maxWaves.ToString();

        if (activeSpawnPoints.Count < maxLanes || firstFourLaneWave == false)
        {
            SpawnEntranceAnimation();
            if (firstFourLaneWave == false && activeSpawnPoints.Count == maxLanes)
            {
                firstFourLaneWave = true;
            }
        }
        //StartCoroutine(StartNewWave());
    }

    private void ManageWave()
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

        int index = ChooseEnemyIndexToSpawn();
        int laneIndex = laneQueue[0];
        if (laneQueue.Count > 0)
        {
            laneQueue.RemoveAt(0);
        }

        GameObject enemyPrefab = activeEnemyPrefabs[index];
        Transform spawnPoint = activeSpawnPoints[laneIndex];
        GameObject enemyInstance = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
        SpriteRenderer spriteRenderer = enemyInstance.GetComponentInChildren<SpriteRenderer>();
        spriteRenderer.sortingOrder = layerOrder;
        layerOrder++;
        enemiesAlive++;
        enemiesLeftToSpawn--;
    }

    private void BuildLaneQueue()
    {
        laneQueue.Clear();

        List<int> guaranteedLanes = Enumerable.Range(0, activeSpawnPoints.Count).ToList();

        for (int i = guaranteedLanes.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (guaranteedLanes[i], guaranteedLanes[j]) = (guaranteedLanes[j], guaranteedLanes[i]);
        }

        laneQueue.AddRange(guaranteedLanes);

        // Fill remaining spawns randomly
        int remaining = enemiesPerWave - activeSpawnPoints.Count;
        for (int i = 0; i < remaining; i++)
        {
            laneQueue.Add(Random.Range(0, activeSpawnPoints.Count));
        }
    }

    private int ChooseEnemyIndexToSpawn()
    {
        if (spawnQueue.Count == 0)
        {
            return 0;
        }

        int nextIndex = spawnQueue[0];
        spawnQueue.RemoveAt(0);
        return nextIndex;
    }

    private void BuildSpawnQueue()
    {
        spawnQueue.Clear();
        int totalEnemies = enemiesPerWave;
        int enemyTypeCount = activeEnemyPrefabs.Count;

        if (enemyTypeCount <= 1)
        {
            for (int i = 0; i < totalEnemies; i++)
            {
                spawnQueue.Add(0);
            }
            /*
            int segmentSize = Mathf.CeilToInt((float)totalEnemies / enemyTypeCount);
            for (int i = 0; i < totalEnemies; i++)
            {
                int index = Mathf.Min(i / segmentSize, enemyTypeCount - 1);
                spawnQueue.Add(index);
            }
            return;
            */
        }

        // Wave with multiple enemeis logic with 3 phases
        // Phase 1: 2-3 easy enemies at the start
        int openingCount = Random.Range(2, 4);
        for (int i = 0; i < openingCount; i++)
        {
            spawnQueue.Add(0);
        }

        int remaining = totalEnemies - openingCount;

        // Phase 2: middle section - mix of strong enemies with some weak ones sprinkled in
        int middleCount = Mathf.RoundToInt(remaining * 0.5f);
        List<int> middleEnemies = new List<int>();
        for (int i = 0; i < middleCount; i++)
        {
            // 65% chance of a stronger enemy, 35% chance of a weak one
            if (Random.value < 0.65f)
            {
                middleEnemies.Add(Random.Range(1, enemyTypeCount));
            }
            else
            {
                middleEnemies.Add(0);
            }
        }
        // Shuffle the middle section so it's not perfectly sorted
        for (int i = middleEnemies.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (middleEnemies[i], middleEnemies[j]) = (middleEnemies[j], middleEnemies[i]);
        }
        spawnQueue.AddRange(middleEnemies);

        remaining -= middleCount;

        // Phase 3: weak enemies first, then strong, then mixed ending
        int endStrongCount = Mathf.RoundToInt(remaining * 0.55f);
        int endWeakCount = remaining - endStrongCount;
        int mixedEndCount = Mathf.Max(1, Mathf.RoundToInt(remaining * 0.2f));

        // Weak enemies first
        int weakFirst = endWeakCount - Mathf.RoundToInt(endWeakCount * 0.3f);
        for (int i = 0; i < weakFirst; i++)
        {
            spawnQueue.Add(0);
        }

        // Strong enemies in the middle of phase 3
        int strongSection = endStrongCount - mixedEndCount;
        for (int i = 0; i < strongSection; i++)
        {
            spawnQueue.Add(Random.Range(1, enemyTypeCount));
        }

        // Mixed ending - weak slightly more likely than strong
        for (int i = 0; i < mixedEndCount; i++)
        {
            if (Random.value < 0.6f)
            {
                spawnQueue.Add(0);
            }
            else
            {
                spawnQueue.Add(Random.Range(1, enemyTypeCount));
            }
        }
    }

    private void UpdateActiveSpawnPoints()
    {
        if (activeSpawnPoints.Count >= spawnPoints.Count)
        {
            return;
        }

        if (currentWave == 1 || currentWave == 3 || currentWave == 6 || currentWave == 9)
        {
            AddRandomSpawnPoint();
        }
    }

    private void AddNewEnemy()
    {
        if ((currentWave % 4 == 0 || currentWave == 1) && activeEnemyPrefabs.Count < enemyPrefabs.Count)
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

    public bool GetIsSpawningWave()
    {
        return isSpawningWave;
    }

    public int GetCurrentWave()
    {
        return currentWave;
    }
}