using System;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Reference to the trigger script get conmponent
    private CenterTrigger centerTrigger;
    private MiddleTrigger middleTrigger;
    private OutsideTrigger outsideTrigger;

    private float nextPictureTime = 0f;

    private void Start()
    {
        // Find the trigger components in the scene
        centerTrigger = GameObject.Find("Centered trigger").GetComponent<CenterTrigger>();
        middleTrigger = GameObject.Find("Middle trigger").GetComponent<MiddleTrigger>();
        outsideTrigger = GameObject.Find("Outside trigger").GetComponent<OutsideTrigger>();
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

    void TakePicture(){
        HashSet<Collider2D> centered = centerTrigger.GetCentered();
        HashSet<Collider2D> middle = middleTrigger.GetMiddle();
        HashSet<Collider2D> outside = outsideTrigger.GetOutside();

        // Intersect outside with the updated middle (keep only common elements)
        outside.IntersectWith(middle);

        // Remove from middle any colliders also in outside
        middle.ExceptWith(outside);

        GameManager.Instance.AddMoney(centered.Count*1 + middle.Count*2 + outside.Count*1);
    }

    void ScheduleNextPicture()
    {
        nextPictureTime = Time.time + 3f;
    }
}
