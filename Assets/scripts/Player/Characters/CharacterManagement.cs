using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerClass", menuName = "Characters")]
public class ClassCharacters : ScriptableObject
{
    //Character Stats
    public string characterName;
    public int currentHealth,maxHealth;
    public int currentMana, maxMana;
    public float currentStamina, maxStamina;
    public int damage ,defense;
    public float speed;
    public int luck;
    public int level;
    public int exp;

}