using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpcomingLR : MonoBehaviour {

    private LineRenderer lr;

    // Use this for initialization
    void Start()
    {
        lr = GetComponent<LineRenderer>();
        lr.material = new Material(Shader.Find("Sprites/Default"));

        float alpha = 1.0f;
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(Color.yellow, 0.0f), new GradientColorKey(Color.yellow, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) }
        );

        lr.colorGradient = gradient;
        lr.positionCount = 0;
    }

    // Update is called once per frame
    void Update()
    {

        // Grab points.
        int NumWaypoints = GameObject.Find("World").GetComponent<WorldScript>().currPointIndex;

        if (NumWaypoints == 0)
        {
            lr = GetComponent<LineRenderer>();
            lr.positionCount = 0;
        }
        else
        {
            DrawLine();
        }
    }

    void DrawLine()
    {

        // List split.

        GameObject[] wayPoints = GameObject.Find("World").GetComponent<WorldScript>().wayPoints;
        int currPointIndex = GameObject.Find("World").GetComponent<WorldScript>().currPointIndex;

        int currUpcoming = 0;

        for (int i = 0; i < currPointIndex; i++)
        {
            if (!wayPoints[i].GetComponent<Waypoint>().WaypointComplete)
            {
                currUpcoming += 1;
            }
        }

        Vector3[] Upcoming = new Vector3[currUpcoming + 1];

        int u = 1;

        for (int i = 0; i < currPointIndex; i++)
        {
            if (!wayPoints[i].GetComponent<Waypoint>().WaypointComplete)
            {
                Upcoming[u] = wayPoints[i].GetComponent<Waypoint>().flatPos;
                u += 1;
            }
        }

        // Draw Upcoming.
        if (Upcoming.Length <= 1)
        {
            lr = GetComponent<LineRenderer>();
            lr.positionCount = 0;
        }
        else if (Upcoming.Length > 1)
        {

            // Add Drone Start Location to Upcoming IN THE BEGINNING.
            GameObject world = GameObject.Find("World");
            Upcoming[0] = world.transform.position;

            // Create a new line.
            lr = GetComponent<LineRenderer>();

            // Set vertex count.
            lr.positionCount = Upcoming.Length;

            // Set positions.=
            lr.SetPositions(Upcoming);
        }


    }
}
