using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.InventorySystem.UIs;
using RPG.ItemShopSystem;
using RPG.PlyerController;
using RPG.QuickSlot.Protion;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    int clickCount = 0;

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
}
