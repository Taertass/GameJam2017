using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (string.Equals(collision.gameObject.tag, "Player", System.StringComparison.InvariantCultureIgnoreCase))
        {
            LevelManager.Instance.TryAndWinCheck();
        }
    }
}
