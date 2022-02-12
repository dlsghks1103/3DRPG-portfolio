using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.InventorySystem.Items;

namespace RPG.QuickSlot
{
    [CreateAssetMenu(fileName = "New QuickSlot", menuName = "QuickSlot System/QuickSlot")]
    public class QuickSlotObject : ScriptableObject
    {
        public ItemObject itemObject;

        public Item item;
        public int amount;

        private void Awake()
        {
            item = itemObject.data;
        }
    }

  
}
