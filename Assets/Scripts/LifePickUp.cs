using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifePickUp : MonoBehaviour
{
    public int lifeAmount = 3; // Cantidad de vida que recupera el jugador

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Verifica si el objeto tocado tiene el tag "Player"
        {
            Player player = other.GetComponent<Player>();

            if (player != null)
            {
                player.Heal(lifeAmount); // Llama al método para curar al jugador
                Destroy(gameObject); // Destruye el ítem después de recogerlo
            }
        }
    }
}

