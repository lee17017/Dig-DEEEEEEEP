using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class MenuHandler : MonoBehaviour {

	
    public UnityEvent p1pressedA, p2pressedA, p1pressedB, p2pressedB, transition;
    bool p1Done=false, p2Done=false, done = false;

    public GameObject drill;

    public bool trans;
    
    // Update is called once per frame
    void Update () {
        if (trans)
        {
            StartCoroutine(nextLevel());
        }


        if(Input.GetKeyDown("joystick 1 button 0") && !p1Done && !done )
        {
            p1pressedA.Invoke();
            p1Done = !p1Done;
        }

        if (Input.GetKeyDown("joystick 2 button 0") && !p2Done && !done)
        {
            p2pressedA.Invoke();
            p2Done = !p2Done;
        }

        if(Input.GetKeyDown("joystick 1 button 1") && p1Done && !done)
        {
            p1pressedB.Invoke();
            p1Done = !p1Done;
        }

        if (Input.GetKeyDown("joystick 2 button 1") && p2Done && !done)
        {
            p2pressedB.Invoke();
            p2Done = !p2Done;
           
        }

        if(p1Done && p2Done)
        {
            done = !done;
            StartCoroutine(nextLevel());
        }

    }

    IEnumerator nextLevel()
    {       
        
        drill.GetComponent<MoveSprite>().moveDrill();
        yield return new WaitForSeconds(2);
        transition.Invoke();

        yield return new WaitForSeconds(5);
        SceneManager.LoadScene(1);
    }
}
