using System;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public BoxCollider2D outerCollider;
    public BoxCollider2D innerCollider;

    private float pictureRate = 1f;
    private float nextPictureTime = 0f;

    private void Start()
    {
        
    }

    void Update()
    {
        FollowMouse();

        if (Input.GetMouseButtonDown(0) && Time.time >= nextPictureTime) // Left click
        {
            TakePicture();
            ScheduleNextPicture();
        }

        if (Input.GetMouseButtonDown(1)){ // Right click
            UsePowerUp();
        }

    }

    private void UsePowerUp()
    {
        Debug.Log("Power-up used!");
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
        Vector2 center = outerCollider.bounds.center;
        Vector2 size = outerCollider.bounds.size;

        Collider2D[] hits = Physics2D.OverlapBoxAll(center, size, 0f);
        List<string> summary = new List<string>();

        int totalPhotoMoney = 0;

        foreach (Collider2D col in hits)
        {
            GameObject obj = col.gameObject;
            string name = obj.name;
            string position = GetObjectPositionStatus(col);

            if (obj.TryGetComponent(out BasicFlight flight))
            {
                summary.Add($"{name} ({position})");

                switch (position)
                {
                    case "CENTERED":
                        totalPhotoMoney += flight.money * 3;
                        break;
                    case "CONTAINED":
                        totalPhotoMoney += flight.money * 2;
                        break;
                    case "CUT":
                        totalPhotoMoney += flight.money;
                        break;
                }
            }
        }

        Debug.Log("📸 Picture Taken:\n" + string.Join(", ", summary));
        Debug.Log($"Total Picture Money: ${totalPhotoMoney}");

        GameManager.Instance.AddMoney(totalPhotoMoney);
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
}
