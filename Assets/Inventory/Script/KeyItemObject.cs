using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "New Key Item", menuName = "Inventory/Items/Key Items")]
public class KeyItemObject : ItemObject
{
    private void Awake()
    {
        Name = name;
        type = ItemType.KeyItem;
    }
}
