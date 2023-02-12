using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectTower : MonoBehaviour
{
    private void Update()
    {
        CheckIfMouseOnTower();        
    }

    private void CheckIfMouseOnTower()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (TilemapUtils.IsMouseOnTower() != null)
            {
                EventBus<TowerSelectedEvent>.Publish(
                    new TowerSelectedEvent(TilemapUtils.IsMouseOnTower()));
            }
            else if (TilemapUtils.IsMouseOnGround())
            {
                EventBus<DeselectEvent>.Publish(new DeselectEvent());
            }
        }
    }

}
