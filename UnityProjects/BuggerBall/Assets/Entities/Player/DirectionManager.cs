using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionManager : MonoBehaviour {

    private Vector3 idleScale = new Vector3(0.3f, 0.3f, 0.3f);
    private PlayerHandler playerHandler;

    private float maxJumpPower = 10f;
    private float minJumpPower = 1f;
    private float currentJumpPower = 1f;
    private float jumpPowerGrowthSpeed = 10f;

    private bool isJumpPowerGrowing = true;
    private Vector3 jumpDirection;

    void Start () {
        transform.localScale = idleScale;
        playerHandler = GetComponentInParent<PlayerHandler>();
    }
	
	// Update is called once per frame
	void Update () {

        //Set rotation of the arrow to towards the mouse cursor
        var mousePosition = Input.mousePosition;
        var pos = Camera.main.WorldToScreenPoint(playerHandler.transform.position);
        jumpDirection = Input.mousePosition - pos;
        var angle = (Mathf.Atan2(jumpDirection.y, jumpDirection.x) * Mathf.Rad2Deg) - 90;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        //Show currentJumpPower as localScale
        var currentScaleIndex = currentJumpPower / maxJumpPower;
        transform.localScale = new Vector3(currentScaleIndex, currentScaleIndex, currentScaleIndex);
    }

    public void IncreaseCurrentJumpPower()
    {
        //Increase the currentJumpPower
        if (isJumpPowerGrowing)
        {
            if (currentJumpPower < maxJumpPower)
                currentJumpPower += jumpPowerGrowthSpeed * Time.deltaTime;

            else if (currentJumpPower >= maxJumpPower)
            {
                currentJumpPower = maxJumpPower;
                isJumpPowerGrowing = false;
            }
        }
        else
        {
            if (currentJumpPower > minJumpPower)
                currentJumpPower -= jumpPowerGrowthSpeed * Time.deltaTime;

            else if (currentJumpPower <= minJumpPower)
            {
                currentJumpPower = minJumpPower;
                isJumpPowerGrowing = true;
            }
        }
    }

    internal bool CanJumpDirection()
    {
        RaycastHit2D hit = Physics2D.Raycast(playerHandler.ball.transform.position, GetJumpDirection(), 0.1f);
        bool canJump = hit.transform == null || hit.transform.gameObject.tag == "Player";

        return canJump;
    }

    public void UpdateLocalScaleWithJumpPower()
    {

    }

    public void ResetCurrentJumpPower()
    {
        //Reset 
        currentJumpPower = minJumpPower;
        isJumpPowerGrowing = true;
    }
    public float GetCurrentJumpPower()
    {
        return currentJumpPower;
    }

    public Vector3 GetJumpDirection()
    {
        return jumpDirection;
    }

    public void IncreaseMaxJumpPower()
    {
        maxJumpPower += 3f;
    }
}
