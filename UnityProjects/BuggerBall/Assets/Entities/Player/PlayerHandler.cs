using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandler : MonoBehaviour {

    public GameObject ball;
    public DirectionManager directionManager;

    private Rigidbody2D rigidBody;
    private Animator ballAnimator;
    private Collider2D collider;

    public IsTriggeredHandler leftTrigger;
    public IsTriggeredHandler topTrigger;
    public IsTriggeredHandler rightTrigger;
    public IsTriggeredHandler bottomTrigger;

    public bool isStuck;

    public bool CanJump
    {
        get
        {
            return isStuck;
        }
    }

    // Use this for initialization
    void Start () {
        directionManager = GetComponentInChildren<DirectionManager>();
        rigidBody = GetComponent<Rigidbody2D>();
        ballAnimator = ball.GetComponent<Animator>();
        collider = GetComponent<Collider2D>();
    }

    private Direction stuckToDirections = Direction.Down;

	// Update is called once per frame
	void Update () {

        SetSpritStuckTo(stuckToDirections);

        if(Input.GetMouseButton(0) && isStuck)
        {
            directionManager.IncreaseCurrentJumpPower();
            ballAnimator.SetBool("IsStartingJump", true);
        }
        else if (Input.GetMouseButtonUp(0) && isStuck)
        {
            ballAnimator.SetBool("IsStartingJump", false);
            ballAnimator.SetBool("IsJumping", true);

            //Reenable the gravity on the player
            rigidBody.gravityScale = 1;

            //Perform the jump
            Vector3 jumpDirection = directionManager.GetJumpDirection();

            rigidBody.velocity = jumpDirection.normalized * directionManager.GetCurrentJumpPower();
            
            isStuck = false;

            //Reset 
            directionManager.ResetCurrentJumpPower();
        }
        else if (isStuck)
        {
            ballAnimator.SetBool("IsStartingJump", false);
            ballAnimator.SetBool("IsJumping", false);
            //Arrest movement
            rigidBody.velocity = Vector2.zero;

            //Disable the gravity on the player
            rigidBody.gravityScale = 0;
        }
    }

    private void SetSpritStuckTo(Direction direction)
    {
        float angle = 0;
        switch(direction)
        {
            case Direction.Down:
                angle = 0;
                break;
            case Direction.Left:
                angle = 270;
                break;
            case Direction.Up:
                angle = 180;
                break;
            case Direction.Right:
                angle = 90;
                break;
        }

        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Collider2D collider = collision.collider;
        float RectWidth = this.collider.bounds.size.x;
        float RectHeight = this.collider.bounds.size.y;
        float circleRad = collider.bounds.size.x;

        if (collider != null)
        {
            Vector3 contactPoint = collision.contacts[0].point;
            Vector3 center = collider.bounds.center;

            if (contactPoint.y > center.y && //checks that circle is on top of rectangle
                (contactPoint.x < center.x + RectWidth / 2 && contactPoint.x > center.x - RectWidth / 2))
            {
                stuckToDirections = Direction.Down;
            }
            else if (contactPoint.y < center.y &&
                (contactPoint.x < center.x + RectWidth / 2 && contactPoint.x > center.x - RectWidth / 2))
            {
                stuckToDirections = Direction.Up;
            }
            else if (contactPoint.x > center.x &&
                (contactPoint.y < center.y + RectHeight / 2 && contactPoint.y > center.y - RectHeight / 2))
            {
                stuckToDirections = Direction.Left;
            }
            else if (contactPoint.x < center.x &&
                (contactPoint.y < center.y + RectHeight / 2 && contactPoint.y > center.y - RectHeight / 2))
            {
                stuckToDirections = Direction.Right;
            }
        }

        isStuck = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isStuck = false;
    }

    private void Grow()
    {
        ball.transform.localScale += new Vector3(0.1f, 0.1f, 0.1f);
    }

    
}

public enum Direction
{
    Up,
    Right,
    Down,
    Left
}
