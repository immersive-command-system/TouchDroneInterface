using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ButtonHelper : MonoBehaviour
{
    public static string Units;
    public GameObject buttonObject;
    private Button actualButton;
    private Text buttonText;
    public GameObject YInfo;
    private Text YText;
	// Use this for initialization
	void Start () {
        Debug.Log("Start of ButtonHelper");
        actualButton = buttonObject.GetComponent<Button>();
        actualButton.onClick.AddListener(TaskOnClick);

        YText = YInfo.GetComponentInChildren<Text>();

        Units = "SI";
        

        buttonText = buttonObject.GetComponentInChildren<Text>();
        buttonText.text = "Switch to Metric";
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void TaskOnClick()
    {
        Debug.Log("Paxtan: Button Pressed");
        if (string.Equals(Units, "SI")){
            Units = "Metric";
            buttonText.text = "Switch to SI";
            YText.text = "Y: (m)";            
        } else
        {
            Units = "SI";
            buttonText.text = "Switch to Metric";
            YText.text = "Y: (ft)";
        }
        GameObject world = GameObject.FindGameObjectWithTag("World");
        world.GetComponent<WorldScript>().switchHovertextUnit(Units);
        Debug.Log("Current Units are" + Units);
    }
}
