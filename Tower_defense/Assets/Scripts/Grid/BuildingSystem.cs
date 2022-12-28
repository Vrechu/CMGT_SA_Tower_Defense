using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;

public class BuildingSystem : MonoBehaviour
{
    public static BuildingSystem Instance;
    public static event Action OnTowerPlaced;


    public GridLayout gridLayout;
    private Grid grid;

    [SerializeField] private Tilemap mainTilemap;

    public GameObject prefab1;
    public GameObject prefab2;

    private PlaceTower ObjectToPlace;

    #region unity methods

    private void Awake()
    {
        Instance = this;
        grid = gridLayout.gameObject.GetComponent<Grid>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            InitializeWithObject(prefab1);
        }
        else if (Input.GetKeyDown(KeyCode.B))
        {
            InitializeWithObject(prefab2);
        }

        if (Input.GetMouseButtonDown(0) && BuildingSystem.IsMouseOnBuildable() && ObjectToPlace != null)
        {
            OnTowerPlaced?.Invoke();
            ObjectToPlace = null;
        }
    }


    #endregion

    #region utils

    public static Vector3 GetMouseWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit))
        {
            return raycastHit.point;
        }
        else return Vector3.zero;
    }
    
    //fix
    public static bool IsMouseOnBuildable()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit))
        {
            if (raycastHit.collider.tag == "Buildable")
            {
                return true;
            }
        }
        return false;
    }

    public Vector3 SnapCoordinateTogrid(Vector3 position)
    {
        Vector3Int cellPosition = gridLayout.WorldToCell(position);
        position = grid.GetCellCenterWorld(cellPosition);
        return position;
    }

    #endregion

    #region building placement

    public void InitializeWithObject(GameObject prefab)
    {
        Vector3 position = SnapCoordinateTogrid(GetMouseWorldPosition());
        PlaceTower placeObjectComponent = prefab.GetComponent<PlaceTower>();

        if (ObjectToPlace != null)
        {
            Destroy(ObjectToPlace.gameObject);
            ObjectToPlace = null;
        }

        if (MoneyManager.Instance.GetMoney() >= placeObjectComponent.moneyWorth)
        {

            GameObject obj = Instantiate(prefab, position, Quaternion.identity);

            ObjectToPlace = placeObjectComponent;
            }
        }

        #endregion
    }
