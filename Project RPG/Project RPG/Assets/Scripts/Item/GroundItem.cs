using RPG.InventorySystem.Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundItem : MonoBehaviour
{
    public ItemObject itemObject;

    public void ItemIcon()
    {
        GetComponent<SpriteRenderer>().sprite = itemObject.icon;
    }
}
