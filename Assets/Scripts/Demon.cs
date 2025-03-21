using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Demon : MonoBehaviour
{
    public int health = 30; // Vida del Demon
    public int damage = 5; // Daño al jugador
    public float attackCooldown = 0.5f; // Tiempo entre ataques
    public float moveSpeed = 1.5f; // Velocidad normal
    public float fastSpeed = 4f; // Velocidad rápida
    public float circleRadius = 2f; // Radio del movimiento circular

    private float nextAttackTime = 0f;
    private float angle = 0f;
    private Vector2 startPosition;
    private int movementType = 0; // Tipo de movimiento actual

    void Start()
    {
        startPosition = transform.position;
        StartCoroutine(ChangeMovementPattern()); // Cambiar el patrón de movimiento aleatoriamente
    }

    void Update()
    {
        switch (movementType)
        {
            case 0:
                MoveInCircle();
                break;
            case 1:
                MoveSideToSide();
                break;
            case 2:
                MoveBackward();
                break;
            case 3:
                MoveFastRandom();
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                DamagePlayer(player);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && Time.time >= nextAttackTime)
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                DamagePlayer(player);
                nextAttackTime = Time.time + attackCooldown;
            }
        }
    }

    private void DamagePlayer(Player player)
    {
        player.TakeDamage(damage);
        Debug.Log("El jugador ha recibido " + damage + " de daño. Vida restante: " + player.health);
    }

    // 🔄 Movimiento en círculos
    private void MoveInCircle()
    {
        angle += moveSpeed * Time.deltaTime;
        float x = Mathf.Cos(angle) * circleRadius;
        float y = Mathf.Sin(angle) * circleRadius;
        transform.position = new Vector2(startPosition.x + x, startPosition.y + y);
    }

    // ↔️ Movimiento de lado a lado
    private void MoveSideToSide()
    {
        float x = Mathf.PingPong(Time.time * moveSpeed, 4) - 2; // Movimiento en un rango de -2 a 2
        transform.position = new Vector2(startPosition.x + x, transform.position.y);
    }

    // 🔙 Movimiento de retroceso
    private void MoveBackward()
    {
        transform.position = new Vector2(transform.position.x, startPosition.y - Mathf.PingPong(Time.time * moveSpeed, 2));
    }

    // ⚡ Movimiento rápido sorpresa
    private void MoveFastRandom()
    {
        transform.position += (Vector3)(Random.insideUnitCircle * fastSpeed * Time.deltaTime);
    }

    // 🎲 Cambia el patrón de movimiento cada 3-6 segundos
    private IEnumerator ChangeMovementPattern()
    {
        while (true)
        {
            movementType = Random.Range(0, 4); // Elige un tipo de movimiento aleatorio (0 a 3)
            yield return new WaitForSeconds(Random.Range(3f, 6f)); // Espera entre 3 y 6 segundos antes de cambiar
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log("Demon recibió " + damage + " de daño. Vida restante: " + health);

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Demon ha sido derrotado.");
        Destroy(gameObject);
    }
}
