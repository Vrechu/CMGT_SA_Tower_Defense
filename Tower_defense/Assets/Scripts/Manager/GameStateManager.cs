using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance { get; private set; }
    private enum GameState {Fight, Build, Win, Lose}

    private GameState gameState = GameState.Build;


    void Start()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
    }

    private void OnEnable()
    {
        EventBus<AllEnemiesGoneEvent>.Subscribe(StartBuild);
        EventBus<StartPressedEvent>.Subscribe(StartFight);
        EventBus<WinGameEvent>.Subscribe(OnGameWin);
        EventBus<LoseGameEvent>.Subscribe(OnGameLose);
    }

    private void OnDestroy()
    {
        EventBus<AllEnemiesGoneEvent>.UnSubscribe(StartBuild);
        EventBus<StartPressedEvent>.UnSubscribe(StartFight);
        EventBus<WinGameEvent>.UnSubscribe(OnGameWin);
        EventBus<LoseGameEvent>.UnSubscribe(OnGameLose);
    }

    private void SetState(GameState newState)
    {
        gameState = newState;
        switch (gameState) {

            case GameState.Build:

                break;

            case GameState.Fight:
                EventBus<BuildingFaseEndedEvent>.Publish(new BuildingFaseEndedEvent());
                break;
            case GameState.Win:
                Time.timeScale = 0;
                break;
            case GameState.Lose:
                Time.timeScale = 0;
                break;

        }

    }

   private void StartFight(StartPressedEvent startPressedEvent)
    {
        if (gameState == GameState.Build)
        {
            SetState(GameState.Fight);
        }
    }

    private void StartBuild(AllEnemiesGoneEvent allEnemiesGoneEvent)
    {
        SetState(GameState.Build);
    }

    private void OnGameWin(WinGameEvent winGameEvent)
    {
        SetState(GameState.Win);
    }

    private void OnGameLose(LoseGameEvent loseGameEvent)
    {
    SetState(GameState.Lose);
    }
}
