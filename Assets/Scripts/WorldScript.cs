using System;
using System.Collections;
using TouchScript.Gestures;
using TouchScript.Hit;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TouchScript.Behaviors;
using TouchScript.Behaviors.UI;
using TouchScript.Gestures.TransformGestures;
using ROSBridgeLib;
using ROSBridgeLib.interface_msgs;

public class WorldScript : MonoBehaviour
{
	public Transform Container;
	public Transform WaypointPrefab;
    public GameObject heightSliderPrefab;
    public GameObject Canvas;
    public GameObject sliderText;


    private GameObject CommandText;
    private GameObject AddWaypointButton;
    private GameObject AddText;
    private GameObject DeleteWaypointButton;
    private GameObject DeleteText;
    private GameObject ZSlider;
    private GameObject InfoText;

	public float Scale = .5f;

	private float TempHeight;

	public GameObject[] wayPoints;
	public int currPointIndex;
	private int numWaypoints;

	void Start()
	{

		// On Start, set overhead cam. Disable height cam.
		TempHeight = 0f;
		wayPoints = new GameObject[20];

		currPointIndex = 0;
		numWaypoints = 0;

        CommandText = GameObject.Find("CommandText");

        AddWaypointButton = GameObject.Find("AddWayPointButton");

        AddText = GameObject.Find("AddText");

        DeleteWaypointButton = GameObject.Find("DeleteWayPointButton");

        DeleteText = GameObject.Find("DeleteText");

        ZSlider = GameObject.Find("ZSlider");

        InfoText = GameObject.Find("InfoText");

    }

	private void OnEnable()
	{
		GetComponent<TapGesture>().Tapped += tappedHandler;

		GetComponent<LongPressGesture> ().LongPressed += longPressHandler;
	}

	private void OnDisable()
	{
		GetComponent<TapGesture>().Tapped -= tappedHandler;

		GetComponent<LongPressGesture> ().LongPressed -= longPressHandler;
	}

	public void tappedHandler(object sender, EventArgs e)
	{

		// Can only add if in Add_State.
		if (GameObject.Find ("Master").GetComponent<StateHandler> ().add_state) {
			
			var gesture = sender as TapGesture;
			HitData hit = gesture.GetScreenPositionHitData ();
//			print ("Tapped Position: " + hit.Point);

			// Switch Add_State off.
			GameObject master = GameObject.Find("Master");
			master.GetComponent<StateHandler> ().add_state = false;

			StartCoroutine(WaitHeight(hit));


		}

		// Delete Functionality.
		if (GameObject.Find ("Master").GetComponent<StateHandler> ().delete_state) {

			var gesture = sender as TapGesture;
			HitData hit = gesture.GetScreenPositionHitData ();

			Vector3 worldHitPoint = new Vector3 (hit.Point.x, 2f, hit.Point.z);

			Vector3 origin = new Vector3 (worldHitPoint.x, 3f, worldHitPoint.z);
	
			RaycastHit h = new RaycastHit ();
			Ray r = new Ray (origin, Camera.main.transform.forward);
			//		Debug.DrawRay (origin, Camera.main.transform.forward,Color.red, 100, true);
			if (Physics.Raycast (origin, Camera.main.transform.forward)) {
				if (Physics.Raycast (r, out h)) {
					if (h.collider.gameObject.CompareTag("Waypoint")) {
						
						GameObject waypoint = h.collider.gameObject;

                        if (!waypoint.GetComponent<Waypoint>().WaypointComplete)
                        {

                            // ROS COMMUNICATION
                            string prevID = GetPrevID(waypoint.name, "delete");
                            float worldX = waypoint.transform.position.x;
                            float worldZ = waypoint.transform.position.z;
                            float height = waypoint.GetComponent<Waypoint>().worldPos.y;
                            UserpointInstruction msg = new UserpointInstruction(waypoint.name, prevID, worldX, height, worldZ, "DELETE");
                            GameObject.Find("Master").GetComponent<ROSDroneConnection>().PublishWaypointUpdateMessage(msg);


                            DeleteHouseKeeping(waypoint);

                            // Switch delete_state off.
                            GameObject master = GameObject.Find("Master");
                            master.GetComponent<StateHandler>().delete_state = false;

                        }

					}
				}
			}
		}
	}

