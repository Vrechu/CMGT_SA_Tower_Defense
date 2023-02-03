using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILookAtCamera : MonoBehaviour
{
    public Camera mainCamera;
    public Canvas canvas;

    private void OnEnable()
    {
        mainCamera = Camera.main;
        canvas = GetComponent<Canvas>();
    }

    // Update is called once per frame
    void Update()
    {
        canvas.transform.LookAt(mainCamera.transform);
    }
}
