using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


//Keep track of coin count, support spirits, items of player
public class PlayerManager : MonoBehaviour
{

    public static PlayerManager instance;
    public int coinCount = 0;
    public TextMeshProUGUI coinText;

    private HashSet<int> collectedItems = new HashSet<int>(); //collected items
    private HashSet<int> purchasedItems = new HashSet<int>(); //purchased items

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); //keep
        }
        else
        {
            Destroy(gameObject);
        }

    }

    public void UpdateCoinCount(int amount)
    {
        coinCount += amount;
        UpdateCoinUI();
    }

    private void UpdateCoinUI()
    {
        coinText.text = "x " + coinCount.ToString();
    }

    //-----Item Management----------------------

    public void CollectItem(int itemID)
    {
        if (!collectedItems.Contains(itemID))
        {
            collectedItems.Add(itemID);
            Debug.Log("Collected Item ID: " + itemID);
        }
    }

    public void BuyItem(int itemID, int price)
    {
        if (coinCount >= price && !purchasedItems.Contains(itemID))
        {
            coinCount -= price;
            purchasedItems.Add(itemID);
            UpdateCoinUI();
            Debug.Log("Purchased Item ID: " + itemID);
        }
        else
        {
            Debug.Log("Not enough coins or item already purchased.");
        }
    }

}
