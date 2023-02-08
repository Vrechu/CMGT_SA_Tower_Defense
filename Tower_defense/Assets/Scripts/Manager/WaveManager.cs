using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{

    private int currentWave = 0;
    private uint spawnNumber = 0;
    [SerializeField] private float spawnRate = 1;
    private float spawnTimer = 1;

    public List<GameObject> EnemyTypes;
    private Dictionary<uint, Transform> wave = new Dictionary<uint, Transform>();

    public Vector2[] waveComposition;

    public Transform enemySpawn;

    private bool canSpawn = false;


    private void OnEnable()
    {
        EventBus<BuildingFaseEndedEvent>.Subscribe(StartWave);
        EventBus<EnemyKilledEvent>.Subscribe(OnEnemyKilled);
        EventBus<EnemyReachedEndEvent>.Subscribe(OnEnemyFinisheded);
        EventBus<TowerPlacedEvent>.Subscribe(UpdateWave);
    }

    private void OnDestroy()
    {
        EventBus<BuildingFaseEndedEvent>.UnSubscribe(StartWave);
        EventBus<EnemyKilledEvent>.UnSubscribe(OnEnemyKilled);
        EventBus<EnemyReachedEndEvent>.UnSubscribe(OnEnemyFinisheded);
        EventBus<TowerPlacedEvent>.UnSubscribe(UpdateWave);
    }

    private void Update()
    {
        if (canSpawn) CountDown();
    }

   private void StartWave(BuildingFaseEndedEvent buildingFaseEndedEvent)
    {
        canSpawn = true;
        wave.Clear();
        Debug.Log("Current wave: " + currentWave);
        EventBus<WaveStartEvent>.Publish(new WaveStartEvent(currentWave, WaveSize()));
    }

   private int WaveSize()
    {
        return (int)(waveComposition[currentWave][0] + waveComposition[currentWave][1]);
    }

   private void EndWave()
    {
        spawnNumber = 0;
        if (currentWave + 1 < waveComposition.Length) currentWave++;
        else Debug.Log("Game Done");
    }

    private void CountDown()
    {
        if (spawnTimer > 0)
        {
            spawnTimer -= Time.deltaTime;
        }
        else
        {
            SpawnRandomEnemy();
            spawnTimer = spawnRate;
        }
    }


    private void SpawnRandomEnemy()
    {
        if (WaveSize() > 0)
        {
            int randomEnemy = Random.Range(0, 2);
            randomEnemy = EnemyLeftOfType(randomEnemy);
            wave.Add(spawnNumber, Instantiate(EnemyTypes[randomEnemy], enemySpawn, false).transform);
            wave[spawnNumber].GetComponent<EnemyID>().SetID(spawnNumber);          
            spawnNumber++;
            waveComposition[currentWave][randomEnemy]--;
            EventBus<WaveChangedEvent>.Publish(new WaveChangedEvent(wave));
        }
        else canSpawn = false;
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
        RemoveEnemy(enemyKilledEvent.enemy);
        CheckWaveEnded();
        EventBus<WaveChangedEvent>.Publish(new WaveChangedEvent(wave));
    }

    private void OnEnemyFinisheded(EnemyReachedEndEvent enemyReachedEndEvent)
    {
        RemoveEnemy(enemyReachedEndEvent.enemy);
        CheckWaveEnded();
        EventBus<WaveChangedEvent>.Publish(new WaveChangedEvent(wave));
    }

    private void RemoveEnemy(uint enemy)
    {
        wave.Remove(enemy);
    }

    private void CheckWaveEnded()
    {
        if (WaveSize() == 0 && wave.Count == 0)
        {
            EventBus<AllEnemiesGoneEvent>.Publish(new AllEnemiesGoneEvent());
            EndWave();
        }
    }

    private void UpdateWave(TowerPlacedEvent towerPlacedEvent)
    {
        EventBus<WaveChangedEvent>.Publish(new WaveChangedEvent(wave));
    }
}
