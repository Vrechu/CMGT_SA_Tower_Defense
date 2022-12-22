using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance;

    private int currentWave = 0;
    [SerializeField] private int[] waveSizes;
    private int spawnNumber = 0;
    [SerializeField] private float spawnRate = 1;
    private float spawnTimer = 1;

    public List<GameObject> EnemyTypes;
    public List<GameObject> Wave;
    public Transform enemySpawn;

    bool canSpawn = false;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
    }

    private void Start()
    {
        GameStateManager.Instance.OnFightStart += StartWave;
    }

    private void OnDestroy()
    {
        GameStateManager.Instance.OnFightStart -= StartWave;
    }


    void Update()
    {
        if (canSpawn) CountDown();
    }

    void StartWave()
    {
        canSpawn = true;
        Debug.Log("Current wave: " + currentWave);
    }
    void EndWave()
    {
        canSpawn = false;
        spawnNumber = 0;
        GameStateManager.Instance.SetState(GameStateManager.GameState.Build);
        if (currentWave + 1 < waveSizes.Length) currentWave++;
        else Debug.Log("Game Done");        
    }

    void CountDown()
    {
        if (spawnTimer > 0)
        {
            spawnTimer -= Time.deltaTime;
        }
        else
        {
            CheckSpawnsleft();
        }
    }

    void CheckSpawnsleft()
    {
        if (waveSizes[currentWave] - spawnNumber > 0)
        {
            SpawnRandomEnemy();
            spawnTimer = spawnRate;
        }
        else EndWave();
    }

    void SpawnRandomEnemy()
    {
        int randomEnemy = Random.Range(0, EnemyTypes.Count);

        if (enemySpawn != null && EnemyTypes[randomEnemy] != null)
        {
            Wave.Add(Instantiate(EnemyTypes[randomEnemy], enemySpawn, false));
            spawnNumber++;
        }
        else Debug.LogError("No enemy or spawn in list.");
    }
}
