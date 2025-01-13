using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAnimation : MonoBehaviour
{
    public GameObject GameObject;
    public bool GameObjectActive = false;
    public void SideBarActive()
    {
        GameObject.SetActive(!GameObjectActive);
    }
}
