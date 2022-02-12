using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RPG.InventorySystem.Items;
using RPG.InventorySystem.UIs;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public ItemDatabaseObject itemDatabase;

    public StaticInventoryUI equipmentUI;
    public DynamicInventoryUI inventoryUI;
    [SerializeField]
    public ScrollRect scrollView;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown("i"))
        {
            inventoryUI.gameObject.SetActive(!inventoryUI.gameObject.activeSelf);
        }

        if (Input.GetKeyDown("e"))
        {
            equipmentUI.gameObject.SetActive(!equipmentUI.gameObject.activeSelf);
        }
    }

    public void OnClickInventoryButton()
    {
        inventoryUI.gameObject.SetActive(!inventoryUI.gameObject.activeSelf);
    }

    public void OnClickEquipmentButton()
    {
        equipmentUI.gameObject.SetActive(!equipmentUI.gameObject.activeSelf);
    }

    public void OnClickQuestUIButton()
    {
        scrollView.gameObject.SetActive(!scrollView.gameObject.activeSelf);
    }
}
