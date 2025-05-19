using UnityEngine;

public class BasicFlight : MonoBehaviour
{
    public bool mobile = true;

    public int money;
    public string powerup;

    public float maxXSpeed = 7f;
    public float minXSpeed = 4f;
    public float maxYSpeed = 7f;
    public float minYSpeed = 4f;
    public float maxWaveAmplitude = 1f;
    public float minWaveAmplitude = 0f;
    public float maxWaveFrequency = 5f;
    public float minWaveFrequency = 0.5f;
    public float maxScale = 1.5f;
    public float minScale = 0.5f;

    private float xSpeed = 0f;
    private float ySpeed = 0f;
    private int direction = 1; // 1 for right, -1 for left
    private bool hasBoomeranged = false; // Flag to check if the object has boomeranged

    private float waveAmplitude = 0f;
    private float waveFrequency = 0f;

    private float baseY; // Starting Y position

    private SpriteRenderer spriteRenderer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        xSpeed = Random.Range(minXSpeed, maxXSpeed);
        ySpeed = Random.Range(minYSpeed, maxYSpeed);

        waveAmplitude = Random.Range(minWaveAmplitude, maxWaveAmplitude);
        waveFrequency = Random.Range(minWaveFrequency, maxWaveFrequency);

        baseY = transform.position.y;

        if (mobile)
        {
            Vector3 newScale = transform.localScale;
            float newScaleF = Random.Range(minScale, maxScale);
            newScale.x = newScaleF;
            newScale.y = newScaleF;
            transform.localScale = newScale;

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
    }

    // Update is called once per frame
    void Update()
    {
        if (mobile && (!(GameManager.Instance.powerUpActive && GameManager.Instance.powerUp == "Flash") || gameObject.CompareTag("Vehicle")))
        {
            Flight();
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

        // Chance to move to the end of the screen and come back
        if ((transform.position.x > 9f && direction == 1) || (transform.position.x < -9f && direction == -1)) {
            if (Random.value < 0.3f && !hasBoomeranged)
            {
                direction *= -1; // Reverse direction
                hasBoomeranged = true;
                if (spriteRenderer.flipX == true) {
                    spriteRenderer.flipX = false;
                } else {
                    spriteRenderer.flipX = true;
                }
            } else {
                hasBoomeranged = true;
            }
        }

        // Destroy the object if it goes off-screen
        if (transform.position.x < -15f || transform.position.x > 15f)
        {
            Destroy(gameObject);
        }
    }
}
