using UnityEngine;

public class PlayerFollowMouse : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
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
}
