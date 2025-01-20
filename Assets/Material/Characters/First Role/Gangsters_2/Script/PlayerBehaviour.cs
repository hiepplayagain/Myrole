using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

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
    

    public float textCooldownSet = 2f;
    public float textCooldown;
    public Vector2 damageTextPosition;
    //Stats

    public int maxHealth = 100;
    public int currentHealth;
    public int maxMana = 100;
    public int currentMana;
    public float maxStamina = 100;
    public float currentStamina = 6;
    public int damage = 25;
    public int defense = 30;
    public int exp;
    public int level;
    public int luck = 20;
    public int potentialSkillPoints = 20;
    public int criticalDamage = 20;
    public int criticalChance = 30;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && OnGround()) Jumping();
        if (Input.GetKeyDown(KeyCode.Q)) AttackFirstSkill();
        if (Input.GetKeyDown(KeyCode.W)) AttackSecondSkill();
        if (Input.GetKeyDown(KeyCode.E)) AttackLastSkill();

        Moving();
        OnGround();

        
    }

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
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    void DamageShow(int damage, Vector2 offset)
    {
        GameObject damageText = Instantiate(damageTextBox, parentTextBox);
        var damageShow = damageText.transform.Find("Point").GetComponent<TextMeshProUGUI>();
        var damagePosition = damageText.transform.Find("Point").GetComponent<RectTransform>();
        damagePosition.position = offset;
        damageShow.text = "- " + damage.ToString();
        Destroy(damageText, 2f);
    }

    public void AttackFirstSkill()
    {
        rb.velocity = Vector2.zero;
        anim.SetTrigger("Attack1");
    }

    void FirstAttack()
    {
        Collider2D[] attacks = Physics2D.OverlapCircleAll(firstSkillCheck.position, 0.05f, LayerMask.GetMask("Enemy"));

        foreach (var attack in attacks)
        {
            attack.GetComponent<NightBorn>().TakeDamage(damage);
            DamageShow(damage, attack.transform.position);
        }
    }
    public void AttackSecondSkill()
    {
        rb.velocity = Vector2.zero;
        anim.SetTrigger("Attack2");
        
    }
    void SecondAttack()
    {
        bool attack = Physics2D.OverlapCircle(secondSkillCheck.position, 0.05f, LayerMask.GetMask("Enemy"));

        //if (attack) DamageShow(damage);
    }
    public void AttackLastSkill()
    {
        rb.velocity = Vector2.zero;
        anim.SetTrigger("Attack3");
    }
    void LastAttack()
    {
        bool attack = Physics2D.OverlapCircle(lastSkillCheck.position, 0.05f, LayerMask.GetMask("Enemy"));
        //if (attack) DamageShow(damage);
    }    
    public bool OnGround()
    {
        if (Physics2D.BoxCast(groundCheck.position, new Vector2(0.1f, 0.05f), 0, -transform.up, 0, layerOfGround)) return true;
        else return false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        
    }

    public void Flip(float direction)
    {
        if (direction != 0) rb.transform.localScale = new Vector3(Mathf.Sign(direction)*2, 2, 2);
    }
}
