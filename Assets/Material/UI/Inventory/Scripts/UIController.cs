using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System;

public class UIController : MonoBehaviour
{
    //Open and close the game UI, switch between tabs
    public GameObject gameUI;
    public bool gameUIActive = false;
    public GameObject[] iconSideBar;
    public GameObject[] tabGame;
    public bool tabGameActive = false;

    public Image keyE;
    public Image keyQ;
    public Sprite[] keySwitch;

    public bool subUIStatus = false;
    public Animator anim;
    int i = 0;

    //Edit User's name
    public GameObject setNameBox;
    public bool setNameBoxActive = false;

    public TMP_Text showData;
    public TMP_Text getData;

    //Stats
    public TMP_Text[] statsIndex;
    public TMP_Text[] skillsIndex;
    public ManageCharacters Characters;
    public PlayerController player;
    public TMP_Text[] informationBox;
    public Image itemSelectedIcon;

    public Sprite selectedSprite;
    public Button button;

    //Select item
    public Transform parentObject; // Parent object (drag vào trong Inspector)
    private int selectedIndex = 0;
    public Sprite[] selectedImage;
    // Start is called before the first frame update
    void Start()
    {
        tabGame[0].SetActive(true);
        iconSideBar[0].transform.position = new Vector3(-640f, 233f, 0);
        
        iconSideBar[0].gameObject.GetComponent<Image>().sprite = keySwitch[4];
        iconSideBar[0].GetComponent<RectTransform>().localScale = new Vector3(2, 2, 2);

        gameUI.SetActive(false);

        SelectChild(selectedIndex);
        //button.onClick.AddListener(UpdateStatsNow);
    }

    

    // Update is called once per frame
    void Update()
    {
        //Show or hide the game UI
        if (Input.GetKeyDown(KeyCode.B)  || Input.GetKeyDown(KeyCode.Escape))
        {
            gameUIActive = !gameUIActive;
            
            if (gameUIActive)
            {
                anim.SetTrigger("Open");
                gameUI.SetActive(true);
                Time.timeScale = 0;
                UpdateStats();
            }
            else
            {
                //anim.enabled = true;
                anim.SetTrigger("Close");
                Time.timeScale = 1;
                StartCoroutine(DeplayForAnimation());
            }
        }
        //Switching between tabs
        if (Input.GetKeyDown(KeyCode.E ) && !setNameBoxActive && gameUIActive)
        {
            keyE.sprite = keySwitch[1];
            SetIconSideBar(-1);
            
        }
        if (Input.GetKeyUp(KeyCode.E)) keyE.sprite = keySwitch[0];

        if (Input.GetKeyDown(KeyCode.Q) && !setNameBoxActive && gameUIActive)
        {
            keyQ.sprite = keySwitch[3];
            SetIconSideBar(1);
        }
        if (Input.GetKeyUp(KeyCode.Q)) keyQ.sprite = keySwitch[2];

        //Moving of the item selection
        if (Input.GetKeyDown(KeyCode.A) && gameUIActive)
        {
            ChangeSelection(-1);
        }
        if (Input.GetKeyDown(KeyCode.D) && gameUIActive)
        {
            ChangeSelection(1);
        }
        if (Input.GetKeyDown(KeyCode.W) && gameUIActive)
        {
            ChangeSelection(-4);
        }
        if (Input.GetKeyDown(KeyCode.S) && gameUIActive)
        {
            ChangeSelection(4);
        }

    }

    public void SetGameUIStatus()
    {
        Time.timeScale = 1;
        gameUIActive = false;
        gameUI.SetActive(false);
    }
    public void SetIconSideBar(int index)
    {
        int previousIndex = i;
        i += index;

        //Check if the index is out of range and set it to the first or last in the list icons
        if (i < 0) i = iconSideBar.Length - 1;
        if (i >= iconSideBar.Length) i = 0;

        //Show the tab game in the present
        tabGame[i].SetActive(true);
        iconSideBar[i].transform.position += new Vector3(-17, 0, 0);
        iconSideBar[i].gameObject.GetComponent<Image>().sprite = keySwitch[4];
        iconSideBar[i].GetComponent<RectTransform>().localScale = new Vector3(2, 2, 2);

        //Hide the tab game in the previous
        iconSideBar[previousIndex].transform.position += new Vector3(17, 0, 0);
        iconSideBar[previousIndex].gameObject.GetComponent<Image>().sprite = keySwitch[5];
        iconSideBar[previousIndex].GetComponent<RectTransform>().localScale = new Vector3(1.6f, 1.6f, 1.6f);
        tabGame[previousIndex].SetActive(false);
    }

