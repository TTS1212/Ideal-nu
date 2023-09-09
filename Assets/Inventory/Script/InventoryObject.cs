using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory/Inventory")]
public class InventoryObject : ScriptableObject
{
    public List<InventorySlot> Container = new List<InventorySlot>();

    public static event Action<ItemObject, int> addedItem;
    public static event Action<InventorySlot> tossedItem;

    public void AddItem(ItemObject _item, int _amount)
    {
        bool hasItem = false;
        for (int i = 0; i < Container.Count; i++)
        {
            if (Container[i].item == _item)
            {
                Container[i].AddAmount(_amount);
                hasItem = true;
                break;
            }
        }

        if (!hasItem)
        {
            Container.Add(new InventorySlot(_item, _amount));
        }
        
        addedItem?.Invoke(_item, _amount);
    }
    
    public void TossItem(InventorySlot _item)
    {
        bool canProceed = false;
        for (int i = 0; i < Container.Count; i++)
        {
            if (Container[i] == _item)
            {
                canProceed = true;
            }
        }

        if (canProceed)
        {
            for (int i = 0; i < Container.Count; i++)
            {
                if (Container[i] == _item && Container[i].amount > 1)
                {
                    Container[i].TossAmount();
                    Debug.Log("Toss Amount");
                }
                else if (Container[i] == _item && Container[i].amount == 1)
                {
                    Container.RemoveAt(i);
                    Debug.Log("Full Remove");
                }
            }

            tossedItem?.Invoke(_item);
        }
        else
        {
            Debug.Log("Object to Toss not found.");
        }
    }
}

[System.Serializable]
public class InventorySlot
{
    public ItemObject item;
    public int amount;

    public InventorySlot(ItemObject _itemObject, int _amount)
    {
        item = _itemObject;
        amount = _amount;
    }

    public void AddAmount(int value)
    {
        amount += value;
    }
    
    public void TossAmount()
    {
        amount -= 1;
    }
}
