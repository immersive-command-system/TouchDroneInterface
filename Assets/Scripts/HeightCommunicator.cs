using System;
using TouchScript.Gestures;
using TouchScript.Hit;
using UnityEngine;
using UnityEngine.UI;

public class HeightCommunicator : MonoBehaviour {

	public String action;
	public String caller;

	void Start()
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

		var gesture = sender as TapGesture;
		HitData hit = gesture.GetScreenPositionHitData ();

		if (action == "Add") {
			float height = CalcHeight (hit);
			GameObject world = GameObject.Find ("World");
			world.SendMessage ("SetHeight", height);
		}
		if (action == "Move") {
			GameObject waypoint = GameObject.Find (caller);
			float height = CalcHeight (hit);
			waypoint.SendMessage ("SetHeight", height);
		}
	
	}

	private float CalcHeight(HitData hit)
	{

		return hit.Point.z/7;
	}

    public void setAction(String s)
    {
        action = s;
    }

    public void setCaller(String s)
    {
        caller = s;
    }
}
