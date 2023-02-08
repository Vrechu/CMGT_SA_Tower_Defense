using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectTower : MonoBehaviour
{
    private uint selectedTower = 0;
    private bool selected = false;

    private void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            if (TilemapUtils.IsMouseOnTower() != null)
            {
                selected = true;
                EventBus<TowerSelectedEvent>.Publish(
                    new TowerSelectedEvent(TilemapUtils.IsMouseOnTower()));
            }
            else if (TilemapUtils.IsMouseOnGround())
            {
                selected = false;
                EventBus<DeselectEvent>.Publish(new DeselectEvent());
            }
        }
    }

}
