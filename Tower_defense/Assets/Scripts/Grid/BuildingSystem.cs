using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildingSystem : MonoBehaviour
{
    public static BuildingSystem Instance;
    public GridLayout gridLayout;
    private Grid grid;

    [SerializeField] private Tilemap mainTilemap;
    [SerializeField] private Tilemap whiteTile;

    public GameObject prefab1;
    public GameObject prefab2;

    private PlaceableObject ObjectToPlace;

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

        GameObject obj = Instantiate(prefab, position, Quaternion.identity);
        ObjectToPlace = obj.GetComponent<PlaceableObject>();
        obj.AddComponent<ObjectDrag>();
    }

    #endregion
}
