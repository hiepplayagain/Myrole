using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    //Health Bar
    public Slider slider;
    public Color lowHealth;
    public Color highHealth;
    public Vector3 offset;
    // Start is called before the first frame update
    

    // Update is called once per frame
    void Update()
    {
        slider.transform.position = Camera.main.WorldToScreenPoint(transform.parent.position + offset);
    }

    public void SetHealth(int currentHealth, int maxHealth)
    {
        slider.gameObject.SetActive(currentHealth < maxHealth);
        slider.value = currentHealth;
        slider.maxValue = maxHealth;
        slider.fillRect.GetComponentInChildren<Image>().color = Color.Lerp(lowHealth, highHealth, slider.normalizedValue);
        if (currentHealth <= 0) slider.gameObject.SetActive(false);
    }
}
