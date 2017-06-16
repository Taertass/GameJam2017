using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandler : MonoBehaviour {

    public GameObject ball;
    public GameObject arrow;

    private Rigidbody2D rigidBody;

    private float maxJumpPower = 10f;
    private float minJumpPower = 1f;
    private float currentJumpPower = 1f;
    private float jumpPowerGrowthSpeed = 5f;

    // Use this for initialization
    void Start () {
        rigidBody = GetComponent<Rigidbody2D>();
        arrow.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
    }
	
	// Update is called once per frame
	void Update () {
        transform.rotation = Quaternion.identity;

        var mousePosition = Input.mousePosition;

        var ballPosition = ball.transform.position;

        var pos = Camera.main.WorldToScreenPoint(transform.position);
        var dir = Input.mousePosition - pos;
        var angle = (Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg) - 90;
        arrow.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        if(Input.GetMouseButton(0) && isStuck)
        {
            if (currentJumpPower < maxJumpPower)
                currentJumpPower += jumpPowerGrowthSpeed * Time.deltaTime;
            var currentScaleIndex = currentJumpPower / maxJumpPower;

            var currentScale = arrow.transform.localScale;
            Debug.Log(currentJumpPower);
            Debug.Log(currentScaleIndex);
            arrow.transform.localScale = new Vector3(currentScaleIndex, currentScaleIndex, currentScaleIndex);
        }
        else if (Input.GetMouseButtonUp(0) && isStuck)
        {
            rigidBody.velocity = dir.normalized * currentJumpPower;
            rigidBody.gravityScale = 1;
            isStuck = false;
            arrow.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            currentJumpPower = minJumpPower;
        }
        else if (isStuck)
        {
            rigidBody.velocity = Vector2.zero;
            rigidBody.gravityScale = 0;
        }
    }


    private bool isStuck;

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
