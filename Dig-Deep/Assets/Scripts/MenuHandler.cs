using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class MenuHandler : MonoBehaviour {

	
    public UnityEvent p1pressedA, p2pressedA, p1pressedB, p2pressedB, transition;
    bool p1Done=false, p2Done=false, done = false;

    
    // Update is called once per frame
    void Update () {
        
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
            transition.Invoke();
            StartCoroutine(nextLevel(5));
        }

    }

    IEnumerator nextLevel(float time)
    {       
        yield return new WaitForSeconds(time);        
        SceneManager.LoadScene(1);
    }
}
