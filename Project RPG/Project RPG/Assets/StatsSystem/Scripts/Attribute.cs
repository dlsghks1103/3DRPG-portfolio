using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum AttributeType
{
    Agility,
    Intellect,
    Stamina,
    Strength,
    Health,
    Mana,
    Level,
    Exp,
}

[Serializable]
public class Attribute
{
    public AttributeType type;
    public ModifiableInt value;
}

