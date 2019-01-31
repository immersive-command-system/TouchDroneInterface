using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DetailBoxDragHelper : EventTrigger
{

    private GameObject InfoText;
    public bool isDragging;
    private Slider slider;
    public bool adding;

    private GameObject world;

	// Use this for initialization
	void Start () {

        InfoText = GameObject.Find("InfoText");
        slider = GameObject.Find("ZSlider").GetComponent<Slider>();
        adding = false;
        world = GameObject.Find("World");
        slider.maxValue = 10f;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void OnEndDrag(PointerEventData data)
    {
        if (isDragging)
        {

            float value = slider.value;
            UpdateHeight(value);
    
        }

        isDragging = false;
    }


    public override void OnBeginDrag(PointerEventData data)
    {
        isDragging = true;
    }
    
    private void UpdateHeight(float value)
    {
        InfoText it = InfoText.GetComponent<InfoText>();

        string currWP = it.currWaypoint;

        if (currWP != null && adding)
        {
            world.SendMessage("SetHeight", value);
        }
        if (currWP != null)
        {

            GameObject currObject = GameObject.Find(currWP);

            Waypoint wp = currObject.GetComponent<Waypoint>();

            wp.UpdateWorldHeight(value);
        }

    }

    public void AddStateTrue()
    {

        adding = true;

    }

    public void AddStateFalse()
    {

        adding = false;

    }
}
