using UnityEngine;
using UnityEngine.UI;

public class PowerBar : MonoBehaviour
{
    private RectTransform rt;
    private float duration = 5f;

    private float timer;
    private bool isShrinking = false;

    void Start()
    {
        // Get the RectTransform component of the progress image
        rt = GetComponent<RectTransform>();
        timer = duration;
    }

    void Update()
    {
        if (timer > 0 && isShrinking)
        {
            timer -= Time.deltaTime;
            float fillAmount = Mathf.Clamp01(timer / duration);

            // Update scale X (shrinks from full to 0)
            rt.localScale = new Vector3(fillAmount, 1f, 1f);
        }
    }

    public void ResetTimer()
    {
        rt.localScale = new Vector3(1f, 1f, 1f); // Reset scale to full
        isShrinking = false;
    }

    public void SetTimer(float newDuration)
    {
        duration = newDuration;
        timer = duration;
        isShrinking = true;
    }
}
