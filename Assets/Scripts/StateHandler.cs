using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TouchScript.Gestures;
using TouchScript.Hit;
using TouchScript.Behaviors;
using TouchScript.Gestures.TransformGestures;

public class StateHandler : MonoBehaviour {

	public bool add_state;
	public bool delete_state;

    private bool transforms_deleted;
    private bool transforms_added;

	// Use this for initialization
	void Start () {
		add_state = false;
		delete_state = false;
        transforms_deleted = false;
        transforms_added = true;
	}

	// Update is called once per frame
	void Update () {
        if (delete_state)
        {
            if (!transforms_deleted)
            {
                DeleteTapGestures();
                transforms_deleted = true;
                transforms_added = false;
            }
        }
        else
        {
            if (!transforms_added)
            {
                AddTapGestures();
                transforms_added = true;
                transforms_deleted = false;
            }
        }
	}
		

	public void ChangeDeleteState(HitData hit) {

		// Add State should not be true right now.
		add_state = false;

		if (delete_state == false) {
			delete_state = true;
		} else if (delete_state == true) {
			delete_state = false;
		} else {
			delete_state = false;
		}
	}

	public void ChangeAddState(HitData hit) {
		
		// Delete State should not be true right now.
		delete_state = false;

		if (add_state == false) {
			add_state = true;
		} else if (add_state == true) {
			add_state = false;
		} else {
			add_state = false;
		}
	}

    private void DeleteTapGestures()
    {

        GameObject[] waypoints = GameObject.FindGameObjectsWithTag("Waypoint");

        if (waypoints.Length != 0)
        {

            for (int i = 0; i < waypoints.Length; i++)
            {
                GameObject wp = waypoints[i];
                Transformer t = wp.GetComponent<Transformer>();
                TransformGesture tg = wp.GetComponent<TransformGesture>();
                TapGesture tap = wp.GetComponent<TapGesture>();

                if (t != null && tg != null && tap != null)
                {

                    t.enabled = false;
                    tg.enabled = false;
                    tap.enabled = false;


                }
            }
        }
    }

    private void AddTapGestures()
    {
        GameObject[] waypoints = GameObject.FindGameObjectsWithTag("Waypoint");

        if (waypoints.Length != 0)
        {

            for (int i = 0; i < waypoints.Length; i++)
            {

                GameObject wp = waypoints[i];
                //                TransformGesture tg = wp.AddComponent<TransformGesture>();
                //                UnityEditorInternal.ComponentUtility.MoveComponentUp(tg);
                //                UnityEditorInternal.ComponentUtility.MoveComponentUp(tg);
                //                Transformer t = wp.AddComponent<Transformer>();
                //                UnityEditorInternal.ComponentUtility.MoveComponentUp(t);
                //                TapGesture tap = wp.AddComponent<TapGesture>();
                //                UnityEditorInternal.ComponentUtility.MoveComponentUp(tap);
                //                UnityEditorInternal.ComponentUtility.MoveComponentUp(tap);


                Transformer t = wp.GetComponent<Transformer>();
                TransformGesture tg = wp.GetComponent<TransformGesture>();
                TapGesture tap = wp.GetComponent<TapGesture>();

                t.enabled = true;
                tg.enabled = true;
                tap.enabled = true;
            }
        }
    }


}
