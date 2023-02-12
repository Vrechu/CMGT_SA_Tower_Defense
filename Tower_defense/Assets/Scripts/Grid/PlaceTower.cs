using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlaceTower : MonoBehaviour
{
    private Vector3 offset;
    private bool placed = false;
    private Collider towerCollider;
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
        towerCollider = GetComponent<Collider>();
        towerCollider.enabled = false;
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

    private void Place(TowerPlacedEvent towerPlacedEvent)
    {
        placed = true;
        offset = transform.position - TilemapUtils.GetMouseWorldPosition();
        towerCollider.enabled = true;
        towerAttack.Enabled(true);
        EventBus<TowerPlacedEvent>.UnSubscribe(Place);
    }
}
