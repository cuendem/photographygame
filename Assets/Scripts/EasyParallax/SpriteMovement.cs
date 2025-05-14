using UnityEngine;

namespace EasyParallax
{
    public enum Direction
    {
        Left,
        Right,
        Up,
        Down
    }

    /**
     * Moves a sprite along a selected direction using a predefined speed
     */
    public class SpriteMovement : MonoBehaviour
    {
        public MovementSpeedType movementSpeedType;

        [Tooltip("Used only if no movement speed type is specified")]
        public float speed = 1f;

        [Tooltip("Direction in which the sprite moves")]
        public Direction movementDirection = Direction.Left;

        private void Awake()
        {
            if (movementSpeedType)
                speed = movementSpeedType.speed;
        }

        private void Update()
        {
            Vector3 newPosition = transform.position;

            switch (movementDirection)
            {
                case Direction.Left:
                    newPosition.x -= speed * Time.deltaTime;
                    break;
                case Direction.Right:
                    newPosition.x += speed * Time.deltaTime;
                    break;
                case Direction.Up:
                    newPosition.y += speed * Time.deltaTime;
                    break;
                case Direction.Down:
                    newPosition.y -= speed * Time.deltaTime;
                    break;
            }

            transform.position = newPosition;
        }
    }
}
