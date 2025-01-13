using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    
    public void PlayGame()
    {
        // Load Scene của game chính (thay "GameScene" bằng tên Scene của bạn)
        SceneManager.LoadSceneAsync("ThelandOfLight");
    }
    
    public void QuitGame()
    {
        // Thoát game
        Application.Quit();
    }
}