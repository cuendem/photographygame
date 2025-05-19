using UnityEngine;
using UnityEngine.UI;

public class Flash : MonoBehaviour
{
    private Image flashImage;
    public float fadeSpeed = 5f; // How fast it fades out

    private void Start()
    {
        flashImage = GetComponent<Image>();
        flashImage.color = new Color(1f, 1f, 1f, 0f); // Fully transparent
    }

    public void TriggerFlash()
    {
        flashImage.color = new Color(1f, 1f, 1f, 1f); // Fully visible
    }

    private void Update()
    {
        Color color = flashImage.color;
        if (color.a > 0f)
        {
            color.a -= Time.deltaTime * fadeSpeed;
            color.a = Mathf.Clamp01(color.a); // Ensure it stays between 0 and 1
            flashImage.color = color;
        }
    }
}
