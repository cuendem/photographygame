using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public BoxCollider2D outerCollider;
    public BoxCollider2D innerCollider;

    private float pictureRate = 0.7f;
    private float nextPictureTime = 0f;

    private Vector3 targetScale = Vector3.one;
    private float scaleSpeed = 6f;

    private SpriteRenderer playerSprite;
    private SpriteRenderer flashRenderer;

    private void Start()
    {
        // Find the child called "Flash" and get its SpriteRenderer
        flashRenderer = transform.Find("Flash")?.GetComponent<SpriteRenderer>();
        playerSprite = GetComponent<SpriteRenderer>();

        if (flashRenderer != null)
        {
            flashRenderer.color = new Color(1f, 1f, 1f, 0f); // Fully transparent
        }
    }

    void Update()
    {
        FollowMouse();

        if (Input.GetMouseButtonDown(0) && Time.time >= nextPictureTime) // Left click
        {
            TakePicture();
            ScheduleNextPicture();
        }

        if (Input.GetMouseButtonDown(1)) // Right click
        {
            UsePowerUp();
        }

        // Set the target scale based on power-up state
        if (GameManager.Instance.powerUpActive && GameManager.Instance.powerUp == "Zoom")
        {
            targetScale = Vector3.one * 0.4f;
        }
        else
        {
            targetScale = Vector3.one;
        }

        // Smoothly interpolate to the target scale
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * scaleSpeed);

        // Flash effect
        if (flashRenderer != null)
        {
            flashRenderer.color = new Color(1f, 1f, 1f, Mathf.Clamp01(flashRenderer.color.a - Time.deltaTime * 5f)); // Fade out
        }
    }

    private void UsePowerUp()
    {
        GameManager.Instance.UsePowerUp();
    }

    void FollowMouse()
    {
        Vector2 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector2(cursorPos.x, cursorPos.y);

        // Limit the player's position to the screen bounds considering the player's size and padding
        Vector2 screenBounds = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        Vector2 playerSize = GetComponent<SpriteRenderer>().bounds.size;
        float padding = 10f * Camera.main.orthographicSize / Screen.height; // Adjust padding to world units

        float clampedX = Mathf.Clamp(transform.position.x, -screenBounds.x + playerSize.x / 2 + padding, screenBounds.x - playerSize.x / 2 - padding);
        float clampedY = Mathf.Clamp(transform.position.y, -screenBounds.y + playerSize.y / 2 + padding, screenBounds.y - playerSize.y / 2 - padding);
        transform.position = new Vector2(clampedX, clampedY);

        Cursor.visible = false;
    }

    void TakePicture()
    {
        transform.localScale = transform.localScale * 0.8f;
        flashRenderer.color = new Color(1f, 1f, 1f, 1f); // Flash

        List<int> highscores = GameManager.Instance.GetHighScores();

        // Start coroutine to capture screenshot portion and instantiate photograph
        StartCoroutine(CapturePhotoCoroutine());

        Vector2 center = outerCollider.bounds.center;
        Vector2 size = outerCollider.bounds.size;

        Collider2D[] hits = Physics2D.OverlapBoxAll(center, size, 0f);
        List<string> summary = new List<string>();

        int totalPhotoMoney = 0;
        string powerUp = "None";

        int totalElements = 0;

        foreach (Collider2D col in hits)
        {
            GameObject obj = col.gameObject;
            string name = obj.name;
            string position = GetObjectPositionStatus(col);

            if (obj.TryGetComponent(out BasicFlight flight))
            {
                totalElements++;
                summary.Add($"{name} ({position})");

                switch (position)
                {
                    case "CENTERED":
                        totalPhotoMoney += flight.money * 3;
                        highscores[16]++;
                        break;
                    case "CONTAINED":
                        totalPhotoMoney += flight.money * 2;
                        highscores[17]++;
                        break;
                    case "CUT":
                        totalPhotoMoney += flight.money;
                        highscores[18]++;
                        break;
                }

                switch (name)
                {
                    case "Parakeet(Clone)":
                        highscores[0]++;
                        break;
                    case "Parrot(Clone)":
                        highscores[1]++;
                        break;
                    case "Hummingbird(Clone)":
                        highscores[2]++;
                        break;
                    case "Helicopter(Clone)":
                        highscores[3]++;
                        break;
                    case "Balloon(Clone)":
                        highscores[4]++;
                        break;
                    case "Plane(Clone)":
                        highscores[5]++;
                        break;
                    case "Firefly(Clone)":
                        highscores[6]++;
                        break;
                    case "Butterfly(Clone)":
                        highscores[7]++;
                        break;
                    case "Ladybug(Clone)":
                        highscores[8]++;
                        break;
                }

                if (!string.IsNullOrEmpty(flight.powerup))
                {
                    powerUp = flight.powerup;
                }
            }

            // Delete if object is an insect
            if (obj.CompareTag("Insect"))
            {
                Destroy(obj);
            }
        }

        Debug.Log("📸 Picture Taken:\n" + string.Join(", ", summary));
        Debug.Log($"Total Picture Money: ${totalPhotoMoney}");

        highscores[19]++;

        if (totalPhotoMoney >= 0) {
            highscores[9] += totalPhotoMoney;
        } else {
            highscores[10] += totalPhotoMoney;
        }

        if (totalPhotoMoney > highscores[14])
        {
            highscores[14] = totalPhotoMoney;
        }

        if (totalElements > highscores[15])
        {
            highscores[15] = totalElements;
        }

        int multiplier = 1;

        if (GameManager.Instance.powerUpActive && GameManager.Instance.powerUp == "Zoom")
        {
            multiplier = 2;
        }

        GameManager.Instance.AddMoney(totalPhotoMoney * multiplier);

        if (powerUp != "None" && GameManager.Instance.powerUpActive == false)
        {
            GameManager.Instance.SetPowerUp(powerUp);
        }

        GameManager.Instance.SetHighScores(highscores);
    }

    string GetObjectPositionStatus(Collider2D col)
    {
        Bounds bounds = col.bounds;
        Bounds outerBounds = outerCollider.bounds;
        Bounds innerBounds = innerCollider.bounds;

        bool fullyInsideOuter = outerBounds.Contains(bounds.min) && outerBounds.Contains(bounds.max);
        bool touchingInner = col.IsTouching(innerCollider);

        if (touchingInner) return "CENTERED";
        else if (fullyInsideOuter) return "CONTAINED";
        else if (col.IsTouching(outerCollider)) return "CUT";
        else return "OUTSIDE";
    }

    void ScheduleNextPicture()
    {
        nextPictureTime = Time.time + pictureRate;
    }

    private IEnumerator CapturePhotoCoroutine()
    {
        // Make player invisible for a moment
        playerSprite.enabled = false;
        flashRenderer.enabled = false;

        yield return new WaitForEndOfFrame(); // Wait for end of frame to capture the screen

        // Get the bounds of the outer collider in world space
        Bounds bounds = outerCollider.bounds;

        // Convert world bounds to screen pixels
        Vector3 bottomLeft = Camera.main.WorldToScreenPoint(new Vector3(bounds.min.x, bounds.min.y));
        Vector3 topRight = Camera.main.WorldToScreenPoint(new Vector3(bounds.max.x, bounds.max.y));

        // Calculate rectangle in screen coordinates
        int width = Mathf.CeilToInt(topRight.x - bottomLeft.x);
        int height = Mathf.CeilToInt(topRight.y - bottomLeft.y);

        // Clamp coordinates and size to screen bounds to avoid errors
        int x = Mathf.Clamp(Mathf.FloorToInt(bottomLeft.x), 0, Screen.width);
        int y = Mathf.Clamp(Mathf.FloorToInt(bottomLeft.y), 0, Screen.height);
        width = Mathf.Clamp(width, 0, Screen.width - x);
        height = Mathf.Clamp(height, 0, Screen.height - y);

        // Read pixels from the screen
        Texture2D screenTexture = new Texture2D(width, height, TextureFormat.RGBA32, false);
        screenTexture.ReadPixels(new Rect(x, y, width, height), 0, 0);
        screenTexture.Apply();

        // Create a sprite from the texture
        Sprite photoSprite = Sprite.Create(screenTexture,
            new Rect(0, 0, width, height),
            new Vector2(0.5f, 0.5f), 100f); // 100 pixels per unit, adjust if needed

        // Instantiate Photograph and assign sprite
        GameObject photoObj = Instantiate(Resources.Load("Photograph"), transform.position, Quaternion.identity) as GameObject;
        Transform takenPhotoTransform = photoObj.transform.Find("TakenPhoto");
        if (takenPhotoTransform != null)
        {
            SpriteRenderer sr = takenPhotoTransform.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
            sr.sprite = photoSprite;
            }
        }

        // Make player visible again
        playerSprite.enabled = true;
        flashRenderer.enabled = true;
    }
}
