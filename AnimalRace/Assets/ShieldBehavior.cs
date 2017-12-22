using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldBehavior : MonoBehaviour {
    public GameObject player;
    public DateTime time;

    // Use this for initialization
    void Start () {
		
	}

    void Update()
    {
        if (DateTime.Now > time + new TimeSpan(0, 0, 5))
            gameObject.SetActive(false);
    }
	
	// Update is called once per frame
	void LateUpdate () {
        transform.position = player.transform.position;
	}
}
