using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopNote : MonoBehaviour
{
    public SpaceNote noteParent;

    private void OnTriggerEnter2D(Collider2D other)
    {

        if(other.tag == "Receiver")
        {
            noteParent.canRelease = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {

        if(other.tag == "Receiver")
        {
            noteParent.canRelease = false;

            if(noteParent.obtainedNote && !noteParent.alreadyMissed)
            {
                GameManager.instance.MissedNote();
            }
        }
    }
}
