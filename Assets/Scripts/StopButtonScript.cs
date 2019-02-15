using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TouchScript.Gestures;
using TouchScript.Hit;
using UnityEngine.UI;

public class StopButtonScript : MonoBehaviour {

    // TapGesture of this object.
    private TapGesture tg;

    // Default color of this button.
    private Color standard;

    // Current color of this button.
    public string currColor;

    // Use this for initialization
    void Start()
    {

        tg = this.GetComponent<TapGesture>();
        standard = this.GetComponent<Renderer>().material.color;
        currColor = "standard";
    }

    // Update is called once per frame
    void Update()
    {

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
     * In the meantime, it only changes color.
     * 
     * ADD TO FUNCTIONALITY IN THE FUTURE.
     * 
     **/
    private void tappedHandler(object sender, EventArgs e)
    {
        Debug.Log("Flight time is:" + WorldScript.runTime);
        // Do what needs to be done.
        GameObject.Find("Master").GetComponent<ROSDroneConnection>().SendServiceCall("land", "");

        if (currColor == "standard")
        {
            SetColorToDarkRed();
        }
        else
        {
            SetColorToStandard();
        }

    }

    /**
     * 
     * Makes this button dark red.
     * 
     **/
    public void SetColorToDarkRed()
    {

        Color red = new Color(0.5F, 0F, 0F, 1F);

        this.GetComponent<Renderer>().material.color = red;

        currColor = "darkred";

    }

    /**
     * 
     * Makes this button default color.
     * 
     **/
    public void SetColorToStandard()
    {

        this.GetComponent<Renderer>().material.color = standard;
        currColor = "standard";
    }

    /**
     * 
     * Disables the TapGesture in the GameObject it's attached to.
     * 
     * USE THIS FUNCTION IF YOU WANT TO TEMPORARILY DISABLE THIS OBJECT
     * FROM DETECTING TAPS.
     * 
     **/
    public void DisableTapGesture()
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
    public void EnableTapGesture()
    {

        tg.enabled = true;

    }

}
