using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTakeDamage : MonoBehaviour
{
    public EnemySetup enemy;
    public ManageCharacters player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            player.currentHealth -= enemy.damage;
            collision.GetComponent<PlayerController>().TakeDamageHero(enemy.damage);
        }
    }

}
