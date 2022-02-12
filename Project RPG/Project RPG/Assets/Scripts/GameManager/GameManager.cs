using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.InventorySystem.UIs;
using RPG.ItemShopSystem;
using RPG.PlyerController;
using RPG.QuickSlot.Protion;

public class GameManager : MonoBehaviour
{
    public ItemShopUI itemShopUI;
    public StaticInventoryUI staticInventoryUI;
    public DynamicInventoryUI dynamicInventory;
    public PlayerController playerController;
    public InformationUIManager informationUI;
    public PortionController portionController;

    private static GameManager instance;

    public static GameManager Instance => instance;

    private void Awake()
    {
        instance = this;
    }

    public void CloseUI()
    {
        itemShopUI.gameObject.SetActive(false);
        staticInventoryUI.gameObject.SetActive(false);
        dynamicInventory.gameObject.SetActive(false);
        //informationUI.transform.GetChild(0).gameObject.SetActive(false);
    }
}
