using System;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Scripting.APIUpdating;
public class PlayerBehaviour : MonoBehaviour
{
    Rigidbody2D rb;
    Animator anim;

    public float walkSpeed = 2f;
    public float runSpeed = 4f;
    public float jumpForce = 4f;
    public Transform groundCheck;
    public Transform firstSkillCheck;
    public Transform secondSkillCheck;
    public Transform lastSkillCheck;
    public float firstSkillRadius;
    public LayerMask layerOfGround;
    public GameObject damageTextBox;
    public Transform parentTextBox;

    public GameObject retryGame;
    public BoxCollider2D bodyMain;

    public GameObject[] objects;
    public bool isGrounded;

    //Stats

    public int maxHealth = 100;
    public int currentHealth;
    public int maxMana = 100;
    public int currentMana;
    public float maxStamina = 100;
    public float currentStamina = 6;
    public int defense = 30;
    public int exp;
    public int level;
    public int luck = 20;
    public int potentialSkillPoints = 20;

    //Damage settings
    public int damage = 25;
    public int criticalChance = 30;
    public int buffEffect;
    public Vector2 damageTextPosition;

    public int moveDirection;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        DamageCalculate(criticalChance);
        currentHealth = maxHealth;
    }

    private void Update()
    {
        OnGround();
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded) Jumping();
        if (Input.GetKeyDown(KeyCode.Q)) AttackFirstSkill();
        if (Input.GetKeyDown(KeyCode.W)) AttackSecondSkill();
        if (Input.GetKeyDown(KeyCode.E)) AttackLastSkill();
        
        Moving();
        if (Input.GetKeyDown(KeyCode.J)) Debug.Log(DamageCalculate(criticalChance));
        Debug.Log(rb.velocity.y);

    }

    //public void MoveRight()
    //{
    //    moveDirection = 1;
    //    rb.velocity = new Vector2(walkSpeed, rb.velocity.y);
    //    anim.SetFloat("IsWalking", 1);
    //    Flip(1);
    //}

    //public void MoveLeft()
    //{
    //    moveDirection = -1;
    //    rb.velocity = new Vector2(moveDirection * walkSpeed, rb.velocity.y);
    //    anim.SetFloat("IsWalking", 1);
    //    Flip(-1);
    //}

    //public void MoveStop()
    //{
    //    moveDirection = 0;
    //    rb.velocity = Vector2.zero;
    //    anim.SetFloat("IsWalking", 0);
    //}
    public void Moving()
    {
        float movingInput = Input.GetAxis("Horizontal");
        float typeInput = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;
        
        rb.velocity = new Vector2(movingInput * typeInput, rb.velocity.y);
        
        
        anim.SetFloat("IsWalking", Mathf.Abs(movingInput));
        anim.SetBool("Running", Input.GetKey(KeyCode.LeftShift));

        Flip(movingInput);
    }

    public void Jumping()
    {
        anim.SetTrigger("Jumping");
    }
    public void IsJump()
    {
        if(isGrounded) rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }
    public void EndJump()
    {
        anim.SetTrigger("EndJump");
    }
    void DamageShow(int damage, Vector2 offset)
    {
        GameObject damageText = Instantiate(damageTextBox, parentTextBox);
        var damageShow = damageText.transform.Find("Point").GetComponent<TextMeshProUGUI>();
        var damagePosition = damageText.transform.Find("Point").GetComponent<RectTransform>();
        damagePosition.position = offset;
        damageShow.text = "- " + damage.ToString();
        Destroy(damageText, 1.5f);
    }

    int DamageCalculate(int criticalRate)
    {
        int currentDamage;
        int decreaseCriticalRate = 100 - criticalRate;
        System.Random random = new System.Random();
        int randomCriticalRate = random.Next(1, decreaseCriticalRate);
        int randomCriticalRateValue = randomCriticalRate switch
        {
             <= 60 => 0,
             <= 90 => 10,
            <= 100 => 100,
        };
        return currentDamage = damage + damage * randomCriticalRateValue / 100 + damage * buffEffect;
    }

    public virtual void AttackFirstSkill()
    {
        Debug.Log("Attack1");
    }

    void FirstAttack()
    {
        Collider2D[] attacks = Physics2D.OverlapCircleAll(firstSkillCheck.position, 0.05f, LayerMask.GetMask("Enemy"));

        foreach (var attack in attacks)
        {
            attack.GetComponent<NightBorn>().TakeDamage(DamageCalculate(criticalChance));
            DamageShow(DamageCalculate(criticalChance), attack.transform.position);
        }
    }
    public void AttackSecondSkill()
    {
        rb.velocity = Vector2.zero;
        anim.SetTrigger("Attack2");
        
    }
    void SecondAttack()
    {
        Collider2D[] attacks = Physics2D.OverlapCircleAll(secondSkillCheck.position, 0.05f, LayerMask.GetMask("Enemy"));

        foreach (var attack in attacks)
        {
            attack.GetComponent<NightBorn>().TakeDamage(DamageCalculate(criticalChance));
            DamageShow(DamageCalculate(criticalChance), attack.transform.position);
        }
    }
    public void AttackLastSkill()
    {
        rb.velocity = Vector2.zero;
        anim.SetTrigger("Attack3");
    }
    void LastAttack()
    {
        Collider2D[] attacks = Physics2D.OverlapCircleAll(lastSkillCheck.position, 0.05f, LayerMask.GetMask("Enemy"));

        foreach (var attack in attacks)
        {
            attack.GetComponent<NightBorn>().TakeDamage(DamageCalculate(criticalChance));
            DamageShow(DamageCalculate(criticalChance), attack.transform.position);
        }
    }    
    public void OnGround()
    {
        RaycastHit2D checking = Physics2D.BoxCast(groundCheck.position, new Vector2(0.1f, 0.05f), 0, -transform.up, 0, layerOfGround);

        if (checking) isGrounded = true;
        else isGrounded = false;
        int layerOrder = checking.collider.gameObject.layer;
        //if (checking && layerOrder == 11)
        //{
        //    Physics2D.IgnoreLayerCollision(3, 9, true);
        //    Physics2D.IgnoreLayerCollision(3, 10, true);
        //    Physics2D.IgnoreLayerCollision(3, 11, false);
        //    return true;

        //}
        //else if (checking && layerOrder == 10)
        //{
        //    Physics2D.IgnoreLayerCollision(3, 9, true);
        //    Physics2D.IgnoreLayerCollision(3, 10, false);
        //    Physics2D.IgnoreLayerCollision(3, 11, true);
        //    return true;
        //}
        //else if (checking && layerOrder == 9)
        //{
        //    Physics2D.IgnoreLayerCollision(3, 9, false);
        //    Physics2D.IgnoreLayerCollision(3, 10, true);
        //    Physics2D.IgnoreLayerCollision(3, 11, true);
        //    return true;
        //}
        //else return false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        
    }

    public void Flip(float direction)
    {
        if (direction != 0) rb.transform.localScale = new Vector3(Mathf.Sign(direction)*2, 2, 2);
    }

    public void TakeDamageHero(int damage)
    {
        currentHealth -= damage;
        anim.SetTrigger("Hurt");
        if (currentHealth <= 0) Dead();
    }

    void Dead()
    {
        anim.SetTrigger("Dead");
        retryGame.SetActive(true);
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        this.enabled = false;

    }
}

public class TestPlayer:PlayerBehaviour
{
    public override void AttackFirstSkill()
    {
        base.AttackFirstSkill();
    }
}
