using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsTriggeredHandler : MonoBehaviour {

    public bool IsTriggered;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IsTriggered = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        IsTriggered = false;
    }
}
