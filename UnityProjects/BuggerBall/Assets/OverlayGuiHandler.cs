using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlayGuiHandler : MonoBehaviour {

    public UnityEngine.UI.Text scoreText;
    public UnityEngine.UI.Text levelText;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        scoreText.text = "Score " + LevelManager.Instance.Score;

    }
}
