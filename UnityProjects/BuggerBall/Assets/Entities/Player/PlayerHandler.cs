using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandler : MonoBehaviour {

    public GameObject ball;
    public DirectionManager directionManager;

    private Rigidbody2D rigidBody;
    private Animator ballAnimator;

    //public float maxJumpPower = 10f;
    //public float minJumpPower = 1f;
    //public float currentJumpPower = 1f;
    //public float jumpPowerGrowthSpeed = 5f;
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
        var direction = ReturnDirection(this.gameObject, collision.gameObject);
        //if(direction.y < 0)
        //{
        //    stuckToDirections = Direction.Up;
        //} else if(direction > 0)
        //{
        //    stuckToDirections
        //}
        Debug.Log(direction);
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

    private Direction ReturnDirection(GameObject Object, GameObject ObjectHit)
    {
        Direction hitDirection = Direction.Down;
        RaycastHit MyRayHit;
        Vector3 direction = (Object.transform.position - ObjectHit.transform.position).normalized;
        Ray MyRay = new Ray(ObjectHit.transform.position, direction);

        

        if (Physics.Raycast(MyRay, out MyRayHit))
        {
            
            if (MyRayHit.collider != null)
            {
                Vector3 MyNormal = MyRayHit.normal;
                MyNormal = MyRayHit.transform.TransformDirection(MyNormal);

                if (MyNormal == MyRayHit.transform.up) { hitDirection = Direction.Up; }
                if (MyNormal == -MyRayHit.transform.up) { hitDirection = Direction.Down; }
                if (MyNormal == MyRayHit.transform.right) { hitDirection = Direction.Right; }
                if (MyNormal == -MyRayHit.transform.right) { hitDirection = Direction.Left; }
            }
        }

        Debug.Log(direction);
        Debug.Log(MyRay);
        Debug.Log(hitDirection);

        return hitDirection;
    }
}

public enum Direction
{
    Up,
    Right,
    Down,
    Left
}
