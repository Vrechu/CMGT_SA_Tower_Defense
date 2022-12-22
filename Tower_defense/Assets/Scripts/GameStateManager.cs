using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance { get; set; }
    public event Action OnFightStart;
    public enum GameState {Fight, Build}

    public GameState gameState = GameState.Build;


    void Start()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
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
                OnFightStart?.Invoke();
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
}
