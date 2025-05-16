using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteDuplicatorY : MonoBehaviour
{
    [SerializeField] private int poolSize = 3;
    [SerializeField] private float spriteRepositionCorrection = 0.03f;

    private Transform[] pool;
    private float spriteHeight;

    [SerializeField] private bool isClone = false;

    private bool isInitialized = false;

    void Start()
    {
        if (isClone) return;

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr == null)
        {
            Debug.LogError("Missing SpriteRenderer!");
            return;
        }

        spriteHeight = sr.bounds.size.y;
        pool = new Transform[poolSize];
        pool[0] = transform;

        Vector3 spawnPos = transform.position;

        for (int i = 1; i < poolSize; i++)
        {
            spawnPos.y -= spriteHeight - spriteRepositionCorrection;

            GameObject clone = Instantiate(gameObject, spawnPos, Quaternion.identity, transform.parent);
            
            SpriteDuplicatorY duplicator = clone.GetComponent<SpriteDuplicatorY>();
            if (duplicator != null)
            {
                duplicator.isClone = true;
                duplicator.enabled = false; // disable Update() on clones entirely
            }

            pool[i] = clone.transform;
        }

        isInitialized = true;
    }

    void Update()
    {
        if (!isInitialized || pool == null) return;

        foreach (Transform t in pool)
        {
            if (t == null) continue;

            // When sprite goes too high (off screen), move it to the bottom
            if (t.position.y > Camera.main.transform.position.y + spriteHeight)
            {
                Transform bottom = GetBottomMost();
                if (bottom != null)
                {
                    Vector3 newPos = bottom.position;
                    newPos.y -= spriteHeight - spriteRepositionCorrection;
                    t.position = newPos;
                }
            }
        }
    }

    private Transform GetBottomMost()
    {
        Transform bottom = null;
        float lowestY = Mathf.Infinity;

        foreach (Transform t in pool)
        {
            if (t != null && t.position.y < lowestY)
            {
                lowestY = t.position.y;
                bottom = t;
            }
        }

        return bottom;
    }
}
