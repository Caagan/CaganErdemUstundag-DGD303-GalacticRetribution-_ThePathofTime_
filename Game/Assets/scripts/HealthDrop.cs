using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthDrop : MonoBehaviour
{
    // Start is called before the first frame update
    public int healthAmount = 20;  // Bu miktar, health objesinin oyuncunun can�n� ne kadar art�raca��n� belirler

    private void OnTriggerEnter2D(Collider2D other)
    {
        // E�er karakterle �arp��ma olursa ve karakterin tag'i "Player" ise
        if (other.CompareTag("SpaceShip"))
        {
            // Sa�l�k miktar�n� art�r
            other.GetComponent<SpaceShipScript>().IncreaseHealth(healthAmount);

            // Sa�l�k objesini yok et
            Destroy(gameObject);
        }
    }
}
