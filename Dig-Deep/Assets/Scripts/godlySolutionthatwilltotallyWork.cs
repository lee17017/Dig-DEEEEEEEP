using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;
public class godlySolutionthatwilltotallyWork : MonoBehaviour {

	// Use this for initialization
	void Start () {
        StartCoroutine(WAH());
	}
    IEnumerator WAH()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(1);
    }
}
