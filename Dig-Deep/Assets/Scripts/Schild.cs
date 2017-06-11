using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Schild : MonoBehaviour {


    public GameObject schild;

    public GameObject[] start_end;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void swap_start_end()
    {
        GameObject cache = start_end[0];
    }

    IEnumerator inAndOut()
    {
        yield return null;
    }
}
