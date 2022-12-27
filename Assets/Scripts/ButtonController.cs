using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    private SpriteRenderer spriteSR;
    public Sprite defaultImg;
    public Sprite clickedImg;

    public KeyCode keyToPress;

    // Start is called before the first frame update
    void Start()
    {
        spriteSR = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(keyToPress))
        {
            spriteSR.sprite = clickedImg;
        }

        if(Input.GetKeyUp(keyToPress))
        {
            spriteSR.sprite = defaultImg;
        }
    }
}
