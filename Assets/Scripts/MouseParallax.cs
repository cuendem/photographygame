using UnityEngine;

public class MouseParallax : MonoBehaviour
{
    [Tooltip("How much this object moves in response to mouse movement. Higher = more movement.")]
    public float parallaxStrength = 0.05f;

    [Tooltip("Optional: Clamp the max movement from original position (in units). Set to 0 for no clamp.")]
    public float maxOffset = 0f;

    private Vector3 initialPosition;
    private Vector2 screenCenter;

    void Start()
    {
        initialPosition = transform.position;
        screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
    }

    void Update()
    {
        Vector2 mouseOffset = (Vector2)Input.mousePosition - screenCenter;
        Vector3 parallaxOffset = new Vector3(
            mouseOffset.x / screenCenter.x,
            mouseOffset.y / screenCenter.y,
            0f) * parallaxStrength;

        Vector3 targetPosition = initialPosition + parallaxOffset;

        if (maxOffset > 0f)
        {
            // Clamp the movement within a box around the initial position
            targetPosition = new Vector3(
                Mathf.Clamp(targetPosition.x, initialPosition.x - maxOffset, initialPosition.x + maxOffset),
                Mathf.Clamp(targetPosition.y, initialPosition.y - maxOffset, initialPosition.y + maxOffset),
                targetPosition.z
            );
        }

        transform.position = targetPosition;
    }
}
