using System.Collections;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;

    public float speed = 3f;
    private int direction = 1;
    public int damage;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        //rb.velocity = new Vector2(speed * direction, 0); // Use Rigidbody for movement
    }

    private void Update()
    {
        rb.velocity = new Vector2(speed * direction, 0); // Use Rigidbody for movement
    }

    public void Initialize(int index)
    {
        direction = index; // Direction should be 1 or -1
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Enemy"))
        {
            HandleCollision();
            collision.GetComponent<NightBorn>().TakeDamage(damage);
        }
    }

    private void HandleCollision()
    {
        anim.SetTrigger("Collided");
        rb.constraints = RigidbodyConstraints2D.FreezePositionX;
        StartCoroutine(DestroyAfterAnimation());
    }

    private IEnumerator DestroyAfterAnimation()
    {
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        Destroy(gameObject);
    }
}
