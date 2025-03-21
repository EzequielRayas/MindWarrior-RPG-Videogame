using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarraVida : MonoBehaviour
{
    public Image rellenoBarraVida;
    private Player Player;
    private float vidaMaxima;
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player")?.GetComponent<Player>();

if (Player == null)
{
    Debug.LogError("No se encontró PlayerController en el objeto con tag 'Player'.");
} vidaMaxima = Player.health;
    }

    // Update is called once per frame
    void Update()
    {
        rellenoBarraVida.fillAmount = Player.health / vidaMaxima;
    }
}