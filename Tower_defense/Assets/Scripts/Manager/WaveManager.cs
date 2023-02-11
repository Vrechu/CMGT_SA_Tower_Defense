using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField]private List<GameObject> enemyTypes;
    [SerializeField]private Transform enemySpawn;

    private int currentWave = 0;
    //[SerializeField]private Vector2[] waveComposition;
    //[SerializeField] private float[] spawnRatePerWave;
    //private int[] oldWave;
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
            spawnCountdownTimer.SetCountdownTime(waves[currentWave].spawnRate);
            spawnCountdownTimer.Unpause();
            inWave = true;
            canSpawn = true;
            spawnedWave.Clear();
            //oldWave = waves[currentWave].GetWave();

            EventBus<WaveStartEvent>.Publish(new WaveStartEvent(currentWave, waves[currentWave].WaveSize()));
        }
    }

    private void EndWave()
    {
        inWave = false;
        canSpawn = false;
        spawnNumber = 0;
        if (currentWave + 1 < waves.Length) currentWave++;
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
        spawnedWave.Add(pSpawnNumber, Instantiate(enemyTypes[waves[currentWave].GetWave()[pSpawnNumber]], enemySpawn, false).transform);
        spawnedWave[pSpawnNumber].GetComponent<EnemyID>().SetID(pSpawnNumber);

        EventBus<WaveChangedEvent>.Publish(new WaveChangedEvent(spawnedWave));
    }

    private void CheckWaveEnded()
    {
        if (spawnNumber >= waves[currentWave].WaveSize() && spawnedWave.Count == 0)
        {
            EventBus<AllEnemiesGoneEvent>.Publish(new AllEnemiesGoneEvent());
            EndWave();
        }
    }

    private bool EnemiesLeftToSpawn()
    {
        if (spawnNumber >= waves[currentWave].WaveSize()) return false;
        return true;
    }

    /*private int[] ShuffledWave()
    {
        int[] betterWave = new int[WaveSize(currentWave)];
        int enemeyNumber = 0;

        while (enemeyNumber < (int)waveComposition[currentWave][0])
        {
            betterWave[enemeyNumber] = 0;
            enemeyNumber++;
        }
        while (enemeyNumber < WaveSize(currentWave))
        {
            betterWave[enemeyNumber] = 1;
            enemeyNumber++;
        }
        Shuffle(betterWave);
        return betterWave;
    }

    private void Shuffle(int[] array)
    {
        int n = array.Length - 1;
        while (n > 1)
        {
            int k = Random.Range(0, array.Length);
            int temp = array[n];
            array[n] = array[k];
            array[k] = temp;
            n--;
        }
    }*/

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
