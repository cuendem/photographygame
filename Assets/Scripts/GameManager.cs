using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

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

    public Flash flash;

    public List<int> highScores;

    private AudioSource audioSource;

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

        if (highScores == null || highScores.Count < 21)
        {
            highScores = new List<int>(new int[21]);
        }

        audioSource = GetComponent<AudioSource>();
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

        // Show payout text
        if (amount != 0) {
            Canvas canvas = GameObject.FindFirstObjectByType<Canvas>();

            // Instantiate the payout object and parent it to the canvas
            GameObject payoutObject = Instantiate(Resources.Load("Payout")) as GameObject;
            payoutObject.transform.SetParent(canvas.transform, false); // 'false' keeps local scale/position intact

            payoutObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(-285, -180);

            // Set payout value
            Payout payout = payoutObject.GetComponent<Payout>();
            payout.SetPayout(amount);
        }
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
                    powerUpDuration = 2.5f;

                    // Iterate through all objects with the "Bird", "Vehicle" or "Insect" tags and summon a FlashedStar above them
                    GameObject[] birds = GameObject.FindGameObjectsWithTag("Bird");
                    // GameObject[] vehicles = GameObject.FindGameObjectsWithTag("Vehicle");
                    GameObject[] insects = GameObject.FindGameObjectsWithTag("Insect");

                    GameObject[] allObjects = new GameObject[birds.Length + insects.Length];

                    birds.CopyTo(allObjects, 0);
                    // vehicles.CopyTo(allObjects, birds.Length);
                    insects.CopyTo(allObjects, birds.Length);

                    foreach (GameObject obj in allObjects)
                    {
                        Vector3 spawnPosition = new Vector3(obj.transform.position.x, obj.transform.position.y + 0.6f, obj.transform.position.z);
                        GameObject flashedStar = Instantiate(Resources.Load("FlashedStar"), spawnPosition, Quaternion.identity) as GameObject;
                        flashedStar.transform.SetParent(obj.transform);
                    }

                    ShakeScreen(0.3f, 0.2f);

                    flash.TriggerFlash();

                    highScores[11]++;

                    break;
                case "SpeedUp":
                    powerUpDuration = 2f;
                    highScores[12]++;
                    ShakeScreen(2f, 0.05f);
                    break;
                case "Zoom":
                    powerUpDuration = 5f;
                    highScores[13]++;
                    break;
            }

            PowerBar powerBar = GameObject.Find("PowerBarOverlay").GetComponent<PowerBar>();
            powerBar.SetTimer(powerUpDuration);

            elapsedPowerUpTime = 0f;
            powerUpActive = true;
        }
        else
        {
            Debug.Log("No power-up to use.");
        }
    }

    public List<int> GetHighScores()
    {
        return highScores;

        // 0 = Parakeets photographed
        // 1 = Parrots photographed
        // 2 = Hummingbirds photographed
        // 3 = Helicopters photographed
        // 4 = Balloons photographed
        // 5 = Planes photographed
        // 6 = Fireflies photographed
        // 7 = Butterflies photographed
        // 8 = Ladybugs photographed
        // 9 = Money earned
        // 10 = Money lost
        // 11 = Flashes used
        // 12 = Speed Ups used
        // 13 = Zooms used
        // 14 = Highest value photograph
        // 15 = Highest amount of elements in a photograph
        // 16 = Total elements taken centered
        // 17 = Total elements taken contained
        // 18 = Total elements taken cut
        // 19 = Total photos taken
        // 20 = Lowest value photograph
    }

    public void SetHighScores(List<int> scores)
    {
        highScores = scores;
        Debug.Log("High scores updated: " + string.Join(", ", highScores));
    }

    public void ShakeScreen(float duration = 0.2f, float magnitude = 0.1f)
    {
        StartCoroutine(ShakeScreenCoroutine(duration, magnitude));
    }

    public IEnumerator ShakeScreenCoroutine(float duration = 0.2f, float magnitude = 0.1f)
    {
        Vector3 originalPos = Camera.main.transform.localPosition;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            float offsetX = Random.Range(-1f, 1f) * magnitude;
            float offsetY = Random.Range(-1f, 1f) * magnitude;

            Camera.main.transform.localPosition = originalPos + new Vector3(offsetX, offsetY, 0f);

            elapsed += Time.deltaTime;
            yield return null;
        }

        Camera.main.transform.localPosition = originalPos;
    }

    public void PlaySound(string soundName)
    {
        AudioClip clip = Resources.Load<AudioClip>(soundName);
        if (clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
