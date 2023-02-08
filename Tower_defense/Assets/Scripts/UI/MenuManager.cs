using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour, IMenuScreen
{
    [SerializeField] private GameObject[] screenAssets;

    public void CloseScreen()
    {
        for (int i = 0; i < screenAssets.Length; i++)
        {
            screenAssets[i].SetActive(false);
        }
    }

    public void OpenScreen()
    {
        for (int i = 0; i < screenAssets.Length; i++)
        {
            screenAssets[i].SetActive(true);
        }
    }
}
