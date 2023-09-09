using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManagement_PLR : MonoBehaviour
{
    public InventoryObject inventory;

    public static InventoryManagement_PLR instance;

    private void Awake()
    {
        instance = this;
    }

    public static InventoryManagement_PLR GetInstance()
    {
        return instance;
    }

    private void OnApplicationQuit()
    {
        inventory.Container.Clear();
    }
}
