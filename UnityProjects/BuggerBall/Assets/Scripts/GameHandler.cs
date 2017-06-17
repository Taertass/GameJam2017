using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour {

    private static GameHandler instance;
    public static GameHandler Instance
    {
        get
        {
            return instance;
        }
    }


	void Start () {
        instance = this;
        DontDestroyOnLoad(this);
	}
}
