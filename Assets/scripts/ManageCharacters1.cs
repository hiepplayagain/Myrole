using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cinemachine;

public class ManageCharacters : MonoBehaviour
{
    //switch cooldown
    public float currentTimeforSwitch;
    private float timeForSwitch = 2f;
    public Image switchCD;
    private bool canSwitch = true;

    //Status management

    public float maxHealth = 100;
    public float currentHealth;
    public float maxMana = 20;
    public float currentMana;
    public float maxStamina = 5;
    public float currentStamina;
    public float staminaRegenRate = 1;
    public float luck = 10;

    public  float currentExperience;
    public float requiredExperience = 100;


    public int level;

    public TMP_Text levelDisplay;

    public Image healthBar;
    public Image manaBar;
    public Image staminaBar;
    public Image experienceCircle;
 

    // References for character switching and camera control
    public CinemachineVirtualCamera camFollow;
    public GameObject[] character;
    public GameObject currentCharacter;
    public int currentCharacterIndex = 0;

    [System.Serializable]
    public class SkillPoints
    {
        public string skillPointName;
        public int skillPointPresent;
        
        public TMP_Text skillPointText;
        public Button addButton;
        public Button subtractButton;
    }

    public SkillPoints[] skillPointName;
    public int totalSkillPoints = 20;
    public int hadSkillPoints;
    public int maxSkillPoint = 20;
    public int defaultPoints = 0;

    public TMP_Text showHadPoints; 
    public TMP_Text levelProfile;


    void Start()
    {
        currentHealth = maxHealth;
        currentMana = maxMana;
        currentStamina = maxStamina;
        currentCharacter = character[0];
        currentCharacter.SetActive(true);
        camFollow.Follow = currentCharacter.transform;
        camFollow.LookAt = currentCharacter.transform;
        currentExperience = 0;
        level = 0;
        levelDisplay.text = level.ToString();

        hadSkillPoints = totalSkillPoints;
        showHadPoints.text = "Điểm: " + hadSkillPoints.ToString();

        for (int i = 0; i < skillPointName.Length; i++)
        {
            int index = i;
            skillPointName[i].skillPointPresent = defaultPoints;
            skillPointName[i].skillPointText.text = skillPointName[index].skillPointPresent.ToString();
            skillPointName[i].addButton.onClick.AddListener(() => AddSkillPoint(index));
            skillPointName[i].subtractButton.onClick.AddListener(() => SubtractSkillPoint(index));
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && canSwitch)
        {
            currentTimeforSwitch = timeForSwitch;
            canSwitch = false;
            SwitchCharacter();
        }


        if (!canSwitch)
        {
            currentTimeforSwitch -= Time.deltaTime;

            switchCD.fillAmount = currentTimeforSwitch / timeForSwitch;

            if (currentTimeforSwitch < 0) canSwitch = true;
        }
        StatusUpdate();

        if (currentExperience >= requiredExperience)
        {
            LevelUp();
        }
        levelProfile.text = level.ToString();

    }

    private void SwitchCharacter()
    {
        //Check 
        if (currentCharacterIndex > character.Length - 2) currentCharacterIndex = 0;
        else currentCharacterIndex ++;
        Vector3 previousPosition = currentCharacter.transform.position;

        currentCharacter.SetActive(false);

        currentCharacter = character[currentCharacterIndex];

        character[currentCharacterIndex].SetActive(true);

        currentCharacter.transform.position = previousPosition;
        
        camFollow.Follow = currentCharacter.transform;
        camFollow.LookAt = currentCharacter.transform;
    }


    public void StatusUpdate()
    {
        healthBar.fillAmount = currentHealth / maxHealth;
       
        manaBar.fillAmount = currentMana / maxMana;
        staminaBar.fillAmount = currentStamina / maxStamina;

        experienceCircle.fillAmount = currentExperience / requiredExperience;
    }

    public void LevelUp()
    {
        level++;
        currentExperience = 0;
        requiredExperience *= 1.5f;
        levelDisplay.text = level.ToString();
        hadSkillPoints += 4;
        showHadPoints.text = "Điểm: " + hadSkillPoints.ToString();
    }

    public void AddSkillPoint(int index)
    {
        if (hadSkillPoints > 0 && skillPointName[index].skillPointPresent < maxSkillPoint)
        {
            hadSkillPoints--;
            skillPointName[index].skillPointPresent++;
            skillPointName[index].skillPointText.text = skillPointName[index].skillPointPresent.ToString();
            showHadPoints.text = "Điểm: " + hadSkillPoints.ToString();
        }
    }

    public void SubtractSkillPoint(int index)
    {
        if (skillPointName[index].skillPointPresent > defaultPoints)
        {
            hadSkillPoints++;
            skillPointName[index].skillPointPresent--;
            skillPointName[index].skillPointText.text = skillPointName[index].skillPointPresent.ToString();
            showHadPoints.text = "Điểm: " + hadSkillPoints.ToString();
        }
    }

   
}
