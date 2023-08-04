using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager instance;

    private void Awake()
    {
        #region SingleTon
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        #endregion
    }

    [Header("Data")]
    [SerializeField] private CropData[] cropData;

    public Sprite GetCropSpriteFromCropType(CropType cropType) 
    {
        for (int i = 0; i < cropData.Length; i++)
        {
            if(cropData[i].cropType == cropType) 
            {
                return cropData[i].icon;
            }
        }

        Debug.LogError("No CropData found of that type");
        return null;
    }

    public int GetCropPriceFromCropType(CropType cropType)
    {
        for (int i = 0; i < cropData.Length; i++)
        {
            if (cropData[i].cropType == cropType)
            {
                return cropData[i].price;
            }
        }

        Debug.LogError("No CropData found of that type");
        return 0;
    }
}
