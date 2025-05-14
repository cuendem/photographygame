using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int money = 0;

    public TextMeshProUGUI moneyText;

    void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep across scenes
        }
        else
        {
            Destroy(gameObject); // Prevent duplicates
        }
    }

    public void AddMoney(int amount)
    {
        money += amount;
        if (money < 0)
        {
            money = 0;
        }
        Debug.Log("Money updated: " + money);
        moneyText.text = "$" + money.ToString();
    }

    public void SetMoney(int value)
    {
        money = value;
        Debug.Log("Money set to: " + money);
        moneyText.text = "$" + money.ToString();
    }

    public int GetMoney()
    {
        return money;
    }
}
