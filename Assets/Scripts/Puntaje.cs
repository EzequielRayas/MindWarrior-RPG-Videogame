using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Puntaje : MonoBehaviour
{
    private float puntos;

    private TextMeshProUGUI textMesh; // Referencia al componente de texto

    private void Start()
    {
        textMesh = GetComponent<TextMeshProUGUI>(); // Obtiene el componente de texto

    }

    private void Update()
    {
        puntos += Time.deltaTime; // Incrementa el puntaje con el tiempo
        textMesh.text = puntos.ToString("0"); // Actualiza el texto sin decimales
    }

    public void SumarPuntos(float puntosEntrada)
    {
        puntos += puntosEntrada; // Agrega los puntos recibidos
    //     textMesh.text = puntos.ToString("0"); // Actualiza el texto
     }
}

