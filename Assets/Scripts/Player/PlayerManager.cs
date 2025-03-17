using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


//Keep track of coin count, support spirits, items of player
public class PlayerManager : MonoBehaviour
{
     public static PlayerManager Instance;
    public int coinCount = 0;
    public int hpCount = 3;
    public TextMeshProUGUI coinText;

    private HashSet<int> ownedItems = new HashSet<int>(); 

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(Instance.gameObject);
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }


    public void UpdateCoinCount(int amount)
    {
        if (ownedItems.Contains(6))
        {
            int randomChance = Random.Range(0, 2);
            if (randomChance == 1)
            {
                amount *= 2;
            }
        }

        coinCount += amount;

        UpdateCoinUI();
    }


    private void UpdateCoinUI()
    {
        if (coinText != null) coinText.text = "x " + coinCount.ToString();
    }

    //-----Item Management----------------------

    public void AddOwnedItem(int itemID)
    {
        if (!ownedItems.Contains(itemID))
        {
            ownedItems.Add(itemID);
            Debug.Log("Owned Item ID: " + itemID);
        }
    }

    public void BuyItem(int itemID, int price)
    {
        if (coinCount >= price && !ownedItems.Contains(itemID))
        {
            coinCount -= price;
            ownedItems.Add(itemID);
            UpdateCoinUI();
            Debug.Log("Purchased Item ID: " + itemID);
        }
        else
        {
            Debug.Log("Not enough coins or item already owned.");
        }
    }

    //Test methods
    public void BuyItemPotion()
    {
        BuyItem(1, 5);
    }

    public void BuyItemArmor()
    {
        BuyItem(2, 6);
    }
    public void BuyItemShield()
    {
        BuyItem(4, 10);
    }

    public void BuyItemWeapon()
    {
        BuyItem(5, 6);
    }

    

    public List<int> GetOwnedItems()
    {
        return new List<int>(ownedItems);
    }



}
