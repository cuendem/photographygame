using UnityEngine;

public class FlashedStar : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!(GameManager.Instance.powerUpActive && GameManager.Instance.powerUp == "Flash")) {
            // Destroy itself
            Destroy(gameObject);
        }
    }
}
