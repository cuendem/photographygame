using UnityEngine;

public class PhotographParticle : MonoBehaviour
{
    private Rigidbody2D rb;
    public float minSpeedX = 1f;
    public float maxSpeedX = 8f;
    public float minSpeedY = 4f;
    public float maxSpeedY = 10f;
    public float minRotationSpeed = 20f;
    public float maxRotationSpeed = 150f;
    public float fadeSpeed = 1f;

    void Start()
    {
        // Get the player transform
        Transform playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        // Get player's X scale
        float playerXScale = playerTransform.localScale.x;

        // Set your particle's scale X same as player
        // Set Y scaled accordingly - for example, maintain aspect ratio by multiplying with a factor or keep same as X
        float particleYScale = playerXScale * (transform.localScale.y / transform.localScale.x); // keeps ratio

        rb = GetComponent<Rigidbody2D>();

        // Choose speed: either between -5 and -2 or between 2 and 5
        float xspeed = (Random.value < 0.5f) 
            ? Random.Range(-maxSpeedX, -minSpeedX)
            : Random.Range(minSpeedX, maxSpeedX);

        float yspeed = Random.Range(minSpeedY, maxSpeedY);

        // Choose rotation speed: either between -100 and -15 or between 15 and 100
        float rotationSpeed = (Random.value < 0.5f)
            ? Random.Range(-maxRotationSpeed, -minRotationSpeed)
            : Random.Range(minRotationSpeed, maxRotationSpeed);

        rb.linearVelocity = new Vector2(xspeed, yspeed);
        rb.angularVelocity = rotationSpeed;

        // rb.gravityScale = 0f; // Disable gravity

        float randomSize = Random.Range(1f, 1.5f);

        transform.localScale = new Vector3(playerXScale * 2, particleYScale * 2, 1f) * randomSize;
    }

    void Update()
    {
        // Fade this object's sprite
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null) {
            Color color = sr.color;
            if (color.a > 0f) {
                color.a -= Time.deltaTime * fadeSpeed;
                color.a = Mathf.Clamp01(color.a);
                sr.color = color;
            }
        }

        // Fade the two inner children: TakenPhoto and Shine
        string[] childNames = { "TakenPhoto", "Shine" };
        foreach (string childName in childNames)
        {
            Transform child = transform.Find(childName);
            if (child != null) {
                SpriteRenderer childSr = child.GetComponent<SpriteRenderer>();
                if (childSr != null)
                {
                    Color childColor = childSr.color;
                    if (childColor.a > 0f) {
                        childColor.a -= Time.deltaTime * fadeSpeed;
                        childColor.a = Mathf.Clamp01(childColor.a);
                        childSr.color = childColor;
                    }
                }
            }
        }

        // Disappear if offscreen
        if (transform.position.y < -15f) {
            Destroy(gameObject);
        }
    }
}
