using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;

public class BuildingSystem : MonoBehaviour
{
    [SerializeField] private GameObject[] TowerTypes;
    [SerializeField] private float[] TowerCosts;

    private GameObject ObjectToPlace;
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
        EventBus<TowerCostsChangedEvent>.Publish(new TowerCostsChangedEvent(TowerCosts));
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && TilemapUtils.IsMouseOnBuildable() && ObjectToPlace != null)
        {
            EventBus<TowerPlacedEvent>.Publish(new TowerPlacedEvent(objectToPlaceType, TowerCosts[objectToPlaceType]));
            towersPlaced.Add(towerNumber, ObjectToPlace.transform);
            towerNumber++;
            ObjectToPlace = null;
        }
    }


    public void InitializeWithObject(EnoughMoneyForTowerEvent enoughMoneyForTowerEvent)
    {
        GameObject prefab = TowerTypes[enoughMoneyForTowerEvent.towerType];

        Vector3 position = TilemapUtils.Instance.SnapCoordinateTogrid(TilemapUtils.GetMouseWorldPosition());
        PlaceTower placeObjectComponent = prefab.GetComponent<PlaceTower>();

        if (ObjectToPlace != null)
        {
            Destroy(ObjectToPlace.gameObject);
            ObjectToPlace = null;
        }

        ObjectToPlace = Instantiate(prefab, position, Quaternion.identity);
        ObjectToPlace.GetComponent<TowerID>().ID = towerNumber;

        objectToPlaceType = enoughMoneyForTowerEvent.towerType;
    }
}