	private void longPressHandler(object sender, EventArgs e)
	{

		// Delete Functionality.
		var gesture = sender as LongPressGesture;
		HitData hit = gesture.GetScreenPositionHitData ();
//		Debug.Log (hit);
		Vector3 worldHitPoint = new Vector3 (hit.Point.x, 2f, hit.Point.z);
//		Debug.Log (worldHitPoint); 
		// Raycast Approach
		Vector3 origin = new Vector3 (worldHitPoint.x, 3f, worldHitPoint.z);

		RaycastHit h = new RaycastHit();
		Ray r = new Ray(origin, Camera.main.transform.forward);
//		Debug.DrawRay (origin, Camera.main.transform.forward,Color.red, 100, true);
		if (Physics.Raycast(origin, Camera.main.transform.forward)) {
			if (Physics.Raycast(r, out h)) {
				if (h.collider.gameObject.CompareTag("Waypoint")) {
					GameObject waypoint = h.collider.gameObject;

                    if (!waypoint.GetComponent<Waypoint>().WaypointComplete)
                    {

                        // ROS COMMUNICATION
                        string prevID = GetPrevID(waypoint.name, "delete");
                        float worldX = waypoint.transform.position.x;
                        float worldZ = waypoint.transform.position.z;
                        float height = waypoint.GetComponent<Waypoint>().worldPos.y;
                        UserpointInstruction msg = new UserpointInstruction(waypoint.name, prevID, worldX, height, worldZ, "DELETE");
                        GameObject.Find("Master").GetComponent<ROSDroneConnection>().PublishWaypointUpdateMessage(msg);

                        DeleteHouseKeeping(waypoint);

                       
                    }
				}
			}
		}

	}

    private void DeleteHouseKeeping(GameObject waypoint)
    {

        // List Reordering Housekeeping
        DeleteWaypoint(waypoint);
        DeleteWaypointReordering();

        InfoText it = InfoText.GetComponent<InfoText>();
        it.WaypointDeleted(waypoint.gameObject.name);

        // Command Text should not display any point information.
        CommandTextScript cts = CommandText.GetComponent<CommandTextScript>();
        cts.SetTextToWaypointDeleted();

        currPointIndex -= 1;
        Destroy(waypoint);

    }

	IEnumerator WaitHeight(HitData hit)
	{

		// Instantiating waypoint in right place. 
		var waypoint = Instantiate(WaypointPrefab) as Transform;
		waypoint.parent = Container;

		String id = CreateWaypointID ("A");
		waypoint.name = id;

        // Add more components to waypoint.
        waypoint.gameObject.AddComponent<TapGesture>();
        waypoint.gameObject.AddComponent<TransformGesture>();
		waypoint.gameObject.AddComponent<Waypoint> ();
		waypoint.gameObject.AddComponent<Transformer> ();

		waypoint.localScale = Vector3.one * Scale * waypoint.localScale.x;
		waypoint.position = new Vector3 (hit.Point.x, 2f, hit.Point.z);

		waypoint.gameObject.GetComponent<Waypoint>().setFlatPos(waypoint.position);
		waypoint.gameObject.GetComponent<Waypoint>().setLastPos(waypoint.position);
		waypoint.gameObject.GetComponent<Waypoint>().setCurrPos(waypoint.position);

        AddWaypointHouseKeeping(waypoint.name);

        // Waiting player for height.
        //		print ("Waiting for a height.");
        yield return new WaitUntil (() => TempHeight != 0);


        AddWaypointUndoHouseKeeping(waypoint.name);

        //		print ("Height received.");
        // Hit coordinates are unreliable...

        float worldX = waypoint.transform.localPosition.x;

        float worldZ = waypoint.transform.localPosition.z;

        waypoint.gameObject.GetComponent<Waypoint> ().setWorldPos(new Vector3 (worldX, TempHeight, worldZ));
		waypoint.gameObject.GetComponent<Waypoint>().SetID (id);
        waypoint.GetComponent<Waypoint>().CreateHoverText();


        string prevID = GetPrevID(waypoint.name, "add");

        // ROS COMMUNICATION
        UserpointInstruction msg = new UserpointInstruction(waypoint.name, prevID, worldX, TempHeight, worldZ, "ADD");
        GameObject.Find("Master").GetComponent<ROSDroneConnection>().PublishWaypointUpdateMessage(msg);

        // Could be buggy to only add the GameObject.
        wayPoints[currPointIndex] = waypoint.gameObject;
		currPointIndex += 1;
		numWaypoints += 1;
//		print ("Stored point: " + store);

		// Get waypoint.
		waypoint.gameObject.AddComponent<SphereCollider> ();

        // Color filled = new Color(0.278F, 0.874F, 1F, 0.9F);
        // waypoint.GetComponent<Renderer>().material.color = filled;

        // Housekeeping
        TempHeight = 0f;

	}

