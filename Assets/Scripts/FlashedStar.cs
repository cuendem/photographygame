using UnityEngine;

public class FlashedStar : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (!(GameManager.Instance.powerUpActive && GameManager.Instance.powerUp == "Flash")) {
            // Destroy itself
            Destroy(gameObject);
        }
    }
}
