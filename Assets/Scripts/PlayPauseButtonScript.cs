using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TouchScript.Gestures;
using TouchScript.Hit;
using UnityEngine.UI;

public class PlayPauseButtonScript : MonoBehaviour {

    // Image of the play icon.
    private GameObject PlayImage;

    // Image of the pause icon.
    private GameObject PauseImage;

    // List of all the waypoints. Taken from WorldScript.
    private GameObject[] waypoints;

    // Default color of button.
    private Color standard;

    // Contains information a list of waypoints.
    private WorldScript ws;

    // TapGesture component.
    private TapGesture tg;

    // All the possible button states.
    public enum PlayState {Play, Pause};

    // Are we actually in play?
    public bool isPlaying;

    // Which button state are we in?
    public PlayState ps;

    // Use this for initialization
    void Start () {

        PlayImage = GameObject.Find("PlayImage");
        PauseImage = GameObject.Find("PauseImage");

        // We can't be playing in the beginning.
        isPlaying = false;
        
        // Image we want to have appear is Play.
        ps = PlayState.Play;

        standard = this.GetComponent<Renderer>().material.color;
        tg = this.GetComponent<TapGesture>();

        GameObject world = GameObject.Find("World");
        ws = world.GetComponent<WorldScript>();

    }

    // Update is called once per frame
    void Update () {

        int NumPoints = ws.currPointIndex;

        if (NumPoints > 0)
        {

            // Users should only be able to press
            // the button if there is a waypoint placed.

            EnableTapGesture();


            if (ps == PlayState.Pause)
            {

                // Set color to green to indicate it's in Play.
                SetColorToGreen();

                // If in Play mode, we'll want Pause image to show
                // that tapping the button again will pause program.
                //DisableImage("play");
                //EnableImage("pause");

                // We're playing.
                isPlaying = true;
            }
            else
            {

                // Set color to default if not in play.
                SetColorToStandard();

                // If in Pause mode, we want Play image to show
                // that tapping button again will play program.
                DisableImage("pause");
                EnableImage("play");

                // We're not playing.
                isPlaying = false;
            }


            
            
        }
        else
        {

            // User cannot press the button if there are no waypoints.

            DisableImage("pause");
            EnableImage("play");
            DisableTapGesture();
            DisableImageTapping();
            SetColorToGray();
            isPlaying = false;
            ps = PlayState.Play;

        }


	}

    /**
     * 
     * Disables both the image and TapGesture of that image.
     * Result: Image will no longer show and can no longer detect touch.
     * 
     * String s is the iamge type you're trying to disable.
     * 
     **/
    private void DisableImage(string s)
    {
       
        if (s == "pause")
        {

            PauseImageScript pis = PauseImage.GetComponent<PauseImageScript>();

            // Disable the tap gesture to prevent double touches.
            pis.DisableTapGesture();

            // Disable image itself.
            PauseImage.GetComponent<Image>().enabled = false;

        }

        else if (s == "play")
        {

            PlayImageScript pis = PlayImage.GetComponent<PlayImageScript>();

            // Disable the tap gesture to prevent double touches.
            pis.DisableTapGesture();

            // Disable image itself.
            PlayImage.GetComponent<Image>().enabled = false;

        }
        else
        {


        }

    }

    /**
     * 
     * Enables both the image and TapGesture of that image.
     * Result: Image will show and will detect touch.
     * 
     * String s is the iamge type you're trying to enable.
     * 
     **/
    private void EnableImage(string s)
    {

        if (s == "pause")
        {

            PauseImageScript pis = PauseImage.GetComponent<PauseImageScript>();

            // Disable the tap gesture to prevent double touches.
            pis.EnableTapGesture();

            // Disable image itself.
            PauseImage.GetComponent<Image>().enabled = true;

        }

        else if (s == "play")
        {

            PlayImageScript pis = PlayImage.GetComponent<PlayImageScript>();

            // Disable the tap gesture to prevent double touches.
            pis.EnableTapGesture();

            // Disable image itself.
            PlayImage.GetComponent<Image>().enabled = true;

        }
        else
        {



        }
    }

    /**
     * 
     * Proprietary TouchScript code.
     * Please leave as is.
     * 
     **/
    private void OnEnable()
    {
        GetComponent<TapGesture>().Tapped += tappedHandler;
    }

    /**
     * 
     * Proprietary TouchScript code.
     * Please leave as is.
     * 
     **/
    private void OnDisable()
    {
        GetComponent<TapGesture>().Tapped -= tappedHandler;
    }

    /**
     * 
     * This function gets called whenever a touch is detected on
     * the object in which it was attached to.
     * 
     * If tapped, we'll want to switch the state of ps.
     * 
     **/
    public void tappedHandler(object sender, EventArgs e)
    {

        if (ps == PlayState.Play)
        {


            GameObject.Find("Master").GetComponent<ROSDroneConnection>().SendServiceCall("takeoff", "");
            ps = PlayState.Pause;


        }

        else if (ps == PlayState.Pause)
        {

            GameObject.Find("Master").GetComponent<ROSDroneConnection>().SendServiceCall("land", "");
            ps = PlayState.Play;

        }
        else
        {

        }

    }


    /* ======================================================================== HELPER FUNCTIONS DO NOT TOUCH ======================================================================== */

    /**
     * 
     * Diables TapGesture OF BOTH IMAGES.
     * 
     **/
    private void DisableImageTapping()
    {

        PlayImageScript play = PlayImage.GetComponent<PlayImageScript>();
        play.DisableTapGesture();

        PauseImageScript pause = PauseImage.GetComponent<PauseImageScript>();
        pause.DisableTapGesture();


    }

    /**
     * 
     * Enables TapGesture OF BOTH IMAGES.
     * 
     **/
    private void EnableImageTapping()
    {

        PlayImageScript play = PlayImage.GetComponent<PlayImageScript>();
        play.EnableTapGesture();

        PauseImageScript pause = PauseImage.GetComponent<PauseImageScript>();
        pause.EnableTapGesture();

    }

    /**
     * 
     * Makes this button gray.
     * 
     **/
    private void SetColorToGray()
    {

        Color silver = new Color(0.588F, 0.588F, 0.588F, 1F);

        this.GetComponent<Renderer>().material.color = silver;

    }

    /**
     * 
     * Makes this button green.
     * 
     **/
    private void SetColorToGreen()
    {

        Color green = new Color(0F, 1F, 0F, 1F);

        this.GetComponent<Renderer>().material.color = green;

    }

    /**
     * 
     * Makes this button default color.
     * 
     **/
    private void SetColorToStandard()
    {

        this.GetComponent<Renderer>().material.color = standard;

    }

    /**
     * 
     * Disables the TapGesture in the GameObject it's attached to.
     * 
     * USE THIS FUNCTION IF YOU WANT TO TEMPORARILY DISABLE THIS OBJECT
     * FROM DETECTING TAPS.
     * 
     **/
    private void DisableTapGesture()
    {

        tg.enabled = false;

    }

    /**
     * 
     * Enables the TapGesture in the GameObject it's attached to.
     * 
     * USE THIS FUNCTION TO RE-ENABLE TAP FUNCTIONALITY.
     * 
     **/
    private void EnableTapGesture()
    {

        tg.enabled = true;

    }

}
