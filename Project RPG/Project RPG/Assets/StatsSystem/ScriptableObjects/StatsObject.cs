using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "New Stats", menuName = "Stats System/New Character Stats")]
public class StatsObject : ScriptableObject
{
    public Attribute[] attributes;

    public Action<StatsObject> OnChangedStats;
    public Action<StatsObject> OnChangedExp;

    public int Level
    {
        get; set;
    }

    [NonSerialized]
    public int[] LevelTabel = new int[100];

    public int Exp
    {
        get; set;
    }

    public int Gold
    {
        get; set;
    }

    public int Health
    {
        get; set;
    }

    public int Mana
    {
        get;
        set;
    }

    public string ExpState
    {
        get
        {
            int exp = Exp;
            int maxExp = LevelTabel[Level+1];

            return maxExp > 0 ? exp + " / " + maxExp : "0/0";
        }
    }

    public string HealthState
    {
        get 
        {
            int health = Health;
            int maxHealth = health;

            foreach (Attribute attribute in attributes)
            {
                if (attribute.type == AttributeType.Health)
                {
                    maxHealth = attribute.value.ModifiedValue;
                }
            }

            return maxHealth > 0 ? health + " / " + maxHealth : "0/0";
        }
    }

    public string ManaState
    {
        get
        {
            int mana = Mana;
            int maxMana = mana;

            foreach (Attribute attribute in attributes)
            {
                if (attribute.type == AttributeType.Mana)
                {
                    maxMana = attribute.value.ModifiedValue;
                }
            }

            return maxMana > 0 ? mana + " / " + maxMana : "0/0";
        }
    }

    public int MaxMana
    {
        get 
        {
            int maxMana = 0;

            foreach (Attribute attribute in attributes)
            {
                if (attribute.type == AttributeType.Mana)
                {
                    maxMana = attribute.value.ModifiedValue;
                }
            }

            return maxMana > 0 ? maxMana : 0;
        }
    }

    public int MaxHealth
    {
        get
        {
            int maxHealth = 0;

            foreach (Attribute attribute in attributes)
            {
                if (attribute.type == AttributeType.Health)
                {
                    maxHealth = attribute.value.ModifiedValue;
                }
            }

            return maxHealth > 0 ? maxHealth : 0;
        }
    }

    public float HealthPercentage
    {
        get
        {
            int health = Health;
            int maxHealth = health;

            foreach (Attribute attribute in attributes)
            {
                if (attribute.type == AttributeType.Health)
                {
                    maxHealth = attribute.value.ModifiedValue;
                }
            }

            return (maxHealth > 0 ? ((float)health / (float)maxHealth) : 0f);
        }
    }
    public float ManaPercentage
    {
        get
        {
            int mana = Mana;
            int maxMana = mana;

            foreach (Attribute attribute in attributes)
            {
                if (attribute.type == AttributeType.Mana)
                {
                    maxMana = attribute.value.ModifiedValue;
                }
            }

            return (maxMana > 0 ? ((float)mana / (float)maxMana) : 0f);
        }
    }

    public float ExpPercentage
    {
        get
        {
            int exp = Exp - LevelTabel[Level];
            int maxExp = LevelTabel[Level+1] - LevelTabel[Level];
            
            return (maxExp > 0 ? ((float)exp / (float)maxExp) : 0f);
        }
    }
  

    public void OnEnable()
    {
        InitializeAttributes();
    }

    public int GetBaseValue(AttributeType type)
    {
        foreach (Attribute attribute in attributes)
        {
            if (attribute.type == type)
            {
                return attribute.value.BaseValue;
            }
        }

        return -1;
    }

    public int GetAddValue(AttributeType type)
    {
        return GetModifiedValue(type) - GetBaseValue(type);
    }

    public int GetModifiedValue(AttributeType type)
    {
        foreach (Attribute attribute in attributes)
        {
            if (attribute.type == type)
            {
                return attribute.value.ModifiedValue;
            }
        }

        return -1;
    }

    public int SetDamage(AttributeType strength, AttributeType agility)
    {
        return GetModifiedValue(strength) + GetModifiedValue(agility);
    }

    public void SetBaseValue(AttributeType type, int value)
    {
        foreach (Attribute attribute in attributes)
        {
            if (attribute.type == type)
            {
                attribute.value.BaseValue = value;
            }
        }
    }

    public void AddBaseValue(AttributeType type, int value)
    {
        foreach (Attribute attribute in attributes)
        {
            if (attribute.type == type)
            {
                attribute.value.BaseValue += value;
            }
        }
    }

    public int AddHealth(int value)
    {
        Health += value;

        OnChangedStats?.Invoke(this);

        return Health;
    }

    public int AddMana(int value)
    {
        Mana += value;

        OnChangedStats?.Invoke(this);
        return Mana;
    }

    public int AddExp(int value)
    {
        LevelUp(Exp += value);

        OnChangedExp?.Invoke(this);
        return Exp;
    }

    public int AddGold(int value)
    {
        Gold += value;
        OnChangedStats?.Invoke(this);
        return Gold;
    }

    [NonSerialized]
    private bool isInitialized = false;

    public void InitializeAttributes()
    {
        if (isInitialized)
        {
            return;
        }

        isInitialized = true;
        Debug.Log("InitializeAttributes");

        foreach (Attribute attribute in attributes)
        {
            attribute.value = new ModifiableInt(OnModifiedValue);
        }

        SetBaseValue(AttributeType.Agility, 5);
        SetBaseValue(AttributeType.Intellect, 1);
        SetBaseValue(AttributeType.Stamina, 1);
        SetBaseValue(AttributeType.Strength, 5);
        SetBaseValue(AttributeType.Health, 1000);
        SetBaseValue(AttributeType.Mana, 200);
        SetBaseValue(AttributeType.Level, 0);
        SetBaseValue(AttributeType.Exp, 0);
        SetBaseValue(AttributeType.Gold, 3000);

        Health = GetModifiedValue(AttributeType.Health);
        Mana = GetModifiedValue(AttributeType.Mana);
        Level = GetModifiedValue(AttributeType.Level);
        Exp = GetModifiedValue(AttributeType.Exp);
        Gold = GetModifiedValue(AttributeType.Gold);

        SetLevelTableL();
    }

    private void SetLevelTableL()
    {
        int MaxExp = 0;

        for (int i = 0; i <= 50; ++i)
        {
            LevelTabel[i] = MaxExp;
            MaxExp += 100;
        }
    }

    private void LevelUp(int exp)
    {
        if (Level >= 50)
        {
            return;
        }

        if (exp >= LevelTabel[Level+1])
        {
            Level++;
            AddLevelUpStats();
        }
        else
        {
            return;
        }
        LevelUp(exp);
    }

    private void AddLevelUpStats()
    {
        AddBaseValue(AttributeType.Agility, 3);
        AddBaseValue(AttributeType.Intellect, 1);
        AddBaseValue(AttributeType.Stamina, 1);
        AddBaseValue(AttributeType.Strength, 3);
    }

    private void OnModifiedValue(ModifiableInt value)
    {
        OnChangedStats?.Invoke(this);
        OnChangedExp?.Invoke(this);
    }
}
