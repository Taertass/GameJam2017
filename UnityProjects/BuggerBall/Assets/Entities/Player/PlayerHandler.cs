using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandler : MonoBehaviour {

    public GameObject ball;
    public DirectionManager directionManager;

    private Rigidbody2D rigidBody;

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
    }
	
	// Update is called once per frame
	void Update () {
        //Lock rotation
        transform.rotation = Quaternion.identity;

        var mousePosition = Input.mousePosition;
        var ballPosition = ball.transform.position;
        var pos = Camera.main.WorldToScreenPoint(transform.position);
        var dir = Input.mousePosition - pos;
        var angle = (Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg) - 90;
        directionManager.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        if(Input.GetMouseButton(0) && isStuck)
        {
            directionManager.IncreaseCurrentJumpPower();
        }
        else if (Input.GetMouseButtonUp(0) && isStuck)
        {
            //Reenable the gravity on the player
            rigidBody.gravityScale = 1;

            //Perform the jump
            rigidBody.velocity = dir.normalized * directionManager.GetCurrentJumpPower();
            
            isStuck = false;

            //Reset 
            directionManager.ResetCurrentJumpPower();
        }
        else if (isStuck)
        {
            //Arrest movement
            rigidBody.velocity = Vector2.zero;

            //Disable the gravity on the player
            rigidBody.gravityScale = 0;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
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
