using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottomNote : MonoBehaviour
{
    public SpaceNote noteParent;

    public KeyCode keyToPress;

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if(other.tag == "Receiver")
        {
            noteParent.canPress = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.tag == "Receiver")
        {
            noteParent.canPress = false;

            if(!noteParent.obtainedNote)
            {
                StartCoroutine(GameManager.instance.MissedNote());
                noteParent.alreadyMissed = true;
            }
        }
    }

    
}
