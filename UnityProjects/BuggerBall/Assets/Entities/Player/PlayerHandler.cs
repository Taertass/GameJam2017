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

    //Collision
    private float circleRadius = 0.5f;
    private Vector3 previousPosition, directionV;
    Transform _transform;


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
        myAudioSource = GetComponent<AudioSource>();

        _transform = transform;
        previousPosition = transform.position;
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
        if (isStuck)
            return;

        isStuck = true;

        //Freeze player
        rigidBody.constraints = RigidbodyConstraints2D.FreezeAll;

        ballAnimator.SetBool("IsStartingJump", false);
        ballAnimator.SetBool("IsJumping", false);

        CheckCollision();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var collectable = collision.gameObject.GetComponent<Collectable>();

        if(collectable != null)
        {
            Destroy(collectable.gameObject);
            LevelManager.Instance.Score = LevelManager.Instance.Score + 1;
            Grow();

            if(upgradeClips != null && upgradeClips.Length > 0)
                myAudioSource.clip = upgradeClips[0];

            myAudioSource.Play();
        }
    }

    private void Grow()
    {
        ball.transform.localScale += new Vector3(0.2f, 0.2f, 0.2f);
    }

    private void CheckCollision()
    {
        if (_transform.position == previousPosition)
            return;

        directionV = _transform.position - previousPosition;
        RaycastHit2D hit = Physics2D.Raycast(_transform.position, directionV, circleRadius);
        if (hit.collider != null)
        {
            if (hit.normal.x != 0f)
            {
                if (hit.normal.x > 0f)
                    stuckToDirections = Direction.Left;
                else
                    stuckToDirections = Direction.Right;
            }
            else if (hit.normal.y != 0f)
            {
                if (hit.normal.y > 0f)
                    stuckToDirections = Direction.Down;
                else
                    stuckToDirections = Direction.Up;
            }
        }

        previousPosition = _transform.position;
    }
}

public enum Direction
{
    Up,
    Right,
    Down,
    Left
}
