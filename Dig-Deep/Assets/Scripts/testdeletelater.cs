using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class testdeletelater : MonoBehaviour {

    public int zahl;
	// Use this for initialization
	void Start () {
        StartCoroutine(wait());
	}

    IEnumerator wait()
    {
        yield return new WaitForSeconds(10);
        SceneManager.LoadScene(zahl);
    }
	// Update is called once per frame
	void Update () {
		
	}
}
