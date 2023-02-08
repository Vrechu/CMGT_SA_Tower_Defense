using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilemapUtils : MonoBehaviour
{
    public static TilemapUtils Instance { get; set; }
    public GridLayout gridLayout;
    private Grid grid;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        grid = gridLayout.gameObject.GetComponent<Grid>();
    }

    public static Vector3 GetMouseWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit))
        {
            return raycastHit.point;
        }
        else return Vector3.zero;
    }

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

    public static TowerID IsMouseOnTower()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit))
        {
            if (raycastHit.collider.tag == "Tower")
            {
                return raycastHit.collider.GetComponent<TowerID>();
            }
        }
        return null;
    }

    public static bool IsMouseOnGround()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit))
        {
            if (raycastHit.collider.tag == "Buildable" || raycastHit.collider.tag == "Ground")
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
}
