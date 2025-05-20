using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        // Start game
        GameManager.Instance.gameStarted = true;
        GameManager.Instance.remainingTime = GameManager.Instance.timeLimit;
        GameManager.Instance.money = 0;
        GameManager.Instance.powerUp = "None";
        GameManager.Instance.powerUpActive = false;
        GameManager.Instance.SetHighScores(new List<int>(new int[21]));

        // Load the game scene
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
    }

    public void QuitGame()
    {
        // Quit the application
        Application.Quit();

        // If running in the editor, stop playing
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
    
    public void OpenSettings()
    {
        // Load the settings scene
        UnityEngine.SceneManagement.SceneManager.LoadScene("Settings");
    }

    public void MainMenuScene()
    {
        // Load the main menu scene
        UnityEngine.SceneManagement.SceneManager.LoadScene("Inicio");
    }
}
