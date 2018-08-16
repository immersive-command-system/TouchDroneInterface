using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonColorHandler : MonoBehaviour {

    // Has information whether or not program is in
    // delete or add state.
    private StateHandler s;

    // Default color of the button.
    private Color standard;

    // Type of button this specific ButtonColorHandler
    // is attached to.
    private string buttonType;

    // For add state. After initial add of a waypoint,
    // a height must be selected. This boolean tells us
    // whether or not we're still waiting for the height.
    private bool isWaiting;

    void Start()
    {

        // Find StateHandler
        s = GameObject.Find("Master").GetComponent<StateHandler>();

        // Set standard to the default color
        standard = this.GetComponent<Renderer>().material.color;

        // We're not waiting.
        isWaiting = false;

        // Determine what button type this script is attached to.
        if (this.name == "AddWayPointButton")
        {
            buttonType = "add";
        }
        else
        {
            buttonType = "delete";
        }

    }

    // Update is called once per frame
    void Update()
    {

        if (s.add_state && (buttonType == "add"))
        {

            // We're in add state. Change the color of AddWaypointButton.
            Color c = new Color(0.466F, 0.682F, 0.858F, 0.627F);
            this.GetComponent<Renderer>().material.color = c;

        }
        else if (s.delete_state && buttonType == "delete")
        {
            
            // We're in delete state. Change the color of AddWaypointButton.
            Color c = new Color(0.466F, 0.682F, 0.858F, 0.627F);
            this.GetComponent<Renderer>().material.color = c;

        }
        else if (isWaiting && ( (buttonType == "add") || (buttonType == "delete") ))
        {

            // If we're waiting for height, gray out both buttons to signal to users
            // that a tap isn't possible.

            // Silver
            Color silver = new Color(0.588F, 0.588F, 0.588F, 1F);
            this.GetComponent<Renderer>().material.color = silver;

        }
        else
        {

            // For all other cases, set color to default.
            this.GetComponent<Renderer>().material.color = standard;

        }

    }

    /**
     * 
     * Change isWaiting to true.
     * In our Update function, it'll change color of our button to Gray.
     * 
     **/
    public void UpdateColorToGray()
    {

        isWaiting = true;

    }

    /**
     * 
     * Change isWaiting to false.
     * In our Update function, it'll change color of our button to default.
     * 
     **/
    public void UpdateColorToDefault()
    {

        isWaiting = false;

    }



    /** =========================================================================================== MAY BE USEFUL =========================================================================================

    /**
        * 
        * ADDITIONAL COLOR OPTIONS IF DESIRED FOR.
        * 
        * Dark Gray to blue.
        * Color DarkGray = new Color(0.439F, 0.502F, 0.565F, 1F);
        *
        * Gray
        * Color Gray = new Color(0.501F, 0.501F, 0.501F, 1F);
        * 
        **/

    //  Color gray = new Color(0.862745098F, 0.862745098F, 0.862745098F, 0.8F);
    //  this.GetComponent<Renderer>().material.color = gray;

    /** =================================================================================================================================================================== **/
}
