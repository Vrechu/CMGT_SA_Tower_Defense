using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlaceTower : MonoBehaviour
{
    public static event Action<float> OnTowerPlaced;

    private Vector3 offset;
    private bool placed = false;
    private Collider collider;
    private TowerAttack towerAttack;
    public float moneyWorth = 50;

    private void Awake()
    {
        BuildingSystem.OnTowerPlaced += Place;
    }

    private void OnDestroy()
    {
        BuildingSystem.OnTowerPlaced -= Place;
    }

    private void Start()
    {
        collider = GetComponent<Collider>();
        collider.enabled = false;
        towerAttack = GetComponent<TowerAttack>();
        towerAttack.enabled = false;
    }

    private void Update()
    {
        if (!placed)
        {
            Vector3 position = BuildingSystem.GetMouseWorldPosition() + offset;
            transform.position = BuildingSystem.Instance.SnapCoordinateTogrid(position);            
        }
    }

    private void Place()
    {
        placed = true;
        offset = transform.position - BuildingSystem.GetMouseWorldPosition();
        collider.enabled = true;
        towerAttack.enabled = true;
        OnTowerPlaced?.Invoke(moneyWorth);
    }
}