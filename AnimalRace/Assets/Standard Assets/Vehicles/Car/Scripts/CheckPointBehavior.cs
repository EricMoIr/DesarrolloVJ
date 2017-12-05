using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointBehavior : MonoBehaviour {

    public int nextPos;
    public bool isLast;

	// Use this for initialization
	void Start () {
        nextPos = 1;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void AddNextPos(int num)
    {
        this.nextPos += num;
    }
}
