using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDrag : MonoBehaviour
{
    private Vector3 offset;
    private bool placed = false;


    private void Update()
    {
        if (!placed)
        {
            Vector3 position = BuildingSystem.GetMouseWorldPosition() + offset;
            transform.position = BuildingSystem.Instance.SnapCoordinateTogrid(position);
            if (Input.GetMouseButtonDown(0))
            {
                placed = true;
                offset = transform.position - BuildingSystem.GetMouseWorldPosition();
            }
        }
    }
}
