using System;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int money = 0;
    private HashSet<Collider2D> overlappingObjects = new HashSet<Collider2D>();

    void Update()
    {
        FollowMouse();

        if (Input.GetMouseButtonDown(0)) // Left click
        {
            TakePicture();
        }

        if (Input.GetMouseButtonDown(1)){ // Right click
            UsePowerUp();
        }

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        overlappingObjects.Add(other);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        overlappingObjects.Remove(other);
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

    void TakePicture(){
        Debug.Log(overlappingObjects.Count);
    }

}
