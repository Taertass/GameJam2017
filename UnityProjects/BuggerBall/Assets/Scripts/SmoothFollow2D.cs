using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothFollow2D : MonoBehaviour {

    public float FollowSpeed = 2f;
    public Transform Target;

    private Camera myCamera;
    private float startTime = 0;

    private void Start()
    {
        myCamera = GetComponent<Camera>();
        
    }

    private void Update()
    {
        if(startTime == 0)
            startTime = Time.time;

        Vector3 newPosition = Target.position;
        newPosition.z = -10;
        transform.position = Vector3.Slerp(transform.position, newPosition, FollowSpeed * Time.deltaTime);

        if (Time.time - startTime > 0.5f)
        {
            if (myCamera.orthographicSize > 4)
                myCamera.orthographicSize -= 2f * Time.deltaTime;
            else if (myCamera.orthographicSize != 4)
                myCamera.orthographicSize = 4;
        }
    }
}
