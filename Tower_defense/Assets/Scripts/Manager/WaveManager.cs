using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance;

    private int currentWave = 0;
    private int spawnNumber = 0;
    [SerializeField] private float spawnRate = 1;
    private float spawnTimer = 1;

    public List<GameObject> EnemyTypes;
    public List<GameObject> Wave;

    public Vector2[] waveComposition;
    private int waveSize;
    private int enemiesKilledInWave;

    public Transform enemySpawn;

    bool canSpawn = false;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
    }

    private void OnEnable()
    {
        EventBus<BuildingFaseEndedEvent>.Subscribe(StartWave);
        EventBus<EnemyKilledEvent>.Subscribe(OnEnemyKilled);
        EventBus<EnemyReachedEndEvent>.Subscribe(OnEnemyFinisheded);
    }

    private void OnDestroy()
    {
        EventBus<BuildingFaseEndedEvent>.UnSubscribe(StartWave);
        EventBus<EnemyKilledEvent>.UnSubscribe(OnEnemyKilled);
        EventBus<EnemyReachedEndEvent>.UnSubscribe(OnEnemyFinisheded);
    }

    void Update()
    {
        if (canSpawn) CountDown();
    }

    void StartWave(BuildingFaseEndedEvent buildingFaseEndedEvent)
    {
        Wave.Clear();
        canSpawn = true;
        Debug.Log("Current wave: " + currentWave);
        CalculateWaveSize();
        EventBus<WaveStartEvent>.Publish(new WaveStartEvent(waveSize));
        enemiesKilledInWave = 0;
    }

    void CalculateWaveSize()
    {
        waveSize = (int)(waveComposition[currentWave][0] + waveComposition[currentWave][1]);
    }

    void EndWave()
    {
        canSpawn = false;
        spawnNumber = 0;
        if (currentWave + 1 < waveSize) currentWave++;
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
        if (waveSize - spawnNumber > 0)
        {
            SpawnRandomEnemy();
            spawnTimer = spawnRate;
        }
        else EndWave();
    }

    void SpawnRandomEnemy()
    {
        int randomEnemy = Random.Range(0, 2);
        randomEnemy = EnemyLeftOfType(randomEnemy);

        if (enemySpawn != null && EnemyTypes[randomEnemy] != null)
        {
            Wave.Add(Instantiate(EnemyTypes[randomEnemy], enemySpawn, false));
            spawnNumber++;
            waveComposition[currentWave][randomEnemy]--;
        }
        else Debug.LogError("No enemy or spawn in list.");
    }

    private int EnemyLeftOfType(int enemy)
    {
        if (waveComposition[currentWave][enemy] > 0)
        {
            return enemy;
        }
        else if (enemy == 0) return EnemyLeftOfType(1);
        else return EnemyLeftOfType(0);
    }

    private void OnEnemyKilled(EnemyKilledEvent enemyKilledEvent)
    {        
        ReduceEnemies();
    }

    private void OnEnemyFinisheded(EnemyReachedEndEvent enemyReachedEndEvent)
    {        
        ReduceEnemies();
    }

    private void ReduceEnemies()
    {
        enemiesKilledInWave++;
        if (enemiesKilledInWave >= waveSize) EventBus<AllEnemiesGoneEvent>.Publish(new AllEnemiesGoneEvent());
    }
}
