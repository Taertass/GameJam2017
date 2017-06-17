using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MovementType
{
    Horizontal,
    Vertical,
    Circle
}

public class MovingEnemyScript : MonoBehaviour {

    public MovementType MovementType = MovementType.Horizontal;
    
    public float speed = 0.5f;
    public bool isMoving = true;
    public Direction direction = Direction.Right;

    private Vector3 lastPosition;
    private SpriteRenderer spriteRenderer;

    // Use this for initialization
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (direction == Direction.Left)
        {
            spriteRenderer.flipX = true;
        }

        if (MovementType == MovementType.Horizontal)
        {
            if (direction != Direction.Left && direction != Direction.Right)
                direction = Direction.Right;
        }
        if (MovementType == MovementType.Vertical)
        {
            if (direction != Direction.Up && direction != Direction.Down)
                direction = Direction.Up;
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (!isMoving)
            return;

        if(direction == Direction.Right)
        {
            transform.Translate(new Vector3(1 * speed * Time.deltaTime, 0, 0));
        }
        else if (direction == Direction.Left)
        {
            transform.Translate(new Vector3(-1 * speed * Time.deltaTime, 0, 0));
        }
        else if (direction == Direction.Up)
        {
            transform.Translate(new Vector3(0, 1 * speed * Time.deltaTime, 0));
        }
        else if (direction == Direction.Down)
        {
            transform.Translate(new Vector3(0, -1 * speed * Time.deltaTime, 0));
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision");
    }

    private float lastDirectionChange = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(string.Equals(collision.tag, "Player", System.StringComparison.InvariantCultureIgnoreCase))
        {
            isMoving = false;
        }
        else
        {
            if (lastDirectionChange != 0 && Time.time - lastDirectionChange < 0.01f)
                return;

            lastDirectionChange = Time.time;

            if(MovementType == MovementType.Horizontal)
            {
                if (direction == Direction.Right)
                {
                    direction = Direction.Left;
                    spriteRenderer.flipX = true;
                }
                else
                {
                    direction = Direction.Right;
                    spriteRenderer.flipX = false;
                }
            }
            else if (MovementType == MovementType.Vertical)
            {
                if (direction == Direction.Up)
                {
                    direction = Direction.Down;
                }
                else
                {
                    direction = Direction.Up;
                }
            }

        }
    }
}
