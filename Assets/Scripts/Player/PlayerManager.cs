using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


//Keep track of coin count, support spirits, items of player
public class PlayerManager : MonoBehaviour
{
    public int coinCount = 0;
    public int hpCount = 3;
    public TextMeshProUGUI coinText;

    private HashSet<int> collectedItems = new HashSet<int>(); //collected items
    private HashSet<int> purchasedItems = new HashSet<int>(); //purchased items

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
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

    //Test methods
    public void BuyItemPotion()
    {
        BuyItem(2, 10);
    }

    public void BuyItemArmor()
    {
        BuyItem(1, 10);
    }

    public List<int> GetPurchasedItems()
    {
        return new List<int>(purchasedItems);
    }



}
