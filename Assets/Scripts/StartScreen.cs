using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class StartScreen : MonoBehaviour
{
    public Animator tooltip, titleText;
    public SpriteRenderer background;

    // Update is called once per frame
    void Update()
    {

        if(Input.GetKeyUp("space"))
        {
            tooltip.SetBool("DisappearTip", true);
            titleText.SetBool("DisappearTitle", true);
            background.color = new Color(255, 0, 0);

            StartCoroutine(StartGame());
            
        }
    }

    private IEnumerator StartGame()
    {
        
        yield return new WaitForSeconds(0.8f);
        SceneManager.LoadScene("MainScene");
    }
}
