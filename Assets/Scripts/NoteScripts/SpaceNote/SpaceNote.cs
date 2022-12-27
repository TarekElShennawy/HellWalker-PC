using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceNote : MonoBehaviour
{
    public bool canPress, isHolding;
    public bool canRelease;

    public bool obtainedNote = false;

    public bool alreadyMissed = false;

    public KeyCode keyToPress;

    public GameObject normalEffect, perfectEffect;


    //TO-DO NOTE: Space holds do not check for misses so theyre always hits, fix it by using the obtainedNote bool + MissedNote method from GameManager

    // Update is called once per frame
    void Update()
    {

        if(canPress)
        {
            if(Input.GetKeyDown(keyToPress))
            {
                obtainedNote = true;
                isHolding = true;
            }
            
        }

        if(Input.GetKeyUp(keyToPress))
        { 

            if(canRelease && isHolding)
            {
                GameManager.instance.PerfectHit();
                Instantiate(perfectEffect, transform.position + new Vector3(0,10,0), perfectEffect.transform.rotation);
            }
            else if(!canRelease && isHolding)
            {
                GameManager.instance.NormalHit();
                Instantiate(normalEffect, transform.position + new Vector3(0,10,0), normalEffect.transform.rotation);
            }
            

            isHolding = false;
        }
    }
    
}
