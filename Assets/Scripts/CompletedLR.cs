using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompletedLR : MonoBehaviour {

    private LineRenderer lr;

    // Use this for initialization
    void Start () {
        lr = GetComponent<LineRenderer>();
        lr.material = new Material(Shader.Find("Sprites/Default"));

        float alpha = 1.0f;
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(Color.green, 0.0f), new GradientColorKey(Color.green, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) }
        );

        lr.colorGradient = gradient;
        lr.positionCount = 0;
    }
	
	// Update is called once per frame
	void Update () {

        // Grab points.
        int NumWaypoints = GameObject.Find("World").GetComponent<WorldScript>().currPointIndex;

        if (NumWaypoints == 0) {
            lr = GetComponent<LineRenderer>();
            lr.positionCount = 0;
        }
        else {
            DrawLine();
        }
    }

    void DrawLine()
    {

        // List split.

        GameObject[] wayPoints = GameObject.Find("World").GetComponent<WorldScript>().wayPoints;
        int currPointIndex = GameObject.Find("World").GetComponent<WorldScript>().currPointIndex;

        int currCompleted = 0;

        for (int i = 0; i < currPointIndex; i++)
        {
            if (wayPoints[i].GetComponent<Waypoint>().WaypointComplete)
            {
                currCompleted += 1;
            }
        }

        Vector3[] Completed = new Vector3[currCompleted + 1];

        int c = 0;

        for (int i = 0; i < currPointIndex; i++)
        {
            if (wayPoints[i].GetComponent<Waypoint>().WaypointComplete)
            {
                Completed[c] = wayPoints[i].GetComponent<Waypoint>().flatPos;
                c += 1;
            }
        }

        // Draw Completed.
        if (Completed.Length <= 1)
        {
            lr = GetComponent<LineRenderer>();
            lr.positionCount = 0;
        }
        else
        {

            // Add Drone to completed.

            GameObject drone = GameObject.Find("Drone");
            Completed[Completed.Length - 1] = drone.transform.position;
            // Create a new line.

            lr = GetComponent<LineRenderer>();

            // Set vertex count.
            lr.positionCount = Completed.Length;

            // Set positions.
            lr.SetPositions(Completed);
        }


    }
}
