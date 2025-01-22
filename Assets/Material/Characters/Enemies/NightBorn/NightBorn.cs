using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class NightBorn : MonoBehaviour
{
    Rigidbody2D rb;
    public Animator anim;
    public float speed = 4f;
    public float direction = 1;
    public Transform checkGround;
    public float lengCheck;
    public float playerCheck;
    public float speedSet;
    public float idleTime;
    public float idleTimeSet = 2f;
    public bool isIdling = false;

    public Transform attackArea;
    public float attackCoolDown;
    public float attackCoolDownDefault = 2f;
    public bool isDead = false;

    public Vector3[] offset;
    public LayerMask groundLayer;
    public float radius;

    //public Canvas dialogueCanvas;
    //public TMP_Text dialogueShowText;

    //[SerializeField]
    //private string[] dialogueText;
    //public float speedTyping = 0.1f;
    //public int textIndex = 0;

    //stats
    public int damage = 20;
    public int maxHealth = 100;
    public int currentHealth;
    public int exp = 100;


    public Image healthBar;
    //public GameObject dialoguaBox;


    void Start()
    {
        //dialoguaBox.SetActive(false);
        //StartCoroutine(TypeText(dialogueText[0]));
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        speedSet = speed;
        attackCoolDown = 0;
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        //dialogueCanvas.transform.position = transform.position + offset[1];
        if (isIdling)
        {
            idleTime -= Time.deltaTime;
            if (idleTime <= 0)
            {
                speed = speedSet;
                isIdling = false;
                direction *= -1;
                Flip(direction);
            }
        }
        else
        {
            Patrolling();
        }
        DetectPlayer();
        
    }

    public void Patrolling()
    {
        RaycastHit2D hit = Physics2D.Raycast(checkGround.position, Vector2.down, lengCheck, groundLayer);
        if (hit.collider == null)
        {
            idleTime = idleTimeSet;
            isIdling = true;
            anim.SetBool("Patrolling", false);

            rb.velocity = Vector2.zero;
        }
        else
        {
            rb.velocity = new Vector2(speed * direction, rb.velocity.y);
            anim.SetBool("Patrolling", true);
        }
    }

    void Flip(float index)
    {
        rb.transform.localScale = new Vector3(index * 4, 4, 4);
    }

    public void DetectPlayer()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position + offset[0], Vector2.right * direction, playerCheck, LayerMask.GetMask("Player"));
        if (hit.collider != null)
        {
            rb.velocity = Vector2.zero;

            if (attackCoolDown <= 0)
            {
                anim.SetTrigger("Attack");
                attackCoolDown = attackCoolDownDefault;
                
            }
            else
            {
                anim.SetBool("Patrolling", false);
                attackCoolDown -= Time.deltaTime;
                
            }

        }
    }

    private void AttackArea()
    {
        Collider2D[] attack = Physics2D.OverlapCircleAll(attackArea.position, radius, LayerMask.GetMask("Player"));
        foreach (Collider2D player in attack)
        {
            player.GetComponent<PlayerBehaviour>().TakeDamageHero(damage);
        }
    }
    

    private void OnDrawGizmos()
    {
        if (checkGround == null) return;
        Gizmos.color = Color.red;

        Gizmos.DrawRay(checkGround.position, Vector2.down * lengCheck);
        Gizmos.DrawRay(transform.position + offset[0], Vector2.right * playerCheck * direction);
        Gizmos.DrawWireSphere(attackArea.position, radius);
    }

    //private IEnumerator TypeText(string text)
    //{
    //    dialogueShowText.text = "";
    //    foreach (char letter in text.ToCharArray())
    //    {
    //        dialogueShowText.text += letter;
    //        yield return new WaitForSeconds(speedTyping);

    //    }
    //    if (dialogueShowText.text == text)
    //    {
    //        yield return new WaitForSeconds(2f);
    //        dialoguaBox.SetActive(false);
    //        yield return new WaitForSeconds(2f);
    //        dialoguaBox.SetActive(true);
    //        dialogueShowText.text = "";
    //        textIndex++;
    //        if (textIndex < dialogueText.Length)
    //        {
    //            StartCoroutine(TypeText(dialogueText[textIndex]));
    //        }
    //        else
    //        {
    //            textIndex = 0;
    //            StartCoroutine(TypeText(dialogueText[textIndex]));
    //        }
    //    }
    //}
    public void TakeDamage(int damage)
    {
        anim.SetTrigger("Hurt");
        currentHealth -= damage;
        healthBar.fillAmount = (float)currentHealth / maxHealth;
        if (currentHealth <= 0)
        {
            AutoExpolusion();
        }
    }

    public void AutoExpolusion()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        isDead = true;
        anim.SetBool("Dead", true);
        StartCoroutine(Epolusion());
    }

    private IEnumerator Epolusion()
    {
        yield return new WaitForSeconds(2.22f);
        Destroy(gameObject);
    }
}



