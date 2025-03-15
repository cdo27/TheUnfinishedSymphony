using System.Collections.Generic;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance;

    private HashSet<string> collectedCoins = new HashSet<string>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(Instance.gameObject);
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void CollectCoin(string coinID)
    {
        if (!collectedCoins.Contains(coinID))
        {
            collectedCoins.Add(coinID);
            Debug.Log($"Collected coin: {coinID}");
        }
    }

    public bool IsCoinCollected(string coinID)
    {
        return collectedCoins.Contains(coinID);
    }
}
