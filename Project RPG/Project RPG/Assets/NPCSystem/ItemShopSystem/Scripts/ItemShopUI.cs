using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using RPG.InventorySystem.Items;
using RPG.InventorySystem.Inventory;
using RPG.InventorySystem.UIs;
using RPG.StatsSystem;

namespace RPG.ItemShopSystem
{
    public class ItemShopUI : MonoBehaviour
    {
        public InventoryObject inventory;

        public ItemDatabaseObject itemDatabase;

        public GameObject[] itemShopSlot = null;

        public Dictionary<GameObject, ItemObject> itemShopList = new Dictionary<GameObject, ItemObject>();

        void Start()
        {
            for (int i = 0; i < itemDatabase.itemObjects.Length; i++)
            {
                itemShopSlot[i].transform.GetChild(0).GetComponent<Image>().sprite = itemDatabase.itemObjects[i].icon;
                itemShopSlot[i].GetComponentInChildren<Text>().text = itemDatabase.itemObjects[i].data.price.ToString() + " Gold";

               GameObject slotGO = itemShopSlot[i];

                AddEvent(slotGO, EventTriggerType.PointerClick, (data) => { OnClick(slotGO, (PointerEventData)data); });

                itemShopList.Add(slotGO, itemDatabase.itemObjects[i]);
            }
        }

        protected void AddEvent(GameObject go, EventTriggerType type, UnityAction<BaseEventData> action)
        {
            EventTrigger trigger = go.GetComponent<EventTrigger>();
            if (!trigger)
            {
                Debug.LogWarning("No EventTrigger component found!");
                return;
            }

            EventTrigger.Entry eventTrigger = new EventTrigger.Entry { eventID = type };
            eventTrigger.callback.AddListener(action);
            trigger.triggers.Add(eventTrigger);
        }

        public void OnClick(GameObject Go, PointerEventData data)
        {
            if (data.button == PointerEventData.InputButton.Left)
            {
                if (data.clickCount >= 2)
                {
                    BuyItem(Go);
                }
            }
        }

        public void BuyItem(GameObject buyItemGo)
        {
            int playerGold = StatsManager.Instance.statsObject.Gold;
            int ItemPrice = itemShopList[buyItemGo].data.price;

            if (playerGold < ItemPrice)
            {
                Debug.Log("골드가 부족합니다");
                return;
            }
            else
            {
                StatsManager.Instance.statsObject.AddGold(-ItemPrice);
                inventory.AddItem(new Item(itemShopList[buyItemGo]), 1);
            }
        }
    }
}
