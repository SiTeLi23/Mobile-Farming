using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

[RequireComponent(typeof(ChunkWalls))]
public class Chunk : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private GameObject unlockedElement;
    [SerializeField] private GameObject lockedElement;
    [SerializeField] private TextMeshPro priceText;
    [SerializeField] private MeshFilter chunkFilter;
    private ChunkWalls chunkWalls;

    [Header("Settings")]
    [SerializeField] private int initialPrice;
    private int currentPrice;
    private bool unlocked;
    private int configuration;

    [Header("Actions")]
    public static Action onUnlocked;
    public static Action onPriceChanged;

    private void Awake()
    {
        chunkWalls = GetComponent<ChunkWalls>();
    }
    void Start()
    {

    }

    void Update()
    {
        
    }

    public void Initialized(int loadedPrice)
    {
        currentPrice = loadedPrice;
        priceText.text = currentPrice.ToString();

        if (currentPrice <= 0) 
        {
            Unlock(false);
        }

    }

    public void TryUnlock() 
    {
        if (CashManager.instance.GetCoins() < currentPrice) return;
        currentPrice--;
        CashManager.instance.UseCoins(1);
        priceText.text = currentPrice.ToString();

        onPriceChanged?.Invoke();

        if(currentPrice <= 0) 
        {
            Unlock();
        }
    }

    private void Unlock(bool triggerAction = true) 
    {
        unlockedElement.SetActive(true);
        lockedElement.SetActive(false);
        unlocked = true;

        if (triggerAction)
        {
            onUnlocked?.Invoke();
        }
    }

    public void UpdateWalls(int configuration) 
    {
        this.configuration = configuration;
        chunkWalls.Configure(configuration);
    }

    public void DisplayLockedElements() 
    {
        lockedElement.SetActive(true);
    }

    public void SetRenderer(Mesh chunkMesh)
    {
        chunkFilter.mesh = chunkMesh;
    }
    public bool IsUnlocked() 
    {
        return unlocked;
    }

    public int GetInitialPrice() 
    {
        return initialPrice;
    }

    public int GetCurrentPrice()
    {
        return currentPrice;
    }


}
