using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Object", menuName = "Inventory/Items/Default Objects")]
public class DefaultObject : ItemObject
{
    private void Awake()
    {
        Name = name;
        type = ItemType.Object;
    }
}