    //Set status for name box
    public void ConfirmData()
    {
        showData.text = getData.text;
        setNameBoxActive = !setNameBoxActive;
        setNameBox.SetActive(setNameBoxActive);
    }
    public void CloseBox()
    {
        setNameBoxActive = !setNameBoxActive;
        setNameBox.SetActive(setNameBoxActive);
    }

    public void OpenNameBox()
    {
        setNameBoxActive = !setNameBoxActive;
        setNameBox.SetActive(setNameBoxActive);
    }

    private IEnumerator DeplayForAnimation()
    {
        yield return new WaitForSeconds(2.09f);
        gameUI.SetActive(false);
    }

    public void UpdateSkillPoints()
    {
        int addSurvival;
        int.TryParse(skillsIndex[0].text, out addSurvival);
        int addNegatiaton;
        int.TryParse(skillsIndex[1].text, out addNegatiaton);
        int addAnalyst;
        int.TryParse(skillsIndex[2].text, out addAnalyst);
        int addFight;
        int.TryParse(skillsIndex[3].text, out addFight);

        Characters.maxHealth = 100 + Characters.level * 10 + addSurvival * 5;
        Characters.maxMana = 40 + Characters.level * 10 + addNegatiaton * 2;
        Characters.maxStamina = 5 + Characters.level * 10 + addSurvival * 1;

        player.defense = 50 + addNegatiaton * 3; 
        player.ratioRun = 2f + addSurvival * 0.1f;
        Characters.luck = 10 + addNegatiaton * 2;
    }
    public void UpdateStats() 
    { 
        statsIndex[0].text = "HP: " + Characters.currentHealth.ToString() + "/" + Characters.maxHealth.ToString();
        statsIndex[1].text = "MP: " + Characters.currentMana.ToString() + "/" + Characters.maxMana.ToString();
        statsIndex[2].text = "Thể lực: " + Mathf.Round(Characters.currentStamina * 10)/10 + "/" + Characters.maxStamina.ToString();
        statsIndex[3].text = "Sát thương: " + player.damage.ToString();
        statsIndex[4].text = "Phòng thủ: " + player.defense.ToString();
        statsIndex[5].text = "Tốc độ: " + player.runSpeed.ToString();
        statsIndex[6].text = "May mắn: " + Characters.luck.ToString();
    }
    public void UpdateStatsNow()
    {
        button.GetComponent<Image>().sprite = selectedSprite;
    }
    void ChangeSelection(int direction)
    {
        // Đặt lại trạng thái của object con hiện tại
        SetChildNormal(selectedIndex);

        // Tính chỉ số mới
        selectedIndex += direction;
        if (selectedIndex < 0) selectedIndex = parentObject.childCount - 1; // Vòng lại object cuối
        else if (selectedIndex >= parentObject.childCount) selectedIndex = 0; // Vòng lại object đầu

        // Cập nhật trạng thái của object con mới
        SelectChild(selectedIndex);
    }

    void SelectChild(int index)
    {
        Transform child = parentObject.GetChild(index); 
        child.Find("Round").GetComponent<Image>().sprite = selectedImage[1];
        itemSelectedIcon.sprite = child.GetComponent<IdentifyItem>().item.itemIcon;
        informationBox[1].text = child.GetComponent<IdentifyItem>().item.itemDescription;
    }

    void SetChildNormal(int index)
    {
        Transform child = parentObject.GetChild(index); 
        child.Find("Round").GetComponent<Image>().sprite = selectedImage[0]; 
    }

    public void UseItem()
    {
        Transform child = parentObject.GetChild(selectedIndex);
        child.GetComponent<IdentifyItem>().Use();
        
        child.GetComponent<IdentifyItem>().item.itemAmount--;
        if (child.GetComponent<IdentifyItem>().item.itemAmount <= 0)
        {
            Destroy(child.gameObject);
            selectedIndex -= 1;
            SelectChild(selectedIndex);
        }

        var itemAmounts = child.Find("Amount").GetComponent<TextMeshProUGUI>();
        itemAmounts.text = child.GetComponent<IdentifyItem>().item.itemAmount.ToString();
    }
    public void RemoveItem()
    {
        Transform child = parentObject.GetChild(selectedIndex);
        child.GetComponent<IdentifyItem>().item.itemAmount = 0;
        Destroy(child.gameObject);
    }
}

