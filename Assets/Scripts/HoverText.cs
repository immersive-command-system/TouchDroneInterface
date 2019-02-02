using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HoverText : MonoBehaviour {

    private TextMesh tm;
    public Waypoint AttachedWaypoint;
    private GameObject c;
	// Use this for initialization
	void Start () {

        tm = this.GetComponent<TextMesh>();

	}
	
	// Update is called once per frame
	void Update () {

        SetPosition();

        SetText();
        
	}

    public void SetText()
    {

        Vector3 worldPos = AttachedWaypoint.worldPos;
        float height = RoundToHundredth(worldPos.y);
        string unit = "ft";
        if (ButtonHelper.Units == "Metric")
        {
            unit = "m";
        }
        tm.text = "" + this.AttachedWaypoint.gameObject.name + " | " + height + " | " + unit;

    }

    public void switchUnit(string convertTo)
    {
        Debug.Log("Converting Units.");

        if (convertTo == "Metric")
        {
            AttachedWaypoint.worldPos.y = AttachedWaypoint.worldPos.y / 3.05f;
        } else
        {
            AttachedWaypoint.worldPos.y = AttachedWaypoint.worldPos.y * 3.05f;
        }

        SetText();
    
    }

    private void SetPosition()
    {

        Vector3 trans = AttachedWaypoint.transform.position;

        float x = 0.27f;
        float z = 0.32f;

        this.transform.position = new Vector3(trans.x - x, trans.y, trans.z + z);

    }

    public void SetAttachedWaypoint(string waypointID)
    {

        GameObject w = GameObject.Find(waypointID);
        Waypoint wp = w.GetComponent<Waypoint>();

        AttachedWaypoint = wp;

        SetHoverTextName(waypointID);

    }

    private void SetHoverTextName(string waypointID)
    {

        this.gameObject.name = "HoverText_" + waypointID;

    }

    private float RoundToHundredth(float f)
    {

        float tempF = f * 100;

        tempF = Mathf.Round(tempF);

        return tempF / 100;

    }
}
