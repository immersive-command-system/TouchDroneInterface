using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TouchScript.Gestures;
using TouchScript.Hit;
using UnityEngine.UI;

public class StopTextScript : MonoBehaviour
{

    private TapGesture tg;
    private StopButtonScript sbs;

    // Use this for initialization
    void Start()
    {

        tg = this.GetComponent<TapGesture>();
        sbs = GameObject.Find("StopButton").GetComponent<StopButtonScript>();

    }

    // Update is called once per frame
    void Update()
    {

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

        // Do what needs to be done.

        print("To be implemented.");

        if (sbs.currColor == "standard")
        {
            sbs.SetColorToDarkRed();
        }
        else
        {
            sbs.SetColorToStandard();
        }

    }


    public void DisableTapGesture()
    {

        tg.enabled = false;

    }

    public void EnableTapGesture()
    {

        tg.enabled = true;

    }

}
