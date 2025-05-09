using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int money = 0;
    public LayerMask targetLayer; // Layer of objects you want to collect
    public float overlapRadius = 0.5f; // Radius for overlap check

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

    void CheckForTarget()
    {
        // Overlap check at the trigger's position
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, overlapRadius, targetLayer);

        foreach (var hit in hits)
        {
            if (hit != null)
            {
                money += 1;
                Debug.Log("Hit " + hit.name + "! money: " + money);
                // Optionally destroy the target or mark it as collected
                Destroy(hit.gameObject);
            }
        }
    }

    void TakePicture(){
        Debug.Log("Photo taken!");
        // // Get the mouse position in world coordinates
        // Vector2 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        // // Create a new picture object at the cursor position
        // GameObject picture = new GameObject("Picture");
        // picture.transform.position = new Vector2(cursorPos.x, cursorPos.y);
        
        // // Add a SpriteRenderer component to the picture object
        // SpriteRenderer spriteRenderer = picture.AddComponent<SpriteRenderer>();
        
        // // Set the sprite for the picture (you can replace "YourSprite" with your actual sprite)
        // spriteRenderer.sprite = Resources.Load<Sprite>("YourSprite");
        
        // // Optionally, set the sorting layer and order of the sprite
        // spriteRenderer.sortingLayerName = "UI";
        // spriteRenderer.sortingOrder = 1;
    }

}
