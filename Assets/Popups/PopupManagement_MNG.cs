using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopupManagement_MNG : MonoBehaviour
{
    public GameObject prefab;

    public Transform parentCanvas;

    public static PopupManagement_MNG instance;

    private void Awake()
    {
        instance = this;
    }

    private void OnEnable()
    {
        InventoryObject.addedItem += OBJADDED;
    }

    private void OnDisable()
    {
        InventoryObject.addedItem -= OBJADDED;
    }


    public static PopupManagement_MNG GetInstance()
    {
        return instance;
    }

    public void CreatePopUp(string Reading)
    {
        GameObject newPopup = Instantiate(prefab, parentCanvas);
        newPopup.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = Reading;
    }

    public void OBJADDED(ItemObject item, int amount)
    {
        CreatePopUp("Added <b><i>" + amount + "x</b></i> " + item.Name + " to the inventory.");
    }
}
