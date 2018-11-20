using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SliderHelper : MonoBehaviour
{

    public Slider slider;

    public String action;
    public String caller;

    public void GetHeight()
    {

        if (action == "Add")
        {
            Debug.Log("Jang");
            float height = CalcHeight(slider.value);
            GameObject world = GameObject.Find("World");
            world.SendMessage("SetHeight", height);
        }
        if (action == "Move")
        {
            Debug.Log("mo");
            GameObject waypoint = GameObject.Find(caller);
            float height = CalcHeight(slider.value);
            waypoint.SendMessage("SetHeight", height);
        }
    }

    public void setAction(String s)
    {
        action = s;
    }

    public void setCaller(String s)
    {
        caller = s;
    }

    // Insert code to convert world coordinates to Unity coordinates.
    private float CalcHeight(float value)
    {
//        print(value);
        return value;
    }

}
