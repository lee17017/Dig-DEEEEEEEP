using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowGameOver : MonoBehaviour {

    public Text gOver;

    private bool display = false;

    private void Awake()
    {
        gOver.gameObject.SetActive(false);
    }
    // Update is called once per frame
    void Update () {
        if (!GameManager.current.run && !display)
        {
            gOver.gameObject.SetActive(true);
            display = true;
        }
	}
}
