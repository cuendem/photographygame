using UnityEngine;

namespace EasyParallax
{
    /**
     * Creates vertical duplicates of this sprite going downward and recycles them as they scroll out of view.
     */
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteDuplicatorY : MonoBehaviour
    {
        [SerializeField] private int poolSize = 5;

        [Tooltip("Determines how far down the sprite can move before being recycled")]
        [SerializeField] private int spriteRepositionIndex = 2;

        [Tooltip("Adjust this if there are visible gaps or overlaps between sprites")]
        [SerializeField] private float spriteRepositionCorrection = 0.03f;

        private Transform[] duplicatesPool;
        private float spriteHeight;

        private void Start()
        {
            duplicatesPool = new Transform[poolSize];
            spriteHeight = GetComponent<SpriteRenderer>().bounds.size.y;
            duplicatesPool[0] = transform;

            var startingPos = transform.position;

            for (int i = 1; i < poolSize; i++)
            {
                var position = new Vector2(startingPos.x, CalculateYDownward(startingPos));
                startingPos = position;

                duplicatesPool[i] = Instantiate(gameObject, position, Quaternion.identity, transform.parent).transform;
                Destroy(duplicatesPool[i].GetComponent<SpriteDuplicator>());
            }
        }

        private void Update()
        {
            foreach (var duplicate in duplicatesPool)
            {
                if (duplicate.transform.position.y < -spriteHeight * spriteRepositionIndex)
                {
                    var bottomSprite = GetBottomMostSprite();
                    var startingPos = bottomSprite.position;
                    var position = new Vector2(startingPos.x, CalculateYDownward(startingPos));
                    duplicate.transform.position = position;
                }
            }
        }

        private float CalculateYDownward(Vector3 startingPos)
        {
            return Mathf.FloorToInt(startingPos.y - spriteHeight) +
                   spriteRepositionCorrection * transform.lossyScale.magnitude;
        }

        private Transform GetBottomMostSprite()
        {
            var lowestY = Mathf.Infinity;
            Transform bottomSprite = null;

            foreach (var duplicate in duplicatesPool)
            {
                if (duplicate.position.y < lowestY)
                {
                    bottomSprite = duplicate;
                    lowestY = duplicate.position.y;
                }
            }

            return bottomSprite;
        }
    }
}
