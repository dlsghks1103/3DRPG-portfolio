using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.InventorySystem.UIs;
using RPG.ItemShopSystem;

public class GameManager : MonoBehaviour
{
    public ItemShopUI itemShopUI;
    public StaticInventoryUI staticInventoryUI;
    public DynamicInventoryUI dynamicInventory;

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
    }
}
