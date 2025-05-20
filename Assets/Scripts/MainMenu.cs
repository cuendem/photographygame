using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        // Move the UI Canvas objects up slowly for a smooth transition
        RectTransform[] uiElements = GetComponentsInChildren<RectTransform>();
        foreach (RectTransform uiElement in uiElements)
        {
            if (uiElement != null && uiElement.gameObject.name != "MainMenu")
            {
                uiElement.anchoredPosition += new Vector2(0, 100f * Time.deltaTime);
            }
        }
        // Load the game scene
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
    }
}
