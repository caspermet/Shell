using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Gun
}

public class ItemInfo : ScriptableObject
{
    public string itemName;
    public ItemType itemType;
}
