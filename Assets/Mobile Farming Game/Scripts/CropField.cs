using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CropField : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Transform tilesParent;
    private List<CropTile> cropTiles = new List<CropTile>();

    [Header("Settings")]
    [SerializeField] private CropData cropData;
    private int tilesSown;
    public int tilesWatered;
    private TileFieldState state;

    [Header("Actions")]
    public static Action<CropField> onFullySown;
    public static Action<CropField> onFullyWatered;

    void Start()
    {
        state = TileFieldState.Empty;
        StoreTiles();
    }

    private void StoreTiles()
    {
        for (int i = 0; i < tilesParent.childCount; i++)
        {
            cropTiles.Add(tilesParent.GetChild(i).GetComponent<CropTile>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SeedsCollidedCallback(Vector3[] seedPositions) 
    {
        for (int i = 0; i < seedPositions.Length; i++)
        {
            CropTile closestCropTile = GetClosestCropTile(seedPositions[i]);

            if (closestCropTile == null) continue;

            if (!closestCropTile.IsEmpty()) continue;

            Sow(closestCropTile);
        }
       
    }

    private void Sow(CropTile cropTile)
    {
        cropTile.Sow(cropData);
        tilesSown++;
        if(tilesSown == cropTiles.Count) 
        {
            FieldFullySown();
        }
    }

    private void Water(CropTile cropTile) 
    {
        cropTile.Water(cropData);
        tilesWatered++;
        if(tilesWatered == cropTiles.Count) 
        {
            FieldFullyWatered();
        }
    }

    public void WaterCollidedCallback(Vector3[] waterPositions)
    {
        for (int i = 0; i < waterPositions.Length; i++)
        {
            CropTile closestCropTile = GetClosestCropTile(waterPositions[i]);

            if (closestCropTile == null) continue;

            if (!closestCropTile.IsSown()) continue;

            Water(closestCropTile);
        }

    }

    private void FieldFullySown() 
    {
        state = TileFieldState.Sown;

        onFullySown?.Invoke(this);
    }

    private void FieldFullyWatered()
    {
        state = TileFieldState.Watered;

        onFullyWatered?.Invoke(this);
    }

    private CropTile GetClosestCropTile(Vector3 seedPosition)
    {
        float minDistance = 5000f;
        int closestCropTileIndex = -1;

        for (int i = 0; i < cropTiles.Count; i++)
        {
            CropTile cropTile = cropTiles[i];
            float distanceTileSeed = Vector3.Distance(cropTile.transform.position, seedPosition);

            if (distanceTileSeed < minDistance) 
            {
                minDistance = distanceTileSeed;
                closestCropTileIndex = i;
            }
        }

        if (closestCropTileIndex == -1) return null;

        return cropTiles[closestCropTileIndex];
    }

    public bool IsEmpty()
    {
        return state == TileFieldState.Empty;
    }

    public bool IsSown()
    {
        return state == TileFieldState.Sown;
    }

    public bool IsWatered() 
    {
        return state == TileFieldState.Watered;
    }
}