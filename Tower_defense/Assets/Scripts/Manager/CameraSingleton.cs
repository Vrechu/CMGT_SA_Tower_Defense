using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSingleton : MonoBehaviour
{
    public Camera mainCamera;
    public static CameraSingleton Instance { get; set; }
    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
    }

    public Camera GetCamera()
    {
        return mainCamera;
    }
}
