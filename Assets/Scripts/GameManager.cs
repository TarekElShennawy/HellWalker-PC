using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using MidiPlayerTK;
using TMPro;

public class GameManager : MonoBehaviour
{
    //Game Control Variables
    public bool startGame;
    public BeatScroll beatScroller;
    public static GameManager instance;
    private float originalTempo;
    private bool missedNote;
    private bool canRestart = false;

    //Score Variables
    public int currentScore;
    private int scorePerHit = 100;
    private int scorePerGoodHit = 125;
    private int scorePerPerfectHit = 150;

    //Monster gameobject list for spawning
    public GameObject[] monsters;

    //Character animator
    public Animator charAnimator;

    //Multiplier Variables
    public int currentMultiplier;
    public int multiplierTracker;
    public int[] multiplierThresholds;
    
    public MidiFilePlayer midiFilePlayer;

    //UI Variables
    public TMP_Text scoreText;
    public TMP_Text multiplierText;


    //Result Variables
    public GameObject resultsUI;
    public TMP_Text normalHitTxt, goodHitTxt, perfectHitTxt, missedHitTxt, totalScoreTxt, percentileHitTxt, rankTxt, finalScoreTxt;
    public float totalNotes;
    public int normalHits, goodHits, perfectHits, missedHits;
    private int missesLimit;

    void Start()
    {
        instance = this;
        currentMultiplier = 1;

        originalTempo = beatScroller.tempo;
        missesLimit = 3;

        //Returning the number of notes at the start of the game 
        totalNotes = FindObjectsOfType<Note>().Length + FindObjectsOfType<SpaceNote>().Length;
    }

    void Update()   
    {
        if(!startGame)
        {
            StartRound();
        }

        //If player finishes game or loses (missing notes)
        if(!midiFilePlayer.MPTK_IsPlaying && !resultsUI.activeInHierarchy && !missedNote || missedHits >= missesLimit && !resultsUI.activeInHierarchy) //TAREK NOTE: Keep note of the active in hierarchy property, very useful
            {
                //Allow player to restart
                canRestart = true;

                //Switch music off
                midiFilePlayer.MPTK_Stop();
                
                //Stop beatscroller operating
                beatScroller.tempo = 0;

                //Removing notes and buttons
                GameObject.Find("BeatController").SetActive(false);
                GameObject.Find("ButtonParent").SetActive(false);

                //Removing score and multiplier text
                multiplierText.text = "";
                scoreText.text = "";

                //Playing character falling animation
                StartCoroutine(PlayDeathAnim());

                normalHitTxt.text = normalHits.ToString();
                goodHitTxt.text = goodHits.ToString();
                perfectHitTxt.text = perfectHits.ToString();
                missedHitTxt.text = missedHits.ToString();

                //Calculating totals
                float totalHit = normalHits + goodHits + perfectHits;
                float percentileHit = (totalHit/totalNotes) * 100f;

                percentileHitTxt.text = percentileHit.ToString("F1") + "%";

                //Returning player ranking. TAREK NOTE: You will think that switch statements are easier than doing the "if limbo" below, this is not possible and the code below is more performance-friendly
                string rankVal = "F";

                if(percentileHit > 40)
                {
                    rankVal = "D";
                    if(percentileHit > 55)
                    {
                        rankVal = "C";
                        if(percentileHit > 70)
                        {
                            rankVal = "B";
                            if(percentileHit > 85)
                            {
                                rankVal = "A";
                                if(percentileHit > 95)
                                {
                                    rankVal = "S";
                                }
                            }
                        }
                    }
                }

                rankTxt.text = rankVal;

                finalScoreTxt.text = currentScore.ToString();
            }
        
        if(canRestart && Input.GetKeyUp("space"))
            {
                Scene scene = SceneManager.GetActiveScene();
                SceneManager.LoadScene(scene.name);
            }
        
    }

    public void StartRound()
    {
        startGame = true;
        beatScroller.roundBegun = true;
        midiFilePlayer.MPTK_Play();
    }

    //Methods that define whether a note is hit or missed

    public void HitNote()
    {
        //Multiplier logic
        if(currentMultiplier - 1 < multiplierThresholds.Length)
        {
            multiplierTracker++;

            if(multiplierThresholds[currentMultiplier - 1] <= multiplierTracker)
            {
                multiplierTracker = 0;
                
                currentMultiplier++;

                //TAREK TO-DO: Hard-coded tempo and speed control, research better ways to implement that
                midiFilePlayer.MPTK_Speed += 0.2f;
                beatScroller.tempo += 0.5f;
            }
        }
        
        multiplierText.text = "X" + currentMultiplier.ToString();
        scoreText.text = "Score: " + currentScore.ToString();
    }

    public IEnumerator MissedNote()
    {
        missedNote = true;

        midiFilePlayer.MPTK_Pause();
        beatScroller.tempo = 0;
        
        missedHits++;
        SpawnMonster(missedHits);
        yield return new WaitForSeconds(3f);
        
        missedNote = false;
        midiFilePlayer.MPTK_UnPause();

        //Moving the beat controller upwards to give player time to recover and giving them temporary slow speed (for some reason only works as intended with hard-coded values..)
        StartCoroutine(TemporarySlowSpeed());

        beatScroller.tempo = 4.18f;
        midiFilePlayer.MPTK_Speed = 1f;

        currentMultiplier = 1;
        multiplierTracker = 0;
    }

    //Methods to define different types of note "hits" by the player
    public void NormalHit()
    {
        currentScore += scorePerHit * currentMultiplier;
        normalHits++;
        
        HitNote();
    }

    public void GoodHit()
    {
        currentScore += scorePerGoodHit * currentMultiplier;
        goodHits++;
        
        HitNote();
    }

    public void PerfectHit()
    {
        currentScore += scorePerPerfectHit * currentMultiplier;
        perfectHits++;
        
        HitNote();
    }
    

    private void SpawnMonster(int misses)
    {

        if(misses <= missesLimit)
        {
            var monsterValue = monsters[misses-1];

            Instantiate(monsterValue, monsterValue.transform.position, Quaternion.identity);
        }
    }

    
    private IEnumerator TemporarySlowSpeed()
    {
        beatScroller.tempo = 4f;
        midiFilePlayer.MPTK_Speed = .9f;

        yield return new WaitForSeconds(3f);
    }
    
    private IEnumerator PlayDeathAnim()
    {
        charAnimator.SetBool("isDead", true);

        yield return new WaitForSeconds(2f);

        ////Results screen pop-up
        resultsUI.SetActive(true);
    }
}
