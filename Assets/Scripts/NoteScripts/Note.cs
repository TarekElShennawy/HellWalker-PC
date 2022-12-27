using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    public bool canPress;

    private bool obtainedNote = false;

    public KeyCode keyToPress;

    float goodHitYThreshold = 10f;
    float perfectHitYThreshold = 8.5f;

    //Hit effect Variables
    public GameObject normalEffect, goodEffect, perfectEffect, missedEffect;

    void Update()
    {
        
        //Checks if player pressed button
        if(Input.GetKeyDown(keyToPress))
        {
            //If arrow is in button area, player can press it
            if(canPress)
            {

                obtainedNote = true;

                gameObject.SetActive(false);

                if(Mathf.Abs(transform.position.y) >= goodHitYThreshold)
                {
                    Instantiate(goodEffect, transform.position, goodEffect.transform.rotation);
                    GameManager.instance.GoodHit();

                } else if(Mathf.Abs(transform.position.y) >= perfectHitYThreshold)
                {
                    Instantiate(perfectEffect, transform.position, perfectEffect.transform.rotation);
                    GameManager.instance.PerfectHit();
                    
                }
                else
                {
                    Instantiate(normalEffect, transform.position, normalEffect.transform.rotation);
                    GameManager.instance.GoodHit();
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if(other.tag == "Receiver")
        {
            canPress = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.tag == "Receiver")
        {
            canPress = false;

            if(!obtainedNote)
            {
                Instantiate(missedEffect, transform.position, missedEffect.transform.rotation);
                StartCoroutine(GameManager.instance.MissedNote());
            }
        }
    }
}
