using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TouchScript.Gestures;
using TouchScript.Hit;

public class DeleteWaypointButtonScript : MonoBehaviour {

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
     * Since this script is attached to DeleteWaypointButton,
     * we'll want to send a message to Master to let it know
     * that DeleteWaypointButton was touched.
     * 
     **/
    private void tappedHandler(object sender, EventArgs e)
	{

		var gesture = sender as TapGesture;
		HitData hit = gesture.GetScreenPositionHitData ();


		GameObject state = GameObject.Find ("Master");
		state.SendMessage ("ChangeDeleteState", hit);

	}

    /**
     * 
     * Makes this button red.
     * 
     **/
    public void MakeButtonRed()
    {

        Color filled = new Color(1F, 0F, 0F, 1F);
        this.GetComponent<Renderer>().material.color = filled;

    }

    /**
     * 
     * Disables the TapGesture in the GameObject it's attached to.
     * 
     * USE THIS FUNCTION IF YOU WANT TO TEMPORARILY DISABLE THIS OBJECT
     * FROM DETECTING TAPS.
     * 
     **/
    public void DisableTap()
    {
        this.GetComponent<TapGesture>().enabled = false;

    }

    /**
     * 
     * Enables the TapGesture in the GameObject it's attached to.
     * 
     * USE THIS FUNCTION TO RE-ENABLE TAP FUNCTIONALITY.
     * 
     **/
    public void EnableTap()
    {
        this.GetComponent<TapGesture>().enabled = true;
    }


}
