using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FinishManager : MonoBehaviour
{
    public static FinishManager Instance { get; set; }
    public event Action OnLivesChange;

    public int lives = 5;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
    }

    void Update()
    {
        
    }

    public void ReduceLives()
    {
        lives--;
        OnLivesChange?.Invoke();
    }
    public void IncreaseLives(int amount)
    {
        lives += amount;
        OnLivesChange?.Invoke();
    }

}
