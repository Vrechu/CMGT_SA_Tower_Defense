using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField]private List<GameObject> enemyTypes;
    [SerializeField]private Transform enemySpawn;

    private int waveNumber = 0;
    private int[] wave;
    private uint spawnNumber = 0;

    private Dictionary<uint, Transform> spawnedWave = new Dictionary<uint, Transform>();

    private bool canSpawn = false;
    private bool inWave = false;
    private CountdownTimer spawnCountdownTimer = new CountdownTimer(1, true);
    [SerializeField]private Wave[] waves;

    private void OnEnable()
    {
        EventBus<BuildingFaseEndedEvent>.Subscribe(StartWave);
        EventBus<EnemyKilledEvent>.Subscribe(OnEnemyKilled);
        EventBus<EnemyReachedEndEvent>.Subscribe(OnEnemyFinisheded);
        EventBus<TowerPlacedEvent>.Subscribe(OnTowerPlaced);
    }

    private void OnDestroy()
    {
        EventBus<BuildingFaseEndedEvent>.UnSubscribe(StartWave);
        EventBus<EnemyKilledEvent>.UnSubscribe(OnEnemyKilled);
        EventBus<EnemyReachedEndEvent>.UnSubscribe(OnEnemyFinisheded);
        EventBus<TowerPlacedEvent>.UnSubscribe(OnTowerPlaced);
    }

    private void Update()
    {
        if (canSpawn && spawnCountdownTimer.CountDown())
        {
            CheckIfSpawnsLeft();
        }
    }

   private void StartWave(BuildingFaseEndedEvent buildingFaseEndedEvent)
    {
        if (!inWave)
        {
            spawnCountdownTimer.SetCountdownTime(waves[waveNumber].spawnRate);
            spawnCountdownTimer.Unpause();
            inWave = true;
            canSpawn = true;
            spawnedWave.Clear();
            wave = waves[waveNumber].GetWave();

            EventBus<WaveStartEvent>.Publish(new WaveStartEvent(waveNumber, waves[waveNumber].WaveSize()));
        }
    }

    private void EndWave()
    {
        inWave = false;
        canSpawn = false;
        spawnNumber = 0;
        if (waveNumber + 1 < waves.Length) waveNumber++;
        else EventBus<WinGameEvent>.Publish(new WinGameEvent());
    }

    private void CheckIfSpawnsLeft()
    {
        if (EnemiesLeftToSpawn())
        {
            SpawnEnemy(spawnNumber);
            spawnNumber++;
        }
        else
        {
            spawnCountdownTimer.Pause();
            canSpawn = false;
        }
    }

    private void SpawnEnemy(uint pSpawnNumber)
    {
        spawnedWave.Add(pSpawnNumber, Instantiate(enemyTypes[wave[pSpawnNumber]], enemySpawn, false).transform);
        spawnedWave[pSpawnNumber].GetComponent<EnemyID>().SetID(pSpawnNumber);

        EventBus<WaveChangedEvent>.Publish(new WaveChangedEvent(spawnedWave));
    }

    private void CheckWaveEnded()
    {
        if (spawnNumber >= waves[waveNumber].WaveSize() && spawnedWave.Count == 0)
        {
            EventBus<AllEnemiesGoneEvent>.Publish(new AllEnemiesGoneEvent());
            EndWave();
        }
    }

    private bool EnemiesLeftToSpawn()
    {
        if (spawnNumber >= waves[waveNumber].WaveSize()) return false;
        return true;
    }

    private void OnEnemyKilled(EnemyKilledEvent enemyKilledEvent)
    {     
        RemoveEnemy(enemyKilledEvent.enemy);
        CheckWaveEnded();
        EventBus<WaveChangedEvent>.Publish(new WaveChangedEvent(spawnedWave));
    }

    private void OnEnemyFinisheded(EnemyReachedEndEvent enemyReachedEndEvent)
    {
        RemoveEnemy(enemyReachedEndEvent.enemy);
        CheckWaveEnded();
        EventBus<WaveChangedEvent>.Publish(new WaveChangedEvent(spawnedWave));
    }

    private void RemoveEnemy(uint enemy)
    {
        spawnedWave.Remove(enemy);
    }
   
    private void OnTowerPlaced(TowerPlacedEvent towerPlacedEvent)
    {
        EventBus<WaveChangedEvent>.Publish(new WaveChangedEvent(spawnedWave));
    }
}
