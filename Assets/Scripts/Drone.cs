using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TouchScript.Gestures;
using TouchScript.Hit;
using UnityEngine.UI;

public class Drone : MonoBehaviour {

    private TapGesture tg;
    private GameObject master;
    private GameObject world;

    // Add HELLA ROS code in this class.
    // Where is the ROS API?
	// Use this for initialization
	void Start () {


        tg = this.GetComponent<TapGesture>();
        master = GameObject.Find("Master");
        world = GameObject.Find("World");

	}
	
	// Update is called once per frame
	void Update () {


	}

    private void OnEnable()
    {
        GetComponent<TapGesture>().Tapped += tappedHandler;
    }

    private void OnDisable()
    {
        GetComponent<TapGesture>().Tapped -= tappedHandler;
    }

    private void tappedHandler(object sender, EventArgs e)
    {

        StateHandler sh = master.GetComponent<StateHandler>();
        if (sh.add_state)
        {

            world.GetComponent<WorldScript>().tappedHandler(sender, e);

        }

    }

    private void DisableTapGesture()
    {

        tg.enabled = false;

    }

    private void EnableTapGesture()
    {

        tg.enabled = true;

    }

}
