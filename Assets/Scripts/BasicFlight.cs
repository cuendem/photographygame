using UnityEngine;

public class BasicFlight : MonoBehaviour
{
    public float xSpeed = 5f; // Speed X of the object
    public float ySpeed = 5f; // Speed Y of the object
    private int direction = 1; // 1 for right, -1 for left
    private bool hasBoomeranged = false; // Flag to check if the object has boomeranged

    private float waveAmplitude = 0.1f; // Amplitude of the wave
    private float waveFrequency = 2f; // Frequency of the wave

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        xSpeed = Random.Range(2f, 7f);
        ySpeed = Random.Range(2f, 7f);

        waveAmplitude = Random.Range(0f, 0.1f);
        waveFrequency = Random.Range(0f, 3f);

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
        Flight();

        // Chance to move to the end of the screen and come back
        if (Random.value > 0.75f && (((transform.position.x > 9f && direction == 1) || (transform.position.x < -9f && direction == -1)) && !hasBoomeranged))
        {
            direction *= -1; // Reverse direction
            hasBoomeranged = true; // Set the flag to true
        }

        // Destroy the object if it goes off-screen
        if (transform.position.x < -15f || transform.position.x > 15f)
        {
            Destroy(gameObject);
        }
    }

    void Flight()
    {
        // Move the object in the specified direction
        transform.Translate(Vector3.right * xSpeed * direction * Time.deltaTime);
        float waveOffset = Mathf.Sin(Time.time * waveFrequency) * waveAmplitude;
        transform.position += new Vector3(0, waveOffset, 0);
    }
}
