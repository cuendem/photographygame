using UnityEngine;

public class BasicFlight : MonoBehaviour
{
    public float speed = 5f; // Speed of the object
    private int direction = 1; // 1 for right, -1 for left

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (transform.position.x < 0)
        {
            direction = 1; // Move right if the object is on the left side
        }
        else
        {
            direction = -1; // Move left if the object is on the right side
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Move the object in the specified direction
        transform.Translate(Vector3.right * speed * direction * Time.deltaTime);

        // Destroy the object if it goes off-screen
        if (transform.position.x < -15f || transform.position.x > 15f)
        {
            Destroy(gameObject);
        }
    }
}
