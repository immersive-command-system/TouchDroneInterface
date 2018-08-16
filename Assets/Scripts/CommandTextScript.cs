using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandTextScript : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetTextToSelectedWaypoint(string s)
    {

        Text t = this.GetComponent<Text>();

        t.text = "Waypoint: " + s;

    }

    public void SetTextToHeightCommand()
    {

        Text t = this.GetComponent<Text>();

        t.text = "Please Select the Height.";

    }

    public void SetTextToWaypointDeleted()
    {

        Text t = this.GetComponent<Text>();

        InfoText it = GameObject.Find("InfoText").GetComponent<InfoText>();

        if (it.currWaypoint == null)
        {
            t.text = "No Point Selected.";
        }

    }

}
