using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;
    public Animator anim;
    public float pushDistance;
    public int damage = 10;
    public bool arrowFlying;
    public EnemySetup enemy;
    public ManageCharacters character;

    public float ratioRun = 2f;
    public float runSpeed = 1.5f;
    private float walkSpeed = 1.5f;

    public int defense = 50;

    public bool canUseSkill;
    public bool canJump;


    public GameObject arrow;
    public GameObject ballEnergy;
    public Transform arrowPosition;
    private bool arrowDirection;

    public bool emptyMana;
    public SkillManager skillManager;
    public bool isAttacking;

    public UIController gameUIStatus;

    
    void Start()
    {
        runSpeed = walkSpeed * ratioRun;
        Debug.Log(runSpeed);
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        arrowFlying = false;
        canUseSkill = true;
        
        isAttacking = false;

        skillManager = GameObject.Find("SkillManager").GetComponent<SkillManager>();
        gameUIStatus = GameObject.Find("UIController").GetComponent<UIController>();
    }

    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Space) && OnGround() && character.currentCharacterIndex != 0) anim.SetTrigger("Jumping");


        if (Input.GetKeyDown(KeyCode.Q) && !gameUIStatus.gameUIActive && skillManager.CanUseSkill(SkillManager.Skill.FirstSkill) && character.currentMana >= 1.5f)
        {
            FirstSkill(character.currentCharacterIndex);
            Debug.Log("First Skill");
        }
        if (Input.GetKeyDown(KeyCode.W) && !gameUIStatus.gameUIActive && skillManager.CanUseSkill(SkillManager.Skill.SecondSkill) && character.currentMana >= 4f) SecondSkill(character.currentCharacterIndex);
        
        if (Input.GetKeyDown(KeyCode.E) && !gameUIStatus.gameUIActive && skillManager.CanUseSkill(SkillManager.Skill.UltimateSkill) && character.currentMana >= 10f ) LastSkill(character.currentCharacterIndex);
        Moving();
        if (OnGround())
        {
            anim.SetBool("OnGround", true);
        }
    }

    public void Moving()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        
        isRunning = Input.GetKey(KeyCode.LeftShift) && Mathf.Abs(horizontalInput) > 0;

        if (isRunning)
        {
            character.currentStamina -= Time.deltaTime;
            if (character.currentStamina <= 0)
            {
                isRunning = false;
            }
        }
        else
        {
            character.currentStamina += character.staminaRegenRate * Time.deltaTime;
            character.currentStamina = Mathf.Clamp(character.currentStamina, 0, character.maxStamina);
        }

        float currentSpeed = isRunning ? runSpeed : walkSpeed;
        rb.velocity = new Vector2(horizontalInput * currentSpeed, rb.velocity.y);

        anim.SetBool("IsRunning", isRunning);
        anim.SetFloat("IsWalking", Mathf.Abs(horizontalInput));

        FlipCharacter(horizontalInput);
    }

    private void FlipCharacter(float direction)
    {
        if (direction != 0)
        {
            rb.transform.localScale = new Vector3(Mathf.Sign(direction), 1, 1);
            arrowDirection = direction > 0;
        }
    }

    private float jumpForce = 5f;
    private bool isRunning;

    private void Jumping()
    {
        character.currentStamina -= 0.7f;
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
       
    }

    private void NextJump()
    {
        anim.SetBool("OnGround", false);
    }


    public bool OnGround()
    {
        if (Physics2D.BoxCast(transform.position, new Vector2(0.5f, 0.22f), 0, -transform.up, 2f, LayerMask.GetMask("Ground")))  return true; 
        else  return false; 
    }

    

    public void FirstSkill(int index)
    {
        skillManager.UseSkill(SkillManager.Skill.FirstSkill);
        anim.SetTrigger("Attack1");
        if (index == 1) character.currentMana -= 1.5f;
    }

    public void SecondSkill(int index)
    {
        skillManager.UseSkill(SkillManager.Skill.SecondSkill);
        if (index == 1) anim.SetTrigger("Attack2");
    }
    public void ArcherHorizontal()
    {
        skillManager.UseSkill(SkillManager.Skill.SecondSkill);
        character.currentMana -= 4f;

        // Create the arrow at the arrow position
        GameObject arrowInstance = Instantiate(arrow, arrowPosition.position, Quaternion.identity);

        // Set the direction based on player facing
        int direction = arrowDirection ? 1 : -1;
        //arrowInstance.GetComponent<Arrow>().direction;
        // Initialize the arrow's direction
        Arrow arrowScript = arrowInstance.GetComponent<Arrow>();
        if (arrowScript != null)
        {
            arrowScript.Initialize(direction);
        }

        // Flip the arrow sprite based on direction
        Vector3 arrowScale = arrowInstance.transform.localScale;
        arrowScale.x = Mathf.Abs(arrowScale.x) * direction;
        arrowInstance.transform.localScale = arrowScale;
    }

    public void ArcherNoDirection()
    {

    }

    public void BallHorizontal()
    {
        skillManager.UseSkill(SkillManager.Skill.UltimateSkill);
        character.currentMana -= 10f;

        // Create the arrow at the arrow position
        GameObject ballInstance = Instantiate(ballEnergy, arrowPosition.position, Quaternion.identity);

        // Set the direction based on player facing
        int direction = arrowDirection ? 1 : -1;

        // Initialize the arrow's direction
        Arrow arrowScript = ballInstance.GetComponent<Arrow>();
        if (arrowScript != null)
        {
            arrowScript.Initialize(direction);
        }

        // Flip the arrow sprite based on direction
        Vector3 arrowScale = ballInstance.transform.localScale;
        arrowScale.x = Mathf.Abs(arrowScale.x) * direction;
        ballInstance.transform.localScale = arrowScale;
    }

    public void LastSkill(int index)
    {
        skillManager.UseSkill(SkillManager.Skill.UltimateSkill);
        if (index == 1) anim.SetTrigger("Attack3");
    }

    public void TakeDamageHero(int damageValue)
    {
        character.currentHealth -= damageValue;
        anim.SetTrigger("Hurt");
        if (character.currentHealth <= 0)
        {
            Death();
        }
    }

    public void BigDamageValue()
    {
        character.currentHealth = 1;
    }

    public void Death()
    {
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
        anim.SetBool("Death", true);
        rb.constraints = RigidbodyConstraints2D.FreezePositionY;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.GetComponent<NightBorn>().TakeDamage(damage);
        }
    }
}

