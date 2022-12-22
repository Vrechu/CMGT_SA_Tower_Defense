using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishManager : MonoBehaviour
{
    public static FinishManager Instance;
    public int lives = 5;

    void Start()
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
        Debug.Log(lives);
    }
}
