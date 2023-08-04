using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory 
{
    [SerializeField] private List<InventoryItem> items = new List<InventoryItem>();

    public void CropHarvestedCallback(CropType cropType) 
    {
        //track if we found a crop
        bool cropFound = false;
        
        //check whether we already have existed item in the inventory list
        for (int i = 0; i < items.Count; i++)
        {
            InventoryItem item = items[i];

            if(item.cropType == cropType) 
            {
                item.amount++;
                cropFound = true;
                break; //exit the loop if we found a existing crop
            }
        }

        //DebugInventory();

        if (cropFound)
            return; // if the crop is already within the inventory list

        //otherwise, create a new item within the list base on the cropType
        items.Add(new InventoryItem(cropType, 1));


    }

    public InventoryItem[] GetInventoryItem() 
    {
        return items.ToArray();
    }

    public void Clear() 
    {
        items.Clear();     
    }

    public void DebugInventory() 
    {
        foreach (InventoryItem item in items)
        {
            Debug.Log("We have " + item.amount + " items in our " + item.cropType + " list.");
        }
    }
}
