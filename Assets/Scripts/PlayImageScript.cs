using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TouchScript.Gestures;
using TouchScript.Hit;
using UnityEngine.UI;

public class PlayImageScript : MonoBehaviour {


    private TapGesture tg;
    private PlayPauseButtonScript ppbs;

	// Use this for initialization
	void Start () {

        tg = this.GetComponent<TapGesture>();
        ppbs = GameObject.Find("PlayPauseButton").GetComponent<PlayPauseButtonScript>();

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void DisableTapGesture()
    {

        tg.enabled = false;

    }

    public void EnableTapGesture()
    {

        tg.enabled = true;

    }

    private void OnEnable()
    {
        GetComponent<TapGesture>().Tapped += tappedHandler;
    }

    private void OnDisable()
    {
        GetComponent<TapGesture>().Tapped -= tappedHandler;
    }

    public void tappedHandler(object sender, EventArgs e)
    {

        ppbs.tappedHandler(sender, e);

    }

}
