using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cat : MonoBehaviour
{
    public float moveSpeed = 2f; // Velocidad del gato
    public float followDistance = 1.5f; // Distancia mínima antes de seguir al jugador
    public float stopDistance = 0.5f; // Distancia en la que el gato se detiene cerca del jugador
    public float roamRadius = 3f; // Radio dentro del cual el gato puede moverse aleatoriamente
    public float idleTime = 0.7f; // Tiempo en segundos antes de que el gato deje de seguir

    private Rigidbody2D rb;
    private Animator animator;
    private Transform player;
    private Vector2 movement;
    private bool isFollowing = false;
    private Vector2 roamTarget;
    private float playerStopTimer = 0f;
    private Vector2 lastPlayerPosition;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        // Encuentra al jugador
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
            lastPlayerPosition = player.position;
        }
        else
        {
            Debug.LogError("No se encontró el objeto del jugador. Asegúrate de que tiene el tag 'Player'.");
        }

        // Inicia el movimiento aleatorio
        InvokeRepeating(nameof(ChooseRoamTarget), 0, 3f);
    }

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // Verificar si el jugador está quieto
        if (Vector2.Distance(player.position, lastPlayerPosition) < 0.01f)
        {
            playerStopTimer += Time.deltaTime;
        }
        else
        {
            playerStopTimer = 0f;
            lastPlayerPosition = player.position;
        }

        if (playerStopTimer >= idleTime)
        {
            // Si el jugador no se mueve por 2 segundos, el gato se detiene
            isFollowing = false;
            movement = Vector2.zero;
        }
        else if (distanceToPlayer > followDistance)
        {
            // Si el jugador está lejos, sigue al jugador
            isFollowing = true;
            movement = (player.position - transform.position).normalized;
        }
        else if (distanceToPlayer <= stopDistance)
        {
            // Si el gato está muy cerca del jugador, se detiene completamente
            isFollowing = false;
            movement = Vector2.zero;
        }
        else
        {
            // Si está dentro del rango pero no tan cerca, deambula aleatoriamente
            isFollowing = false;
            movement = (roamTarget - (Vector2)transform.position).normalized;

            if (Vector2.Distance(transform.position, roamTarget) < 0.5f)
            {
                ChooseRoamTarget(); // Elegir un nuevo punto de deambulación
            }
        }

        // Actualizar animaciones
        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetBool("Walking", movement.magnitude > 0);
    }

    void FixedUpdate()
    {
        rb.velocity = movement * moveSpeed;
    }

    void ChooseRoamTarget()
    {
        if (!isFollowing)
        {
            roamTarget = (Vector2)transform.position + new Vector2(
                Random.Range(-roamRadius, roamRadius),
                Random.Range(-roamRadius, roamRadius)
            );
        }
    }
}