    public string GetPrevID(string s, string action)
    {

        if (action == "add")
        {

            if (currPointIndex == 0)
            {
                return "DRONE";
            }
            else
            {
                return wayPoints[currPointIndex - 1].name;
            }

        }

        else
        {
            for (int i = 0; i < currPointIndex; i++)
            {

                if (wayPoints[i].name == s)
                {

                    if (i == 0)
                    {
                        return "DRONE";
                    }
                    else
                    {
                        return wayPoints[i - 1].name;
                    }
                }
            }
        }

        return "DRONE";

    }

    private void AddWaypointUndoHouseKeeping(string waypoint)
    {
        // Enable Touches on AddWaypointButton. 
        // Enable Touches on DeleteWaypointButton.
        // Switch CommandText.

        AddWaypointButtonScript abs = AddWaypointButton.GetComponent<AddWaypointButtonScript>();
        abs.EnableTap();
        AddWaypointButtonScript absText = AddText.GetComponent<AddWaypointButtonScript>();
        absText.EnableTap();

        DeleteWaypointButtonScript dwbs = DeleteWaypointButton.GetComponent<DeleteWaypointButtonScript>();
        dwbs.EnableTap();
        DeleteWaypointButtonScript dwbsText = DeleteText.GetComponent<DeleteWaypointButtonScript>();
        dwbsText.EnableTap();

        CommandTextScript cts = CommandText.GetComponent<CommandTextScript>();
        cts.SetTextToSelectedWaypoint(waypoint);

        // Default color for AddWaypointButton.

        ButtonColorHandler bch = AddWaypointButton.GetComponent<ButtonColorHandler>();
        bch.UpdateColorToDefault();

        // Default color for DeleteWaypointButton.
        ButtonColorHandler bchd = DeleteWaypointButton.GetComponent<ButtonColorHandler>();
        bchd.UpdateColorToDefault();

        // Switching ZSlider's adding boolean to true.
        DetailBoxDragHelper dbdh = ZSlider.GetComponent<DetailBoxDragHelper>();
        dbdh.AddStateFalse();

        InfoText it = InfoText.GetComponent<InfoText>();
        it.NoAdding();

        // Re-enable tap function for all waypoints.
        EnableAllWaypointTapFunction();

    }

    private void AddWaypointHouseKeeping(string waypoint)
    {

        // Disable Touches on AddWaypointButton. 
        // Disable Touches on DeleteWaypointButton.
        // Switch CommandText.

        AddWaypointButtonScript abs = AddWaypointButton.GetComponent<AddWaypointButtonScript>();
        abs.DisableTap();
        AddWaypointButtonScript absText = AddText.GetComponent<AddWaypointButtonScript>();
        absText.DisableTap();

        DeleteWaypointButtonScript dwbs = DeleteWaypointButton.GetComponent<DeleteWaypointButtonScript>();
        dwbs.DisableTap();
        DeleteWaypointButtonScript dwbsText = DeleteText.GetComponent<DeleteWaypointButtonScript>();
        dwbsText.DisableTap();

        CommandTextScript cts = CommandText.GetComponent<CommandTextScript>();
        cts.SetTextToHeightCommand();

        // Graying out the AddwaypointButton b/c we're waiting for the height.

        ButtonColorHandler bch = AddWaypointButton.GetComponent<ButtonColorHandler>();
        bch.UpdateColorToGray();

        // Graying out the DeleteWaypointButton b/c we're waiting for the height.

        ButtonColorHandler bchd = DeleteWaypointButton.GetComponent<ButtonColorHandler>();
        bchd.UpdateColorToGray();

        // Switch ZSliders adding boolean to true.
        DetailBoxDragHelper dbdh = ZSlider.GetComponent<DetailBoxDragHelper>();
        dbdh.AddStateTrue();

        // Set CurrWaypoint of InfoText to the current waypoint because
        // an add action is considered a selection.

        InfoText it = InfoText.GetComponent<InfoText>();
        it.SetCurrWaypoint(waypoint);
        it.YesAdding();

        // Disable all waypoint tap function to prevent moves when waiting for height.
        DisableAllWaypointTapFunction();

    }

