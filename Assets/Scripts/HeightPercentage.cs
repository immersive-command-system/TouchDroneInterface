using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeightPercentage : MonoBehaviour {

    public Text percentageText;

	// Use this for initialization
	void Start () {
        percentageText = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
        GameObject s = GameObject.Find("HeightSlider");
        textUpdate(s.GetComponent<Slider>().value);
	}

    public void textUpdate(float value)
    {
        percentageText.text = Mathf.RoundToInt(value * 100) + "%";
    }
}
