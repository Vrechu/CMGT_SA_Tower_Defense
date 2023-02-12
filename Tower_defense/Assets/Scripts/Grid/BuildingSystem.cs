using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;

public class BuildingSystem : MonoBehaviour
{
    [SerializeField] private GameObject[] towerTypes;
    [SerializeField] private float[] towerCosts;

    private GameObject objectToPlace;
    private int objectToPlaceType;
    private Dictionary<uint, Transform> towersPlaced = new Dictionary<uint, Transform>();
    private uint towerNumber = 0;


    private void OnEnable()
    {
        EventBus<EnoughMoneyForTowerEvent>.Subscribe(InitializeWithObject);
    }

    private void OnDestroy()
    {
        EventBus<EnoughMoneyForTowerEvent>.UnSubscribe(InitializeWithObject);
    }

    private void Start()
    {
        EventBus<TowerCostsChangedEvent>.Publish(new TowerCostsChangedEvent(towerCosts));
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && TilemapUtils.IsMouseOnBuildable() && objectToPlace != null)
        {
            EventBus<TowerPlacedEvent>.Publish(new TowerPlacedEvent(objectToPlaceType, towerCosts[objectToPlaceType]));
            towersPlaced.Add(towerNumber, objectToPlace.transform);
            towerNumber++;
            objectToPlace = null;
        }
    }


    private void InitializeWithObject(EnoughMoneyForTowerEvent enoughMoneyForTowerEvent)
    {
        GameObject prefab = towerTypes[enoughMoneyForTowerEvent.towerType];

        Vector3 position = TilemapUtils.Instance.SnapCoordinateTogrid(TilemapUtils.GetMouseWorldPosition());

        if (objectToPlace != null)
        {
            Destroy(objectToPlace.gameObject);
            objectToPlace = null;
        }

        objectToPlace = Instantiate(prefab, position, Quaternion.identity);
        objectToPlace.GetComponent<TowerID>().SetID(towerNumber);

        objectToPlaceType = enoughMoneyForTowerEvent.towerType;
    }
}
