using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Payout : MonoBehaviour
{
    private TextMeshProUGUI payoutText;
    public float fadeSpeed = 1f; // How fast it fades out
    public float moveSpeed = 1f; // Speed of movement

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        payoutText = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        // Fade out
        Color color = payoutText.color;
        if (color.a > 0f)
        {
            color.a -= Time.deltaTime * fadeSpeed;
            color.a = Mathf.Clamp01(color.a);
            payoutText.color = color;
        }

        // Destroy when fully faded
        if (color.a <= 0f)
        {
            Destroy(gameObject);
        }

        // Move upwards using anchoredPosition
        RectTransform rt = GetComponent<RectTransform>();
        rt.anchoredPosition += new Vector2(0, moveSpeed * Time.deltaTime);
    }

    public void SetPayout(int amount)
    {
        payoutText = GetComponent<TextMeshProUGUI>();

        string prefix = "+$";

        if (amount < 0)
        {
            // Set the text color to red
            payoutText.color = ColorUtility.TryParseHtmlString("#ff004d", out var c) ? c : Color.white;
            Color color = payoutText.color;
            color.a = 1f;
            payoutText.color = color;
            prefix = "-$";
            amount = -amount;
        }
        else if (amount >= 15)
        {
            // Set the text color to orange
            payoutText.color = ColorUtility.TryParseHtmlString("#ffa300", out var c) ? c : Color.white;
            Color color = payoutText.color;
            color.a = 1f;
            payoutText.color = color;
        }
        else {
            // Set the text color to green
            payoutText.color = ColorUtility.TryParseHtmlString("#00e436", out var c) ? c : Color.white;
            Color color = payoutText.color;
            color.a = 1f;
            payoutText.color = color;
        }

        // Update the payout text with the amount
        payoutText.text = prefix + amount.ToString();
    }
}
