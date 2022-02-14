using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using RPG.Core;

[Serializable]
public class ModifiableInt
{
    #region Variables
    [SerializeField]
    private int baseValue;

    [SerializeField]
    private int modifiedValue;

    private event Action<ModifiableInt> OnModifiedValue;

    private List<IModifier> modifiers = new List<IModifier>();
    #endregion Variables

    #region Properties
    public int BaseValue
    {
        get => baseValue;
        set
        {
            baseValue = value;
            UpdateModifiedValue();
        }
    }

    public int ModifiedValue
    {
        get => modifiedValue;
        set => modifiedValue = value;
    }
    #endregion Properties

    #region Methods
    public ModifiableInt(Action<ModifiableInt> method = null)
    {
        ModifiedValue = baseValue;
        RegisterModEvent(method);

    }

    public void RegisterModEvent(Action<ModifiableInt> method)
    {
        if (method != null)
        {
            OnModifiedValue += method;
        }
    }

    public void UnregisterModEvent(Action<ModifiableInt> method)
    {
        if (method != null)
        {
            OnModifiedValue -= method;
        }
    }

    private void UpdateModifiedValue()
    {
        int valueToAdd = 0;
        foreach (IModifier modifier in modifiers)
        {
            modifier.AddValue(ref valueToAdd);
        }

        ModifiedValue = baseValue + valueToAdd;

        OnModifiedValue?.Invoke(this);
    }

    public void AddModifier(IModifier modifier)
    {
        modifiers.Add(modifier);
        UpdateModifiedValue();
    }

    public void RemoveModifier(IModifier modifier)
    {
        modifiers.Remove(modifier);
        UpdateModifiedValue();
    }
    #endregion Methods
}
