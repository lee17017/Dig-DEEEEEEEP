using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Clockcode : MonoBehaviour {

	void Update () {
		//Stoppuhr
        GetComponent<Image>().fillAmount = GameManager.current.clockPercentage;
	}
}
