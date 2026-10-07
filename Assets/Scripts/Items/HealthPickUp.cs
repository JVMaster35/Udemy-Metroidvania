using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickUp : MonoBehaviour
{
    public int healthToGive;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            Player.instance.RestoreHealth(healthToGive);
            AudioManager.instance.PlayUiSFX(3);
            Destroy(gameObject);
        }
    }
}