	/** Max Height of plane is 7.
	 *  Min Height of plane is 2.
	 *  Length of plane is 5.
	 *  First, let's store the ratio (Selected height/Max Height).
	 **/
	private float SetHeight(float height) 
	{

		// This is the ratio.
		TempHeight = height;
		return TempHeight;
	}

	void DeleteWaypointReordering() {
		GameObject[] newWayPoints = new GameObject[20];

		int j = 0;
		for (int i = 0; i < currPointIndex; i++) {

			if (wayPoints[i] == null) {

			} else {
				newWayPoints [j] = wayPoints [i];
				j = j + 1;
			}
		}

		wayPoints = newWayPoints;
	}

	void DeleteWaypoint(GameObject d) {

		// Figure out index of the position by comparing with 3D coordinates.

		for (int i = 0; i < wayPoints.Length; i++) {

			if (d.Equals(wayPoints[i])) {

				wayPoints[i] = null;

			}

		}

	}

	// Most likely not necessary anymore.
	// Previous: needed new waypoint position to update the Vector3 within waypoints[] of this script.
	// New: Don't need it because we have the waypoint itself keep track of its position. 
	//      The information is already in the waypoint.
//	public void MovedWaypoint(Vector3 old_pos, Vector3 new_pos) {
//
//		for (int i = 0; i < wayPoints.Length; i++) {
//			if (old_pos.Equals (wayPoints[i].transform.position)) {
//
//				// Current moving only supports x, z transforms. NO HEIGHT.
//				// There may exist weird bugs involving pos, currPos, and lastPos.
//				wayPoints[i].transform.position = new_pos;
//			}
//		}
//	}

	private String CreateWaypointID(String droneID) {
		String id = droneID + numWaypoints;
		return id;
	}


    /* *
     * 
     * If desired, we can disable all waypoint interactions.
     * User will not be able to check new heights until
     * height is selected.
     * 
     * */


    private void DisableAllWaypointTapFunction()
    {

        for (int i = 0; i < currPointIndex; i++)
        {

            // Grabs the waypoint script.
            GameObject w = wayPoints[i];
            Waypoint wp = w.GetComponent<Waypoint>();

            // Disable all tap components.
            // - Transformer
            // - Transform Gesture
            // - Tap Gesture

            wp.DisableTransformer();
            wp.DisableTransformGesture();
            wp.DisableTapGesture();

        }

    }

    private void EnableAllWaypointTapFunction()
    {

        for (int i = 0; i < currPointIndex; i++)
        {

            // Grabs the waypoint script.
            GameObject w = wayPoints[i];
            Waypoint wp = w.GetComponent<Waypoint>();

            // Enable all tap components.
            // - Transformer
            // - Transform Gesture
            // - Tap Gesture

            wp.EnableTransformer();
            wp.EnabledTransformGesture();
            wp.EnableTapGesture();

        }

    }



    /** =========================================================================================== NEGLECT =========================================================================================

    //		 Summon the plane.
    // var heightPlane = GameObject.CreatePrimitive(PrimitiveType.Plane);

    // Resizing, rotation, and position of plane.

    // Pick position.

    //		heightPlane.transform.localPosition = new Vector3 (7.78f, 0f, 4.56f);

    // or


    // HeightPlane implementation
    //		heightPlane.transform.localPosition = new Vector3 (9.5f, 0f, 4.56f);


    //		heightPlane.transform.localScale = new Vector3(0.5f, 1f, 0.038f);
    //		heightPlane.transform.localRotation = Quaternion.Euler(0, 90, 0);
    //		heightPlane.AddComponent<TapGesture> ();
    //		heightPlane.AddComponent<HeightCommunicator> ();

    //		heightPlane.GetComponent<HeightCommunicator> ().setAction ("Add");
    //		heightPlane.GetComponent<HeightCommunicator> ().setCaller ("World");

    //		 Summon slider.
    /**
    var hs = Instantiate(heightSliderPrefab) as GameObject;
    hs.transform.SetParent(Canvas.transform, false);
    hs.name = "HeightSlider";
    hs.GetComponent<SliderHelper>().setAction("Add");
    hs.GetComponent<SliderHelper>().setCaller("World");
    **/



    /**
    // Summon slider text.
    var st = Instantiate(sliderText) as GameObject;
    st.transform.SetParent(Canvas.transform, false);
    **/

    //		Destroy (heightPlane);

    /**
    Destroy(hs);
    Destroy(st);
    
    
    /** =============================================================================== NEGLECT ================================================================================ **/
}