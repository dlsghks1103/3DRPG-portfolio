using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Linq;
using RPG.InventorySystem.Inventory;
using RPG.InventorySystem.Items;

namespace RPG.InventorySystem.Inventory
{
    [CreateAssetMenu(fileName = "New Invnetory", menuName = "Inventory System/Inventory")]
    public class InventoryObject : ScriptableObject
    {
        #region Variables

        public ItemDatabaseObject database;
        public InterfaceType type;

        [SerializeField]
        private Inventory container = new Inventory();

        public Action<InventorySlot> OnUseItem;

        #endregion Variables

        #region Properties

        public InventorySlot[] Slots => container.slots;

        public int EmptySlotCount
        {
            get
            {
                int counter = 0;
                foreach (InventorySlot slot in Slots)
                {
                    if (slot.item.id <= -1)
                    {
                        counter++;
                    }
                }

                return counter;
            }
        }

        #endregion Properties

        #region Unity Methods



        #endregion Unity Methods

        #region Methods
        public bool AddItem(Item item, int amount)
        {
            InventorySlot slot = FindItemInInventory(item);
            if (!database.itemObjects[item.id].stackable || slot == null)
            {
                if (EmptySlotCount <= 0)
                {
                    return false;
                }

                GetEmptySlot().UpdateSlot(item, amount);
            }
            else
            {
                slot.AddAmount(amount);
            }

            return true;
        }

        public InventorySlot FindItemInInventory(Item item)
        {
            return Slots.FirstOrDefault(i => i.item.id == item.id);
        }

        public bool IsContainItem(ItemObject itemObject)
        {
            return Slots.FirstOrDefault(i => i.item.id == itemObject.data.id) != null;
        }

        public InventorySlot GetEmptySlot()
        {
            return Slots.FirstOrDefault(i => i.item.id <= -1);
        }

        public void SwapItems(InventorySlot itemA, InventorySlot itemB)
        {
            if (itemA == itemB)
            {
                return;
            }

            if (itemB.CanPlaceInSlot(itemA.ItemObject) && itemA.CanPlaceInSlot(itemB.ItemObject))
            {
                InventorySlot temp = new InventorySlot(itemB.item, itemB.amount);
                itemB.UpdateSlot(itemA.item, itemA.amount);
                itemA.UpdateSlot(temp.item, temp.amount);
            }
        }

        public void OutoSwapItems(InventorySlot[] staticSlot, InventorySlot dynamicSlot)
        {
            for (int i = 0; i < staticSlot.Length; i++)
            {
                if (dynamicSlot.ItemObject == null)
                {
                    return;
                }

                if (staticSlot[i].CanPlaceInSlot(dynamicSlot.ItemObject) && dynamicSlot.CanPlaceInSlot(staticSlot[i].ItemObject))
                {
                    InventorySlot temp = new InventorySlot(staticSlot[i].item, staticSlot[i].amount);
                    staticSlot[i].UpdateSlot(dynamicSlot.item, dynamicSlot.amount);
                    dynamicSlot.UpdateSlot(temp.item, temp.amount);
                }
            }
        }
        #endregion

        [ContextMenu("Clear")]
        public void Clear()
        {
            container.Clear();
        }

        public void UseItem(InventorySlot slotToUse)
        {
            if (slotToUse.ItemObject == null || slotToUse.item.id < 0 || slotToUse.amount <= 0)
            {
                return;
            }

            if (slotToUse.ItemObject.stackable)
            {
                //ItemObject itemObject = slotToUse.ItemObject;
                //slotToUse.UpdateSlot(slotToUse.item, slotToUse.amount - 1);
                OnUseItem.Invoke(slotToUse);
            }

            else if (!slotToUse.ItemObject.stackable)
            {
                return;
            }
        }
    }
}
