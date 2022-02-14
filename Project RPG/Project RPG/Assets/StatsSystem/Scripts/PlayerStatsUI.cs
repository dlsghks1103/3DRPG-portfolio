using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RPG.InventorySystem.Inventory;
using RPG.InventorySystem.Items;
using RPG.Enemy;

public class PlayerStatsUI : MonoBehaviour
{
    #region Variables
    public InventoryObject equipment;
    public StatsObject stats;

    public Text[] attributeTexts;
    #endregion Variables

    #region Unity Methods
    private void OnEnable()
    {
        stats.OnChangedStats += OnChangedStats;
        stats.OnChangedExp += OnChangedStats;

        if (equipment != null && stats != null)
        {
            foreach (InventorySlot slot in equipment.Slots)
            {
                slot.OnPreUpdate += OnRemoveItem; 
                slot.OnPostUpdate += OnEquipItem;
            }
        }

        UpdateAttributeTexts();
    }

    private void OnDisable()
    {
        stats.OnChangedStats -= OnChangedStats;
        stats.OnChangedExp += OnChangedStats;

        if (equipment != null && stats != null)
        {
            foreach (InventorySlot slot in equipment.Slots)
            {
                slot.OnPreUpdate -= OnRemoveItem;
                slot.OnPostUpdate -= OnEquipItem;
            }
        }
    }
    #endregion Unity Methods

    #region Methods
    private void UpdateAttributeTexts()
    {
        attributeTexts[0].text = stats.GetBaseValue(AttributeType.Agility) + " ( +" + stats.GetAddValue(AttributeType.Agility) + " )";
        attributeTexts[1].text = stats.GetBaseValue(AttributeType.Intellect) + " ( +" + stats.GetAddValue(AttributeType.Intellect) + " )";
        attributeTexts[2].text = stats.GetBaseValue(AttributeType.Stamina) + " ( +" + stats.GetAddValue(AttributeType.Stamina) + " )";
        attributeTexts[3].text = stats.GetBaseValue(AttributeType.Strength) + " ( +" + stats.GetAddValue(AttributeType.Strength) + " )";
        attributeTexts[4].text = stats.SetDamage(AttributeType.Strength, AttributeType.Agility).ToString("n0");
    }

    public void OnRemoveItem(InventorySlot slot)
    {
        if (slot.ItemObject == null)
        {
            return;
        }

        Debug.Log("OnRemoveItem");

        if (slot.parent.type == InterfaceType.Equipment)
        {
            foreach (ItemBuff buff in slot.item.buffs)
            {
                foreach (Attribute attribute in stats.attributes)
                {
                    if (attribute.type == buff.stat)
                    {
                        attribute.value.RemoveModifier(buff);
                    }
                }
            }
        }
    }

    public void OnEquipItem(InventorySlot slot)
    {
        if (slot.ItemObject == null)
        {
            return;
        }

        Debug.Log("OnEquipItem");

        if (slot.parent.type == InterfaceType.Equipment)
        {
            foreach (ItemBuff buff in slot.item.buffs)
            {
                foreach (Attribute attribute in stats.attributes)
                {
                    if (attribute.type == buff.stat)
                    {
                        attribute.value.AddModifier(buff);
                    }
                }
            }
        }
    }

    public void OnChangedStats(StatsObject statsObject)
    {
        Debug.Log("OnChangedStats");
        UpdateAttributeTexts();
    }
    #endregion Methods
}
