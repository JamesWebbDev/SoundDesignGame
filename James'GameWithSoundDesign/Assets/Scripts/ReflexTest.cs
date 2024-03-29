﻿using UnityEngine;
using UnityEngine.UI;

public class ReflexTest : MonoBehaviour
{
    // Links Test to Buttons
    public ReflexButtons reflexButtons;
    public FPSMovement fpsMovement;

    public GameObject startButton;
    public GameObject startingPoint;
    private GameObject player;

    private float rayLength = 4f;

    public int score;

    // Establishes UI elements for result
    public Image reflexMask;
    public Text reflexText;
    private float resultTime = 2f;
    private bool resultShown = false;

    // Establishes UI elements for start
    public Image startMask;
    public Text startText;
    public Text startNumber;
    private float startTime = 4f;
    private bool startShown = false;

    // Start and finishing sound sources
    public AudioSource AudioS_testStart;
    public AudioSource AudioS_testDone;
    public AudioClip ui_test_start;
    public AudioClip ui_test_done;
    bool countdownActive = false;

    [HideInInspector]
    public bool playing;
    [HideInInspector] public bool RBActive = false;

    public bool reflexSent = false;

    // Start is called before the first frame update
    void Start()
    {
        playing = reflexButtons.GetComponent<ReflexButtons>().enabled;
        playing = false;

        reflexText.enabled = false;
        reflexMask.enabled = false;

        startMask.enabled = false;
        startText.enabled = false;
        startNumber.enabled = false;

        // Finds "Player" in Scene
        player = GameObject.FindWithTag("Player");

        
    }

    // Update is called once per frame
    void Update()
    {
        // Establishes a temporary Ray pointing out of camera
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Establishes that attached object can be hit by Raycast
        RaycastHit hit;

        // When buttons is pressed
        if (Physics.Raycast(ray, out hit, rayLength) && Input.GetMouseButtonDown(0) && hit.transform.tag == "StartButton" && playing == false)
        {
            // Sets player position to game
            player.transform.position = startingPoint.transform.position;
            player.transform.rotation = startingPoint.transform.rotation;
            // Turns off Player movement
            fpsMovement.enabled = false;

            RBActive = true;

            startShown = true;
            
        }

        StartingUI();

        // What happens when Test ends
        if (reflexButtons.gameTimer <= 0)
        {
            // Stop the game
            playing = false;

            AudioS_testDone.PlayOneShot(ui_test_done);

            reflexButtons.randomNumber = 0;

            reflexButtons.LitButtons();

            reflexSent = true;

            resultShown = true;

            RBActive = false;

            // Resets Timer
            reflexButtons.gameTimer = 10;

            reflexButtons.hasRun = false;

            fpsMovement.enabled = true;   
        }

        ResultUI();    
    }

    void ResultUI()
    {
        if (resultShown)
        {
            resultTime -= Time.deltaTime;
            reflexText.enabled = true;
            reflexMask.enabled = true;

            if (resultTime <= 0)
            {
                reflexText.enabled = false;
                reflexMask.enabled = false;
                resultTime = 2f;
                resultShown = false;
            }
        }
    }
    void StartingUI ()
    {
        if (startShown)
        {
            startTime -= Time.deltaTime;
            startMask.enabled = true;
            startText.enabled = true;
            startNumber.enabled = true;

            if (countdownActive == false)
            {
                AudioS_testStart.PlayOneShot(ui_test_start);
                countdownActive = true;
            }

            startNumber.text = "" + startTime;

            if (startTime <= 1)
            {
                startMask.enabled = false;
                startText.enabled = false;
                startNumber.enabled = false;
                startShown = false;
                countdownActive = false;

                playing = true;
                score = 0;
                startTime = 4f;
            }
        }
    }
}
