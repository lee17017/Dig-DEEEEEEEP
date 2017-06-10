using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeSprite : MonoBehaviour {


    public Sprite red;
    public Sprite green;

    public Text pressA;

    private bool state = false; /* false = red true=green*/

    public bool test;

    

    void Update()
    {
        if (test)
        {
            test = false;
            toggleSprite();
            
        }
    }

    /*
     changes the button sprite from red to green 
         */
    public void toggleSprite()
    {
        if (state)
        {
            toRED();
            pressA.gameObject.SetActive(true);
        }
        else
        {
            toGreen();
            pressA.gameObject.SetActive(false);
        }

        state = !state;
    }

    private void toRED()
    {
        this.GetComponent<Image>().sprite = red;
    }

    private void toGreen()
    {
        this.GetComponent<Image>().sprite = green;
    }
}
