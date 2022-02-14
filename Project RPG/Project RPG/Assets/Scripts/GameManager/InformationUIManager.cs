using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RPG.InventorySystem.Inventory;
using RPG.InventorySystem.Items;

public class InformationUIManager : MonoBehaviour
{
    private static InformationUIManager instance;

    public GameObject InformationUI;
    public static InformationUIManager Instance => instance;

    private InventorySlot inventorySlot;

    private void Start()
    {
        instance = this;
    }

    public void ViewItemInformation(InventorySlot slot)
    {
        if (slot.ItemObject == null)
        {
            return;
        }
        else
        {
            inventorySlot = slot;

            InformationUI.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = slot.ItemObject.icon;
            InformationUI.transform.GetChild(0).GetChild(2).GetComponentInChildren<Text>().text = slot.ItemObject.data.name;
            InformationUI.transform.GetChild(0).GetChild(3).GetComponentInChildren<Text>().text = slot.ItemObject.data.buffs[0].stat.ToString() + " : " + slot.ItemObject.data.buffs[0].value.ToString();

            //Vector3 uiPoint = new Vector3(150, 150);

            //transform.GetChild(0).GetChild(0).gameObject.transform.position = GameManager.Instance.dynamicInventory.transform.position + uiPoint;

            InformationUI.transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    public void OnClickUseItem()
    {
        if (inventorySlot.ItemObject.type == ItemType.Food)
        {
            GameManager.Instance.portionController.SwapPortionSlot(inventorySlot);
            GameManager.Instance.dynamicInventory.inventoryObject.AddItem(inventorySlot.item, -inventorySlot.amount);
            InformationUIClose();
        }
        else
        {
            GameManager.Instance.dynamicInventory.OnLeftDoubleClick(inventorySlot);
            InformationUIClose();
        }
    }

    public void OnClickClose()
    {
        InformationUIClose();
    }

    public void InformationUIClose()
    {
        InformationUI.transform.GetChild(0).gameObject.SetActive(false);
        InformationUI.transform.GetChild(1).gameObject.SetActive(false);
    }
}
