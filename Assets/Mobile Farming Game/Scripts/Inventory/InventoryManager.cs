using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[RequireComponent(typeof(InventoryDisplay))]
public class InventoryManager : MonoBehaviour
{
    private Inventory inventory;
    private string dataPath;
    private InventoryDisplay inventoryDisplay;
    void Start()
    {
        dataPath = Application.dataPath + "/inventoryData.txt";

        LoadInventory();
        ConfigureInventoryDisplay();

        CropTile.onCropHarvested += CropHarvestedCallback;
    }


    private void OnDestroy()
    {
        CropTile.onCropHarvested -= CropHarvestedCallback;
    }

    private void ConfigureInventoryDisplay() 
    {

        inventoryDisplay = GetComponent<InventoryDisplay>();
        inventoryDisplay.Configure(inventory);
    }

    private void CropHarvestedCallback(CropType cropType)
    {
        //Update inventory
        inventory.CropHarvestedCallback(cropType);

        inventoryDisplay.UpdateDisplay(inventory);
        SaveInventory();
    }

    [NaughtyAttributes.Button]
    public void ClearInventory() 
    {
        inventory.Clear();
        inventoryDisplay.UpdateDisplay(inventory);
        SaveInventory();
    }

    public Inventory GetInventory() 
    {
        return inventory;
    }
    private void LoadInventory() 
    {
        string data = "";

        if (File.Exists(dataPath)) 
        {
            data = File.ReadAllText(dataPath);
            inventory = JsonUtility.FromJson<Inventory>(data);

            if(inventory == null) 
            {
                inventory = new Inventory(); //check if we already created inventory, if not create a new one
            }          
        }
        else 
        {
            //if there's no save file, create one
            File.Create(dataPath);
            inventory = new Inventory();
        }

    }

    private void SaveInventory() 
    {
        string data = JsonUtility.ToJson(inventory, true);
        File.WriteAllText(dataPath, data);
    }
}
