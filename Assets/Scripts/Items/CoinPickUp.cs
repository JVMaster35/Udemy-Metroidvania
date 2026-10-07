using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickUp : MonoBehaviour
{
    public int coinValue;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            GameManager.instance.GetCoins(coinValue);
            AudioManager.instance.PlayUiSFX(1);
            Destroy(gameObject);
        }
    }
}
