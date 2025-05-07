using UnityEngine;

public class PlayerUsePowerup : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("Power-up used!");
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
}
