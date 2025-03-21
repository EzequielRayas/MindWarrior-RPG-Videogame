using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public enum PlayerState { idle, walk, run, attack }
    public enum PlayerFacing { up, down, left, right }

    public PlayerFacing facing;
    public PlayerState currentState;

    public float moveSpeed = 5f;
    public int attackDamage = 5; // Daño del ataque
    public float attackCooldown = 0.5f; // Tiempo entre ataques
    private float nextAttackTime = 0f;

    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 velocity;

    public int health = 100; // Vida del jugador

    // ✅ Arrastrar la animación de ataque aquí desde Unity
    public AnimationClip attackAnimation;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>(); // Obtener el Animator
        currentState = PlayerState.walk;
        velocity = Vector2.down;
    }

    void Update()
    {
        if (currentState == PlayerState.walk)
        {
            UpdateAnimation();
        }

        velocity = Vector2.zero;
        velocity.x = Input.GetAxisRaw("Horizontal");
        velocity.y = Input.GetAxisRaw("Vertical");

        // ✅ Si presiona Space, ataca
        if (Input.GetKeyDown(KeyCode.Space) && Time.time >= nextAttackTime)
        {
            StartCoroutine(Attack());
            nextAttackTime = Time.time + attackCooldown;
        }
    }

    void UpdateAnimation()
    {
        if (currentState == PlayerState.attack) return; // No moverse si está atacando

        if (velocity != Vector2.zero)
        {
            UpdateMovement();
            animator.SetBool("Running", true);
            animator.SetFloat("Horizontal", velocity.x);
            animator.SetFloat("Vertical", velocity.y);
        }
        else
        {
            animator.SetBool("Running", false);
        }
    }

    void UpdateMovement()
    {
        rb.MovePosition(rb.position + velocity * moveSpeed * Time.fixedDeltaTime);
    }

    // ✅ Función para atacar con la animación asignada
//     private IEnumerator Attack()
// {
//     currentState = PlayerState.attack;

//     animator.Play(attackAnimation.name); // ✅ Reproduce la animación de ataque

//     yield return new WaitForSeconds(attackAnimation.length); // ⏳ Espera el tiempo de la animación

//     animator.Play("Idle"); // ✅ Vuelve a la animación de Idle manualmente
//     currentState = PlayerState.walk; // ✅ Regresar al estado normal
// }


private IEnumerator Attack()
{
    currentState = PlayerState.attack;
    animator.Play(attackAnimation.name); // ✅ Reproduce la animación de ataque

    yield return new WaitForSeconds(0.2f); // ⏳ Esperar antes de detectar el golpe (ajústalo según la animación)

    // ✅ Detectar enemigos en un radio de ataque
    Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, 1f); // 1f = Radio de ataque

    foreach (Collider2D enemy in hitEnemies)
    {
        if (enemy.CompareTag("Enemy")) // Asegúrate de que el `Demon` tenga el Tag "Enemy"
        {
            Demon demon = enemy.GetComponent<Demon>();
            if (demon != null)
            {
                demon.TakeDamage(10); // ✅ Hacerle 10 de daño al Demon (ajusta el valor)
            }
        }
    }

    yield return new WaitForSeconds(attackAnimation.length - 0.2f); // ⏳ Esperar el resto de la animación
    animator.Play("Idle"); // ✅ Volver a Idle después del ataque
    currentState = PlayerState.walk; // ✅ Regresar a estado normal
}


    // ✅ Función para recibir daño
    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log("Vida del jugador: " + health);

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("El jugador ha muerto. Reiniciando nivel...");
        Destroy(gameObject); // ✅ Eliminar al jugador
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // ✅ Reiniciar nivel
     }

     public void Heal(int amount)
{
    health += amount; // Aumenta la vida del jugador
    // if (Health > 100) // Limita la vida máxima a 100
    // {
    //     Health = 100;
    // }
    Debug.Log("Player recuperó " + amount + " puntos de vida. Vida actual: " + health);
}
}
