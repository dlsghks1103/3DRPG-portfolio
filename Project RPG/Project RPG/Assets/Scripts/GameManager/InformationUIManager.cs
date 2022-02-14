using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RPG.InventorySystem.Inventory;
using RPG.InventorySystem.Items;

public class InformationUIManager : MonoBehaviour
{
    #region Variables
    private static InformationUIManager instance;

    public GameObject InformationUI;
    private InventorySlot inventorySlot;
    #endregion Variables

    #region Properties
    public static InformationUIManager Instance => instance;
    #endregion Properties

    #region Unity Methods
    private void Start()
    {
        instance = this;
    }
    #endregion Unity Methods

    #region Methods
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
    #endregion Methods
}
