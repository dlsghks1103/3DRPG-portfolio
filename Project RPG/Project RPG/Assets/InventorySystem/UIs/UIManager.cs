using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RPG.InventorySystem.Items;
using RPG.InventorySystem.UIs;

public class UIManager : MonoBehaviour
{
    #region Variables
    public static UIManager Instance;

    public ItemDatabaseObject itemDatabase;

    public StaticInventoryUI equipmentUI;
    public DynamicInventoryUI inventoryUI;

    [SerializeField]
    public GameObject QuestPanel;
    #endregion Variables

    #region Unity Methods
    private void Awake()
    {
        
        var obj = FindObjectsOfType<UIManager>();

        if (obj.Length == 1)
        {
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
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
    #endregion Unity Methods

    #region Methods
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
        QuestPanel.gameObject.SetActive(!QuestPanel.gameObject.activeSelf);
    }
    #endregion Methods
}
