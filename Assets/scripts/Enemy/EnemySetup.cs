using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemySetup : MonoBehaviour
{
    Animator anim;
    //Patrolling
    public Transform[] patrolPoints;
    private int currentPatrolIndex = 1;
    private float patrolSpeed = 2f;
    private float direction = 1;

    private float idleTime = 2.5f;
    private float currentIdleTime;
    private bool isIdle = false;

    public bool isChasing;
    public Transform player;
    public float distance;

    public Rigidbody2D rb;

    public ManageCharacters playerPosition;
    public bool isPatrolling;
    public int damage = 10;

    public float chaseSpeed = 2.5f;
    //Status
    public int maxHealth = 100;
    public int currentHealth;

    public EnemyHealthBar healthBar;
    void Start()
    {
        isPatrolling = true;

        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        currentHealth = maxHealth;
        healthBar.SetHealth(currentHealth, maxHealth);
    }

    public void Update()
    {
        distance = Mathf.Abs(transform.position.x - player.transform.position.x);
        Debug.Log(distance);

        if (distance > 4f)
        {
            // Player is out of detection range, resume patrolling
            isChasing = false;
            isPatrolling = true;
            anim.SetBool("Detected", false);
        }
        if (distance > 1.5f && distance <= 4f)
        {
            // Player is within detection range, start chasing
            isChasing = true;
            isPatrolling = false;
            anim.SetBool("Detected", true);
        }
        if (distance < 1.5f) Attack();
        if (distance <= 1f)
        {
            // Player is very close, stop chasing
            isChasing = false;
            isPatrolling = false;
            anim.SetBool("Detected", false);
            anim.SetTrigger("Attack");
        }

        if (isPatrolling) Patrolling();

        if (isChasing) Chasing();
    }

    public void Patrolling()
    {

        if (isIdle)
        {
            // Enemy đang dừng, đếm thời gian chờ
            currentIdleTime -= Time.deltaTime;
            anim.SetBool("IsIdling", true);
            if (currentIdleTime <= 0)
            {
                // Hết thời gian chờ, bắt đầu di chuyển
                isIdle = false;
                currentIdleTime = 0f;
                anim.SetBool("IsIdling", false);

                // Lật hướng enemy trước khi đi tiếp
                FlipEnemy(direction);
            }

            return; // Không di chuyển khi đang idle
        }

        Transform targetPosition = patrolPoints[currentPatrolIndex];
        rb.velocity = new Vector2(direction * patrolSpeed, rb.velocity.y);
        float distance = targetPosition.position.x - transform.position.x;

        if (Mathf.Abs(distance) <= 0.1f)
        {
            isIdle = true;
            direction *= -1;
            currentPatrolIndex = direction == 1 ? 1 : 0;
            currentIdleTime = idleTime;
        }
    }

    public void Idling()
    {

    }

    public void FlipEnemy(float index)
    {
        rb.transform.localScale = new Vector3(index, 1, 1);
    }

    public void Chasing()
    {
        if (transform.position.x < player.transform.position.x)
        {
            rb.velocity = Vector2.right*chaseSpeed;
            FlipEnemy(1f);
        }
        else if (transform.position.x > player.transform.position.x)
        {
            rb.velocity = Vector2.left*chaseSpeed;
            FlipEnemy(-1f);
        }
        
    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.left * 4f);
    }

    public void PushEnemy()
    {
        rb.transform.position = new Vector2(transform.position.x + .5f, transform.position.y);
    }

    void Death()
    {
        playerPosition.currentExperience += 10;
        anim.SetBool("Attack1", false);

        anim.SetBool("IsDead", true);
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
        rb.constraints = RigidbodyConstraints2D.FreezePositionY;
        rb.velocity = Vector3.zero;
    }
    public void TakeDamageEnemy()
    {
        healthBar.SetHealth(currentHealth, maxHealth);
        anim.SetTrigger("hurtMonster");
        if (playerPosition.currentCharacterIndex == 1) PushEnemy();
        if (currentHealth <= 0)
        {
            Death();
        }
    }

    public void Attack()
    {
        anim.SetTrigger("Attack1");
        anim.SetBool("Detected", false);
        anim.SetBool("IsIdling", false);

        rb.velocity = Vector2.zero;
    }
}

