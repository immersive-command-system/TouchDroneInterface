using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoText : MonoBehaviour {

    public Text XInfo;
    public Text YInfo;
    public Text ZInfo;
    public Slider ZSlider;

    public string currWaypoint;
    private string prevWaypoint;
    private DetailBoxDragHelper dbdh;

    public bool moving;
    public bool waiting;
    public bool adding;
    private bool first_run;


	// Use this for initialization
	void Start () {

        currWaypoint = null;
        prevWaypoint = null;

        moving = false;
        waiting = false;
        adding = false;

        first_run = false;
        dbdh = GameObject.Find("ZSlider").GetComponent<DetailBoxDragHelper>();
    }

    // Update is called once per frame
    void Update () {

        UpdateText();

        if (dbdh.isDragging)
        {
            UpdateY();
        }
	}


    private void UpdateText()
    {
        if ((currWaypoint != null) && adding)
        {

            GameObject currPoint = GameObject.Find(currWaypoint);

            float x = currPoint.transform.position.x;
            float z = currPoint.transform.position.z;

            SetX(x);
            // SetY(Mathf.Infinity);
            SetZ(z);
            // SetHeight(Mathf.Infinity);

            first_run = true;


        }

        else if ((currWaypoint != null) && moving)
        {
            GameObject currPoint = GameObject.Find(currWaypoint);

            Vector3 currPos = currPoint.GetComponent<Waypoint>().currPos;
            Vector3 worldPos = currPoint.GetComponent<Waypoint>().worldPos;

            SetX(currPos.x);
            SetY(worldPos.y);
            SetZ(currPos.z);
            SetHeight(worldPos.y);
        }

        else if (currWaypoint != null && first_run)
        {
            GameObject currPoint = GameObject.Find(currWaypoint);

            Vector3 worldPos = currPoint.GetComponent<Waypoint>().worldPos;

            SetX(worldPos.x);
            SetY(worldPos.y);
            SetZ(worldPos.z);
            SetHeight(worldPos.y);
            first_run = false;

        }
        else
        {
            /*
            SetX(Mathf.Infinity);
            SetY(Mathf.Infinity);
            SetZ(Mathf.Infinity);
            SetHeight(0f);
            */
        }

    }
 
    /**
     * 
     * 
     * There are inherent tradeoffs. 
     * Lots more accidental scrolling opportunities.
     * Constant update/non-interactable slider 
     * ensures that the height doesn't get changed on accident.
     * 
     **/


    // Should have automatic updating now.
    // How to switch between using world/curr pos?
    // Above is resolved.
 
     private void UpdateTextOnce ()
     {

        if (currWaypoint != null)
        {
            GameObject currPoint = GameObject.Find(currWaypoint);

            //Vector3 currPos = currPoint.GetComponent<Waypoint>().currPos;
            Vector3 worldPos = currPoint.GetComponent<Waypoint>().worldPos;

            SetX(worldPos.x);
            SetY(worldPos.y);
            SetZ(worldPos.z);
            SetHeight(worldPos.y);

            //print(currPos.x);
        }
    }

    private void UpdateTextOnceAddVersion()
    {

        if (currWaypoint != null)
        {

            GameObject currPoint = GameObject.Find(currWaypoint);

            float x = currPoint.transform.position.x;
            float z = currPoint.transform.position.z;

            SetX(x);
            SetY(Mathf.Infinity);
            SetZ(z);
            SetHeight(Mathf.Infinity);


        }

    }
    
    public void AddWaypoint(string wp) {

        prevWaypoint = currWaypoint;
        currWaypoint = wp;

        UpdateTextOnceAddVersion();

        if (prevWaypoint != null)
        {
            WaypointColorUpdater(prevWaypoint, "prev");
        }

        WaypointColorUpdater(currWaypoint, "curr");

    }

    private void WaypointColorUpdater(string waypoint, string type)
    {

        GameObject w = GameObject.Find(waypoint);
        Waypoint wp = w.GetComponent<Waypoint>();

        if ((wp != null) && type == "adding")
        {

            wp.ColorUpdateAdd();

        }

        if ((wp != null) && type == "prev")
        {

            wp.ColorUpdatePrev();

        }

        if ((wp != null) && type == "curr")
        {

            wp.ColorUpdateCurrent();

        }

    }

    private void SetX(float value)
    {
        if (value != Mathf.Infinity)
        {
            XInfo.text = "X: " + value;
        } else
        {
            XInfo.text = "X: ";
        }
    }

    private void SetY(float value)
    {
        if (value != Mathf.Infinity)
        {
            //YInfo.text = "Y: " + value + " (ft)";
            if (ButtonHelper.Units == "SI")
            {
                YInfo.text = "Y: " + value + " (ft)";
            } else
            {
                YInfo.text = "Y: " + value + " (m)";
            }
        }
        else
        {
            YInfo.text = "Y: ";
        }
    }

    private void SetZ(float value)
    {
        if (value != Mathf.Infinity)
        {
            ZInfo.text = "Z: " + value;
        }
        else
        {
            ZInfo.text = "Z: ";
        }
    }

    // This will require future revisions to adopt the mapping
    // we develop going from world to unity space.
    void SetHeight(float value)
    {
        ZSlider.value = value;
    }

    public void SetCurrWaypoint(string w)
    {

        prevWaypoint = currWaypoint;
        currWaypoint = w;

        //       GameObject point = GameObject.Find(currWaypoint);
        //       Waypoint wp = point.GetComponent<Waypoint>();
        //       SetX(wp.worldPos.x);
        //       SetY(wp.worldPos.y);
        //       SetZ(wp.worldPos.z);
        //       SetHeight(wp.worldPos.y);

        //  UpdateText();

        // Color Updating

        // Update Prev

        UpdateTextOnce();

        if (prevWaypoint != null)
        {
            WaypointColorUpdater(prevWaypoint, "prev");
        }

        WaypointColorUpdater(currWaypoint, "curr");
    }

    public void WaypointDeleted(string s)
    {
       if (s == currWaypoint)
        {
            currWaypoint = null;
            prevWaypoint = null;
            SetX(Mathf.Infinity);
            SetY(Mathf.Infinity);
            SetZ(Mathf.Infinity);
            SetHeight(0f);

        }
    }

    private void UpdateY()
    {

        Slider s = dbdh.GetComponent<Slider>();
        SetY(s.value);

    }

    public void YesMoving()
    {
        moving = true;
    }

    public void NoMoving()
    {
        moving = false;
    }

    public void YesWaiting()
    {
        waiting = true;
    }

    public void NoWaiting()
    {
        waiting = false;
    }

    public void YesAdding()
    {

        adding = true;

        WaypointColorUpdater(currWaypoint, "adding");
        
    }

    public void NoAdding()
    {

        adding = false;

        WaypointColorUpdater(currWaypoint, "curr");

    }

    /**
     * 
     * Notes: HUGE disadvantage.
     * Since my program relies on the transform handler to tell it when it's moving,
     * as soon as it's not moving, we start using worldPos. Thereby, switching
     * from currPOS TO WORLDPOS, which isn't updated until height is selected.
     * This can be confusing for user.
     * 
     **/



    // Old Code from UpdateText.

    //else if ((currWaypoint != null) && waiting)
    //{
    //    GameObject currPoint = GameObject.Find(currWaypoint);

    //    Vector3 currPos = currPoint.GetComponent<Waypoint>().currPos;

    //     SetX(currPos.x);
    //     SetY(Mathf.Infinity);
    //      SetZ(currPos.z);
    //     SetHeight(0);
    //    first_run = true;
    // }

    // Only use if constant updating.
}
