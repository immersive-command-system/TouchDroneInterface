using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragHandler : EventTrigger
{

    private bool isDragging;

    // Use this for initialization
    void Start () {

        isDragging = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void OnEndDrag(PointerEventData data)
    {
        if (isDragging)
        {
            Debug.Log("hello");
            SliderHelper sh = this.GetComponent<SliderHelper>();
            sh.GetHeight();
        }
        isDragging = false;
    }


    public override void OnBeginDrag(PointerEventData data)
    {
        isDragging = true;
    }

}
