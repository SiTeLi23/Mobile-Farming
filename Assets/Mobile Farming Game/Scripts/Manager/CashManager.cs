using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CashManager : MonoBehaviour
{
    public static CashManager instance;
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
    [Header("Elements")]
    [SerializeField] private int coins;
    [SerializeField] private Transform coinContainer;
    [SerializeField] private TextMeshProUGUI coinAmountText;
    void Start()
    {
        LoadData();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UseCoins(int amount) 
    {
        AddCoins(-amount);
        if (coins <= 0) 
        {
            coins = 0;
        }
    }
    public void AddCoins(int amount) 
    {
        coins += amount;

        UpdateCoinContainers();
        SaveData();
    }

    public void UpdateCoinContainers() 
    {
        coinAmountText.text = coins.ToString();
    }

    public int GetCoins() 
    {
        return coins;
    }
    [NaughtyAttributes.Button]
    private void Add500Coins() 
    {
        AddCoins(500);
    }

    public void LoadData() 
    {
        coins = PlayerPrefs.GetInt("Coins");
        UpdateCoinContainers();
    }
    public void SaveData() 
    {
        PlayerPrefs.SetInt("Coins", coins);
    }


}
