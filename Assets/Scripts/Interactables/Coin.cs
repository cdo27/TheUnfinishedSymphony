using UnityEngine;

public class Coin : MonoBehaviour
{
    public string coinID; 

    private void Start()
    {
        if (CoinManager.Instance.IsCoinCollected(coinID))
        {
            Destroy(gameObject);
        }
    }
}
