using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CropTile : MonoBehaviour
{
    private TileFieldState state;

    [Header("Elements")]
    [SerializeField] private Transform cropParent;
    [SerializeField] private MeshRenderer tileRenderer;
    private Crop crop;
    private CropData cropData;

    [Header("Events")]
    public static Action<CropType> onCropHarvested;
    void Start()
    {
        state = TileFieldState.Empty;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Sow( CropData cropData) 
    {
        state = TileFieldState.Sown;

        crop = Instantiate(cropData.cropPrefab, transform.position, Quaternion.identity,cropParent);

        this.cropData = cropData;
    }

    public void Water(CropData cropData)
    {
        state = TileFieldState.Watered;

        //changing tile color
        tileRenderer.gameObject.LeanColor(Color.white * 0.45f, 1);
        //scale up crop;
        crop.ScaleUp();

        //StartCoroutine(ColorTileCoroutine());
    }
   /* IEnumerator ColorTileCoroutine() 
    {
        float duration = 1;
        float timer = 0;

        while(timer< duration) 
        {
            float t = timer / duration;
            Color lerpedColor = Color.Lerp(Color.white, Color.white * 0.45f, t);

            tileRenderer.material.color = lerpedColor;
            timer += Time.deltaTime;
            yield return null; // skip a frame to prevent blocking the main thread
        }
        yield return null;
    }*/

    public void Harvest() 
    {
        state = TileFieldState.Empty;

        crop.ScaleDown();

        tileRenderer.gameObject.LeanColor(Color.white, 1);

        onCropHarvested?.Invoke(cropData.cropType);
    }
    public bool IsEmpty()
    {
        return state == TileFieldState.Empty;
    }

    public bool IsSown()
    {
        return state == TileFieldState.Sown;
    }


}
