using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using RPG.InventorySystem.UIs;
using RPG.ItemShopSystem;
using RPG.PlyerController;
using RPG.QuickSlot.Protion;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Variables
    public ItemShopUI itemShopUI;
    public StaticInventoryUI staticInventoryUI;
    public InformationUIManager informationUI;
    public DynamicInventoryUI dynamicInventory;

    public PlayerController playerController;
    public PortionController portionController;

    [NonSerialized]
    public bool IsInventoryDrag = false;

    private static GameManager instance;

    int clickCount = 0;
    #endregion Variables

    #region Properties
    public static GameManager Instance => instance;
    #endregion Properties

    #region Unity Methods
    private void Awake()
    {
        var obj = FindObjectsOfType<GameManager>();

        if (obj.Length == 1)
        {
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(obj[1]);
        }
    }

    private void Start()
    {
        instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            clickCount++;
            if (!IsInvoking("DoubleClick"))
                Invoke("DoubleClick", 1.0f);

        }
        else if (clickCount == 2)
        {
            CancelInvoke("DoubleClick");
            Application.Quit();
        }
    }
    #endregion Unity Methods

    #region Methods
    public void CloseUI()
    {
        if (itemShopUI != null)
        {
            itemShopUI.gameObject.SetActive(false);
            staticInventoryUI.gameObject.SetActive(false);
            dynamicInventory.gameObject.SetActive(false);
        }
    }

    void DoubleClick()
    {
        clickCount = 0;
    }
    #endregion Methods
}
