using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using RPG.InventorySystem.Items;
using System.Linq;

namespace RPG.InventorySystem.Inventory
{
    [Serializable]
    public class Inventory
    {
        #region Variables

        public InventorySlot[] slots = new InventorySlot[24];

        #endregion Variables

        #region Methods

        public void Clear()
        {
            foreach (InventorySlot slot in slots)
            {
                slot.UpdateSlot(new Item(), 0);
            }
        }

        public bool IsContain(ItemObject itemObject)
        {
            return Array.Find(slots, i => i.item.id == itemObject.data.id) != null;
        }

        public bool IsContain(int id)
        {
            return slots.FirstOrDefault(i => i.item.id == id) != null;
        }

        #endregion Methods
    }
}
