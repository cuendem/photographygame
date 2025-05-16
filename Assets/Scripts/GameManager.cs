using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int money = 0;
    public string powerUp = "None";
    public bool powerUpActive = false;

    public float powerUpDuration = 5f;
    public float elapsedPowerUpTime = 0f;

    public TextMeshProUGUI moneyText;
    public Image powerUpImage;
    public Sprite[] powerUpSprites;

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

    void Update()
    {
        if (powerUpActive)
        {
            elapsedPowerUpTime += Time.deltaTime;
            if (elapsedPowerUpTime >= powerUpDuration)
            {
                powerUpActive = false;
                elapsedPowerUpTime = 0f;
                powerUpDuration = 0f;
                ClearPowerUp();
            }
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

    public Sprite GetPowerUpSprite(string powerUpName)
    {
        foreach (Sprite sprite in powerUpSprites)
        {
            if (sprite.name == powerUpName)
            {
                return sprite;
            }
        }
        return null; // Return null if no matching sprite is found
    }

    public void SetPowerUp(string powerUpName)
    {
        powerUp = powerUpName;

        Sprite sprite = GetPowerUpSprite(powerUpName);
        if (sprite != null)
        {
            powerUpImage.sprite = sprite;
            powerUpImage.enabled = true;
        }
    }

    public void ClearPowerUp()
    {
        powerUp = "None";
        powerUpImage.enabled = false; // Hide the image
        powerUpImage.sprite = null;   // Optional: clear the sprite to avoid showing old image if re-enabled later
        Color imgColor = powerUpImage.color;
        imgColor.a = 1f;
        powerUpImage.color = imgColor;
        Debug.Log("Power-up used!");
    }

    public string GetPowerUp()
    {
        return powerUp;
    }

    public void UsePowerUp()
    {
        if (powerUp != "None")
        {
            Debug.Log("Using power-up: " + powerUp);
            Color imgColor = powerUpImage.color;
            imgColor.a = 0.5f;
            powerUpImage.color = imgColor;

            powerUpDuration = 0f;

            switch (powerUp)
            {
                case "Flash":
                    powerUpDuration = 3f;

                    // Iterate through all objects with the "Bird", "Vehicle" or "Insect" tags and summon a FlashedStar above them
                    GameObject[] birds = GameObject.FindGameObjectsWithTag("Bird");
                    GameObject[] vehicles = GameObject.FindGameObjectsWithTag("Vehicle");
                    GameObject[] insects = GameObject.FindGameObjectsWithTag("Insect");

                    GameObject[] allObjects = new GameObject[birds.Length + vehicles.Length + insects.Length];

                    birds.CopyTo(allObjects, 0);
                    vehicles.CopyTo(allObjects, birds.Length);
                    insects.CopyTo(allObjects, birds.Length + vehicles.Length);

                    foreach (GameObject obj in allObjects)
                    {
                        Vector3 spawnPosition = new Vector3(obj.transform.position.x, obj.transform.position.y + 0.4f, obj.transform.position.z);
                        GameObject flashedStar = Instantiate(Resources.Load("FlashedStar"), spawnPosition, Quaternion.identity) as GameObject;
                        flashedStar.transform.SetParent(obj.transform);
                    }
                    break;
                case "SpeedUp":
                    powerUpDuration = 5f;
                    break;
                case "Zoom":
                    powerUpDuration = 10f;
                    break;
            }

            elapsedPowerUpTime = 0f;
            powerUpActive = true;
        }
        else
        {
            Debug.Log("No power-up to use.");
        }
    }
}
