using UnityEngine;

public class Coin : MonoBehaviour
{
    public string coinID; 

    private void Start()
    {
        if (CoinManager.Instance == null)
        {
            return;
        }

        if (CoinManager.Instance.IsCoinCollected(coinID))
        {
            Destroy(gameObject);
        }
    }
}
