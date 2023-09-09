using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Food Item", menuName = "Inventory/Items/Foods")]
public class FoodObject : ItemObject
{
    public int restoreHealthValue;

    private void Awake()
    {
        Name = name;
        type = ItemType.Food;
    }
}
