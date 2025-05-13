using UnityEngine;

public class BasicFlight : MonoBehaviour
{
    public float xSpeed = 5f; // Speed X of the object
    public float ySpeed = 5f; // Speed Y of the object
    private int direction = 1; // 1 for right, -1 for left
    private bool hasBoomeranged = false; // Flag to check if the object has boomeranged

    private float waveAmplitude = 0f; // Amplitude of the wave
    private float waveFrequency = 0f; // Frequency of the wave

    private float baseY; // Starting Y position

    private SpriteRenderer spriteRenderer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        xSpeed = Random.Range(4f, 7f);
        ySpeed = Random.Range(4f, 7f);

        waveAmplitude = Random.Range(0f, 1f);
        waveFrequency = Random.Range(0.5f, 5f);

        baseY = transform.position.y;

        if (transform.position.x < 0)
        {
            direction = 1;
            spriteRenderer.flipX = false;
        }
        else
        {
            direction = -1;
            spriteRenderer.flipX = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Flight();

        // Chance to move to the end of the screen and come back
        if (Random.value > 0.75f && (((transform.position.x > 9f && direction == 1) || (transform.position.x < -9f && direction == -1)) && !hasBoomeranged))
        {
            direction *= -1; // Reverse direction
            hasBoomeranged = true; // Set the flag to true
            if (spriteRenderer.flipX == true) {
                spriteRenderer.flipX = false;
            } else {
                spriteRenderer.flipX = true;
            }
        }

        // Destroy the object if it goes off-screen
        if (transform.position.x < -15f || transform.position.x > 15f)
        {
            Destroy(gameObject);
        }
    }

    void Flight()
    {
        // Move horizontally
        transform.Translate(Vector3.right * xSpeed * direction * Time.deltaTime);

        // Apply vertical wave offset relative to baseY
        float waveOffset = Mathf.Sin(Time.time * waveFrequency) * waveAmplitude;
        Vector3 position = transform.position;
        position.y = baseY + waveOffset;
        transform.position = position;
    }
}
