using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tunnelbohralgorithmus : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
    int startLane;
    bool mid = true;
    bool switching = false;
	void Update () {
        if (!switching)
        {
            if (Input.GetButtonDown("Fire2")/*Input rechts (5)*/&& transform.position.x < startLane + 3)
            {
                //Move to the right (player view)
                switching = true;
                transform.Rotate(new Vector3(0, 0, 45), Space.Self);
            }
            if (Input.GetButtonDown("Fire1")/*Input links (4)*/&&transform.position.x>startLane-3)
            {
                //Move to the left (player view)
                switching = true;
                transform.Rotate(new Vector3(0, 0, -45), Space.Self);
            }
        }
        if (switching)
        {
            if (mid&&(transform.position.x>=startLane+5||transform.position.x<=startLane-5))
            {
                mid = false;
                switching = false;
                transform.rotation.SetEulerAngles(0, 0, 0);
            }
            else if (!mid && transform.position.x < startLane + 0.5f && transform.position.x > startLane - 0.5f)
            {
                mid = true;
                switching = false;
                transform.rotation.SetEulerAngles(0, 0, 0);
            }
        }
        //transform.Translate(Vector3.down, Space.Self);
	}
}
