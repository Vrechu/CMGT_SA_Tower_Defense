using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLivesManager : MonoBehaviour
{
    private int lives = 5;

    private void OnEnable()
    {
        EventBus<EnemyReachedEndEvent>.Subscribe(ReduceLives);
    }

    private void OnDestroy()
    {
        EventBus<EnemyReachedEndEvent>.UnSubscribe(ReduceLives);
    }

    private void Start()
    {
        EventBus<LivesChangedEvent>.Publish(new LivesChangedEvent(lives));
    }

    private void ReduceLives(EnemyReachedEndEvent enemyReachedEndEvent)
    {
        lives--;
        CheckLives();
        EventBus<LivesChangedEvent>.Publish(new LivesChangedEvent(lives));
    }

    private void CheckLives()
    {
        if (lives < 1)
        EventBus<LoseGameEvent>.Publish(new LoseGameEvent());
    }

}
