using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandler : MonoBehaviour {

    public GameObject ball;
    public DirectionManager directionManager;

    public AudioClip[] jumpClips;
    public AudioClip[] impactClips;
    public AudioClip[] impactNoneStickClips;
    public AudioClip[] hurtClips;
    public AudioClip[] upgradeClips;

    private Rigidbody2D rigidBody;
    private Animator ballAnimator;
    private AudioSource myAudioSource;
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
        myAudioSource = GetComponent<AudioSource>();

        _transform = transform;
    }

	// Update is called once per frame
	void Update () {
        if (!LevelManager.Instance.isGameRunning)
            return;

        CheckCollision();

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
        else if (Input.GetMouseButtonUp(0))
        {
            if (!directionManager.CanJumpDirection())
                return;

            isStuck = false;

            ballAnimator.SetBool("IsStartingJump", false);
            ballAnimator.SetBool("IsJumping", true);


            //Perform the jump
            Vector3 jumpDirection = directionManager.GetJumpDirection();

            rigidBody.velocity = jumpDirection.normalized * directionManager.GetCurrentJumpPower();

            
            
            //Reset 
            directionManager.ResetCurrentJumpPower();

            if (jumpClips != null && jumpClips.Length > 0)
                myAudioSource.clip = jumpClips[0];

            myAudioSource.Play();

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
            isStuck = false;
            return;
        }

        if (isStuck)
            return;
        
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

            Invoke("LoseGame", 3);
        }
        else
        {
            var collectable = collision.gameObject.GetComponent<Collectable>();

            if (collectable != null)
            {
                Destroy(collectable.gameObject);
                LevelManager.Instance.Score = LevelManager.Instance.Score + 1;
                Grow();

                if (upgradeClips != null && upgradeClips.Length > 0)
                    myAudioSource.clip = upgradeClips[0];

                myAudioSource.Play();
            }
        }
    }

    private void LoseGame()
    {
        LevelManager.Instance.LoseLevel();
    }

    private void Grow()
    {
        ball.transform.localScale += new Vector3(0.2f, 0.2f, 0.2f);
    }

    private void CheckCollision()
    {
        RaycastHit2D hitUp = Physics2D.Raycast(_transform.position, Vector3.up, 0.5f);
        RaycastHit2D hitRight = Physics2D.Raycast(_transform.position, Vector3.right, 0.5f);
        RaycastHit2D hitDown = Physics2D.Raycast(_transform.position, Vector3.down, 0.5f);
        RaycastHit2D hitLeft = Physics2D.Raycast(_transform.position, Vector3.left, 0.5f);

        if (hitUp != null && hitUp.collider != null)
            stuckToDirections = Direction.Up;
        if (hitRight != null && hitRight.collider != null)
            stuckToDirections = Direction.Right;
        if (hitDown != null && hitDown.collider != null)
            stuckToDirections = Direction.Down;
        if (hitLeft != null && hitLeft.collider != null)
            stuckToDirections = Direction.Left;

    }
}

public enum Direction
{
    Up,
    Right,
    Down,
    Left
}
