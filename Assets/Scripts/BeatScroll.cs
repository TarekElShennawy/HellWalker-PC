using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatScroll : MonoBehaviour
{
    public float tempo;
    public bool roundBegun;
    
    // Start is called before the first frame update
    void Start()
    {
        tempo = tempo / 60f; //beat tempo moves at 2 beats per second for 120 bpm 
    }

    // Update is called once per frame
    void Update()
    {
        if(!roundBegun)
        {
            if(Input.anyKeyDown)
            {
                roundBegun = true;
            }
        }
        else
        {
            transform.position -= new Vector3(0f, tempo * Time.deltaTime, 0f);
        }
    }
}
