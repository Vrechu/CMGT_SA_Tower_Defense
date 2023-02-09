using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField]private List<GameObject> enemyTypes;
    [SerializeField]private Transform enemySpawn;

    private int currentWave = 0;
    [SerializeField]private Vector2[] waveComposition;
    [SerializeField] private float[] spawnRatePerWave;
    private float spawnTimer = 1;
    private uint spawnNumber = 0;

    private Dictionary<uint, Transform> wave = new Dictionary<uint, Transform>();

    private bool canSpawn = false;
    private bool inWave = false;
    private CountDownTimer spawnCountdownTimer = new CountDownTimer(1, true);

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
    private void Start()
    {
        spawnCountdownTimer.Pause();
    }

    private void Update()
    {
        if (canSpawn && spawnCountdownTimer.CountDown())
        {
            SpawnRandomEnemy();
        }
    }

   private void StartWave(BuildingFaseEndedEvent buildingFaseEndedEvent)
    {
        if (!inWave)
        {
            spawnCountdownTimer.SetCountdownTime(spawnRatePerWave[currentWave]);
            spawnCountdownTimer.Unpause();
            inWave = true;
            canSpawn = true;
            wave.Clear();
            EventBus<WaveStartEvent>.Publish(new WaveStartEvent(currentWave, WaveSize()));
        }
    }

   private int WaveSize()
    {
        return (int)(waveComposition[currentWave][0] + waveComposition[currentWave][1]);
    }

   private void EndWave()
    {
        inWave = false;
        canSpawn = false;
        spawnNumber = 0;
        if (currentWave + 1 < waveComposition.Length) currentWave++;
        else EventBus<WinGameEvent>.Publish(new WinGameEvent());
    }

    private void SpawnRandomEnemy()
    {
        if (WaveSize() > 0)
        {
            int randomEnemy = Random.Range(0, 2);
            randomEnemy = EnemyLeftOfType(randomEnemy);
            wave.Add(spawnNumber, Instantiate(enemyTypes[randomEnemy], enemySpawn, false).transform);
            wave[spawnNumber].GetComponent<EnemyID>().SetID(spawnNumber);
            spawnNumber++;
            waveComposition[currentWave][randomEnemy]--;
            EventBus<WaveChangedEvent>.Publish(new WaveChangedEvent(wave));
        }
        else
        {
            spawnCountdownTimer.Pause();
            canSpawn = false;
        }
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
