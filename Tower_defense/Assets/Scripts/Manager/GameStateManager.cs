using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance { get; set; }
    public enum GameState {Fight, Build}

    public GameState gameState = GameState.Build;


    void Start()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
    }

    private void OnEnable()
    {
        EventBus<AllEnemiesGoneEvent>.Subscribe(StartBuild);
    }

    private void OnDestroy()
    {
        EventBus<AllEnemiesGoneEvent>.UnSubscribe(StartBuild);
    }

    void Update()
    {
        StartFight();
    }

    public void SetState(GameState newState)
    {
        gameState = newState;
        switch (gameState) {

            case GameState.Build:

                break;

            case GameState.Fight:
                EventBus<BuildingFaseEndedEvent>.Publish(new BuildingFaseEndedEvent());
                break;

        }

    }

    void StartFight()
    {
        if (gameState == GameState.Build && Input.GetKeyDown(KeyCode.Space))
        {
            SetState(GameState.Fight);
        }
    }

    void StartBuild(AllEnemiesGoneEvent allEnemiesGoneEvent)
    {
        SetState(GameState.Build);
    }
}
