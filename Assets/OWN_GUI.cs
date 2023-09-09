using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OWN_GUI : MonoBehaviour
{
    public ItemObject item;
    public int amount;
    public TextMeshProUGUI amount_t;
    public InventorySlot _slot;

    private void Start()
    {
        amount_t.text = amount.ToString();
    }
}
