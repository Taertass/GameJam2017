using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandler : MonoBehaviour {

    public GameObject ball;
    public DirectionManager directionManager;

    private Rigidbody2D rigidBody;
    private Animator ballAnimator;
    private Direction stuckToDirections = Direction.Down;

    //Collision
    public Transform _transform;
    
    public bool isStuck;

    public bool isAlive = true;

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

        _transform = transform;

        rigidBody.velocity += Vector2.down;
    }

	// Update is called once per frame
	void Update () {
        if (!LevelManager.Instance.isGameRunning)
            return;

        CheckCollision();

        if (rigidBody.velocity.magnitude == 0 && !isStuck)
            SetToBeStuck();

        if (!isAlive)
        {
            //Arrest movement
            rigidBody.velocity = Vector2.zero;
            rigidBody.gravityScale = 0;

            return;
        }

        SetSpritStuckTo(stuckToDirections);

        if(Input.GetMouseButton(0) && isStuck)
        {
            directionManager.IncreaseCurrentJumpPower();
            ballAnimator.SetBool("IsStartingJump", true);
        }
        else if (Input.GetMouseButtonUp(0) && isStuck)
        {
            if (!directionManager.CanJumpDirection())
            {
                directionManager.ResetCurrentJumpPower();
                return;
            }

            isStuck = false;

            ballAnimator.SetBool("IsStartingJump", false);
            ballAnimator.SetBool("IsJumping", true);


            //Perform the jump
            Vector3 jumpDirection = directionManager.GetJumpDirection();

            rigidBody.velocity = jumpDirection.normalized * directionManager.GetCurrentJumpPower();

            
            
            //Reset 
            directionManager.ResetCurrentJumpPower();

            if(SoundHandler.Instance != null)
                SoundHandler.Instance.PlayJumpClip();

            //Reenable movement on the player
            rigidBody.constraints = RigidbodyConstraints2D.None;
        }
        else if (isStuck)
        {
            //Arrest movement
            rigidBody.velocity = Vector2.zero;            
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
        string tag = string.Empty;
        if (collision.gameObject != null)
            tag = collision.gameObject.tag;

        if(string.Equals(tag, "NoneStick"))
        {
            if (SoundHandler.Instance != null)
                SoundHandler.Instance.PlayNoneStickImpactClip();
            isStuck = false;
            return;
        }

        if (isStuck)
            return;

        SetToBeStuck();



    }

    private void SetToBeStuck()
    {
        if(SoundHandler.Instance != null)
            SoundHandler.Instance.PlayImpactClip();

        isStuck = true;

        //Freeze player
        rigidBody.constraints = RigidbodyConstraints2D.FreezeAll;

        ballAnimator.SetBool("IsStartingJump", false);
        ballAnimator.SetBool("IsJumping", false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        string tag = string.Empty;
        if(collision.gameObject != null)
            tag = collision.gameObject.tag;
        
        if (string.Equals(tag, "Kill", System.StringComparison.InvariantCultureIgnoreCase))
        {
            isAlive = false;
            ballAnimator.SetBool("IsAlive", false);

            Invoke("LoseGame", 2.5f);

            if (SoundHandler.Instance != null)
                SoundHandler.Instance.PlayHurtClip();
        }
        else
        {
            var collectable = collision.gameObject.GetComponent<Collectable>();

            if (collectable != null)
            {
                Destroy(collectable.gameObject);
                LevelManager.Instance.Score = LevelManager.Instance.Score + 1;
                Grow();

                if (SoundHandler.Instance != null)
                    SoundHandler.Instance.PlayUpgradeSound();
            }
        }
    }

    private void LoseGame()
    {
        LevelManager.Instance.LoseLevel();
    }

    private void Grow()
    {
        GetComponent<BoxCollider2D>().offset += new Vector2(0, 0.1f);
        hitLimitDistance += 0.2f;
        GetComponent<BoxCollider2D>().size += new Vector2(0.2f, 0.2f);
        ball.transform.localScale += new Vector3(0.3f, 0.3f, 0.3f);

        directionManager.IncreaseMaxJumpPower();
    }

    private float hitLimitDistance = 0.6f;

    private void CheckCollision()
    {
        RaycastHit2D hitUp = Physics2D.Raycast(_transform.position, Vector3.up, hitLimitDistance);
        RaycastHit2D hitRight = Physics2D.Raycast(_transform.position, Vector3.right, hitLimitDistance);
        RaycastHit2D hitDown = Physics2D.Raycast(_transform.position, Vector3.down, hitLimitDistance);
        RaycastHit2D hitLeft = Physics2D.Raycast(_transform.position, Vector3.left, hitLimitDistance);

        if (hitUp != null && hitUp.collider != null)
        {
            //Debug.DrawRay(_transform.position, Vector3.up, Color.red, 20, true);
            stuckToDirections = Direction.Up;
        }
        if (hitRight != null && hitRight.collider != null)
        {
            //Debug.DrawRay(_transform.position, Vector3.right, Color.red, 20, true);
            stuckToDirections = Direction.Right;
        }
        if (hitDown != null && hitDown.collider != null)
        {
            //Debug.DrawRay(_transform.position, Vector3.down, Color.red, 20, true);
            stuckToDirections = Direction.Down;
        }
        if (hitLeft != null && hitLeft.collider != null)
        {
            //Debug.DrawRay(_transform.position, Vector3.left, Color.red, 20, true);
            stuckToDirections = Direction.Left;
        }

    }
}

public enum Direction
{
    Up,
    Right,
    Down,
    Left
}
