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
    private ITowerAttack towerAttack;

    private void OnEnable()
    {
        EventBus<TowerPlacedEvent>.Subscribe(Place);
    }

    private void OnDestroy()
    {
        EventBus<TowerPlacedEvent>.UnSubscribe(Place);
    }

    private void Start()
    {
        collider = GetComponent<Collider>();
        collider.enabled = false;
        towerAttack = GetComponent<ITowerAttack>();
        towerAttack.Enabled(false);
    }

    private void Update()
    {
        if (!placed)
        {
            Vector3 position = TilemapUtils.GetMouseWorldPosition() + offset;
            transform.position = TilemapUtils.Instance.SnapCoordinateTogrid(position);            
        }
    }

    private void Place(TowerPlacedEvent towerSelectionConfirmedEvent)
    {
        placed = true;
        offset = transform.position - TilemapUtils.GetMouseWorldPosition();
        collider.enabled = true;
        towerAttack.Enabled(true);
        EventBus<TowerPlacedEvent>.UnSubscribe(Place);
    }
}
