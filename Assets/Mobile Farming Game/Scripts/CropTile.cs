using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileFieldState { Empty, Sown, Watered }
public class CropTile : MonoBehaviour
{
    private TileFieldState state;

    [Header("Elements")]
    [SerializeField] private Transform cropParent;
    [SerializeField] private MeshRenderer tileRenderer;
    private Crop crop;
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
    }

    public void Water(CropData cropData)
    {
        state = TileFieldState.Watered;

        //change color after water
        tileRenderer.material.color = Color.white * .45f;
        //scale up crop;
        crop.ScaleUp();
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
