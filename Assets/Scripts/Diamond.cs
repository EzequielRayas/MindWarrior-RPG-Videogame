
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Diamond : MonoBehaviour
{
    [SerializeField] private float cantidadPuntos = 50; // Puntos que otorga la pizza
    [SerializeField] private Puntaje puntaje; // Referencia al script de puntaje

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Si el jugador toca la pizza
        {
            puntaje.SumarPuntos(cantidadPuntos); // Suma los puntos al marcador
             Destroy(gameObject);    }
    }
}
