using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryDisplay : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Transform cropContainerParent;
    [SerializeField] private UICropContainer uICropContainerPrefab;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Configure(Inventory inventory) 
    {
        InventoryItem[] items = inventory.GetInventoryItem();

        for (int i = 0; i < items.Length; i++)
        {
            UICropContainer cropContainerInstance = Instantiate(uICropContainerPrefab, cropContainerParent);

            Sprite cropIcon = DataManager.instance.GetCropSpriteFromCropType(items[i].cropType);
            
            cropContainerInstance.Configure(cropIcon,items[i].amount);
        }
    }

    public void UpdateDisplay(Inventory inventory) 
    {
        InventoryItem[] items = inventory.GetInventoryItem();

        for (int i = 0; i < items.Length; i++)
        {
            UICropContainer containerInstance;

            if (i < cropContainerParent.childCount) 
            {
                containerInstance = cropContainerParent.GetChild(i).GetComponent<UICropContainer>();
                containerInstance.gameObject.SetActive(true);
            }
            else 
            {
                //if the cropcontainer is not enough, creat new one
                containerInstance = Instantiate(uICropContainerPrefab, cropContainerParent);
            }

            Sprite cropIcon = DataManager.instance.GetCropSpriteFromCropType(items[i].cropType);
            containerInstance.Configure(cropIcon, items[i].amount);

        }
           
            int remainingContainers = cropContainerParent.childCount - items.Length;
            if (remainingContainers <= 0) return;

            //if the containers is more than what we require, set it back to false
            for (int i = 0; i < remainingContainers; i++)
            {
                cropContainerParent.GetChild(items.Length + i).gameObject.SetActive(false);
            }

    }
}
