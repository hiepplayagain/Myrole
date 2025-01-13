using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    // Start is called before the first frame update

    public enum Skill
    {
        FirstSkill,
        SecondSkill,
        UltimateSkill
    }

    private Dictionary<Skill, float> cooldowns = new(); //Create a dictionary to store the cooldowns of each skill
    public Dictionary<Skill, float> SkillCooldownDuration = new() //Create a dictionary to store the duration of each skill
    {
        {Skill.FirstSkill, 1f},
        {Skill.SecondSkill, 5f},
        {Skill.UltimateSkill, 15f}
    };

    
    public Dictionary < Skill, string> Animations = new()
    {
        {Skill.FirstSkill, "Attack1"},
        {Skill.SecondSkill, "Attack2"},
        {Skill.UltimateSkill, "Attack3"}
    };

    void Start()
    {
        foreach (var skill in SkillCooldownDuration.Keys) //Set the initial cooldowns of each skill to 0
        {
            cooldowns[skill] = 0f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var skill in cooldowns.Keys) //Decrease the cooldown of each skill by the time passed since the last frame
        {
            if (cooldowns[skill] > 0) cooldowns[skill] -= Time.deltaTime;
        }
    }
    public bool CanUseSkill(Skill skill) //Check if a skill can be used
    {
        return cooldowns[skill] <= 0;
    }

    public void UseSkill(Skill skill)
    {
        if (CanUseSkill(skill)) cooldowns[skill] = SkillCooldownDuration[skill]; //Set the cooldown of the skill to the duration of the skill
    }
}
