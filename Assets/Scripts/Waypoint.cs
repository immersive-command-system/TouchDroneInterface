using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TouchScript.Behaviors;
using TouchScript.Gestures.TransformGestures;
using TouchScript.Gestures;
using UnityEngine.UI;
using ROSBridgeLib;
using ROSBridgeLib.interface_msgs;

// Right now, I'm not doing anything to support movign waypoint with height information. Also, worldPos and flatPos is not being updated.

public class Waypoint : MonoBehaviour {

	private TransformGesture gesture;

    public GameObject heightSliderPrefab;
    public GameObject Canvas;
    public GameObject sliderText;

    public Vector3 currPos;
	private Vector3 lastPos;
	public String waypointID;

	public Vector3 worldPos;
	public Vector3 flatPos;
	private float TempHeight;

    private GameObject CommandText;

    private GameObject Master;

    public HoverText MyHoverText;

    private bool added;

	// Indicating whether or not the waypoint has been completed by ROS.
	public bool WaypointComplete;

	void Start() {

        Canvas = GameObject.Find("Canvas");

        // Convoluted way of getting a reference to a prefab.

        GameObject world = GameObject.Find("World");

        TempHeight = 0f;	
		this.gameObject.tag="Waypoint";
		WaypointComplete = false;

        CommandText = GameObject.Find("CommandText");

        Master = GameObject.Find("Master");

        added = false;

    }


    public void CreateHoverText()
    {

        // Hover Text

        HoverTextContainerScript htcs = Master.GetComponent<HoverTextContainerScript>();

        GameObject HoverTextPrefab = htcs.HoverTextPrefab;

        GameObject HoverText = Instantiate(HoverTextPrefab) as GameObject;

        HoverText.transform.SetParent(this.transform, false);

        HoverText ht = HoverText.GetComponent<HoverText>();

        ht.SetAttachedWaypoint(this.gameObject.name);

        SetMyHoverText(ht);
    }

    private void SetMyHoverText(HoverText HoverText)
    {

        MyHoverText = HoverText;

    }

    void Update() {
        if (!WaypointComplete)
        {
            currPos = this.transform.position;
            if (currPos != lastPos)
            {

                flatPos = currPos;

                Vector3 sub = worldPos;
                worldPos = new Vector3(currPos.x, sub.y, currPos.z);
            }

            lastPos = currPos;
        } else
        {
            Transformer t = this.GetComponent<Transformer>();
            TransformGesture tg = this.GetComponent<TransformGesture>();

            //            Destroy(t);
            //            Destroy(tg);

            //            t.enabled = !t.enabled;
            //            tg.enabled = !tg.enabled;

            t.enabled = false;
            tg.enabled = false;
        }
	}


    private void OnEnable()
	{
		// The gesture
		gesture = GetComponent<TransformGesture>();

        // Subscribe to gesture events
        GetComponent<TapGesture>().Tapped += tappedHandler;
        gesture.TransformStarted += transformStartedHandler;
        gesture.TransformCompleted += transformCompletedHandler;
    }

	private void OnDisable()
	{
        // Unsubscribe from gesture events
        GetComponent<TapGesture>().Tapped -= tappedHandler;
        gesture.TransformStarted -= transformStartedHandler;
		gesture.TransformCompleted -= transformCompletedHandler;
     }

    private void tappedHandler(object sender, EventArgs e)
    {

        GameObject InfoText = GameObject.Find("InfoText");
        InfoText.GetComponent<InfoText>().SetCurrWaypoint(this.gameObject.name);

        UpdateCommandText(this.gameObject.name);

    }

    private void transformStartedHandler(object sender, EventArgs e)
	{

        // When movement starts we need to tell physics that now WE are moving this object manually

        GameObject InfoText = GameObject.Find("InfoText");
        InfoText.GetComponent<InfoText>().SetCurrWaypoint(this.gameObject.name);

        UpdateCommandText(this.gameObject.name);

        MovingYes();
    }

    private void UpdateCommandText(string name)
    {

        CommandTextScript cts = CommandText.GetComponent<CommandTextScript>();
        // print("Update Command Text.");
        cts.SetTextToSelectedWaypoint(name);

    }

	private void transformCompletedHandler(object sender, EventArgs e)
	{

        //MovedWaypoint (currPos);
        MovingNo();

        SendROSModifyMessage();

	}

	private float SetHeight(float height) 
	{

        // This is the ratio.
        Debug.Log("heldaf");
		TempHeight = height;
		return TempHeight;
	}

	public void SetID(String id) 
	{
		waypointID = id;
	}

	public void setFlatPos(Vector3 v) {
		flatPos = v;
	}

	public void setWorldPos(Vector3 v)
	{
		worldPos = v;
	}

	public void setCurrPos(Vector3 v) {
		currPos = v;
	}

    public void setLastPos(Vector3 v) {
        lastPos = v;
    }

    public void DisableTapGesture()
    {

        this.GetComponent<TapGesture>().enabled = false;
             
    }

    public void EnableTapGesture()
    {

        this.GetComponent<TapGesture>().enabled = true;
    }

    public void DisableTransformGesture()
    {

        this.GetComponent<TransformGesture>().enabled = false;

    }

    public void EnabledTransformGesture()
    {

        this.GetComponent<TransformGesture>().enabled = true;

    }

    public void DisableTransformer()
    {

        this.GetComponent<Transformer>().enabled = false;

    }

    public void EnableTransformer()
    {

        this.GetComponent<Transformer>().enabled = true;
    }

    private void MovingYes()
    {
        GameObject it = GameObject.Find("InfoText");

        InfoText infot = it.GetComponent<InfoText>();

        infot.YesMoving();
    }

    private void MovingNo()
    {
        GameObject it = GameObject.Find("InfoText");

        InfoText infot = it.GetComponent<InfoText>();

        infot.NoMoving();
    }

    private void WaitingYes()
    {
        GameObject it = GameObject.Find("InfoText");

        InfoText infot = it.GetComponent<InfoText>();

        infot.YesWaiting();
    }

    private void WaitingNo()
    {
        GameObject it = GameObject.Find("InfoText");

        InfoText infot = it.GetComponent<InfoText>();

        infot.NoWaiting();
    }

    /**
     * 
     * Method called by DetailBoxDragHelper.
     * This will only update the height attribute in the
     * worldPos.
     * 
     **/
    public void UpdateWorldHeight(float value)
    {

        if (!added)
        {
            worldPos.y = value;
            added = true;
        }
        else
        {
            worldPos.y = value;

            SendROSModifyMessage();
        }

    }

    private void SendROSModifyMessage() 
    {

        //TODO: Determine Why Tom has height being set to worldPos on edit (Kind of breaks the ability to edit) 
        WorldScript ws = GameObject.Find("World").GetComponent<WorldScript>();
        
        string prevID = ws.GetPrevID(this.name, "modify");
        float worldX = this.transform.localPosition.x;
        float worldZ = this.transform.localPosition.z;
        float height = this.worldPos.y;
        UserpointInstruction msg = new UserpointInstruction(this.name, prevID, worldX, height, worldZ, "MODIFY");
        GameObject.Find("Master").GetComponent<ROSDroneConnection>().PublishWaypointUpdateMessage(msg);

    }

    public void ColorUpdatePrev()
    {
        // GameObject w = GameObject.Find(prevWaypoint);
        // Waypoint waypoint = w.GetComponent<Waypoint>();

        Color filled = new Color(0.278F, 0.874F, 1F, 0.9F);
        this.GetComponent<Renderer>().material.color = filled;
    }

    public void ColorUpdateCurrent()
    {
        //GameObject w = GameObject.Find(curr);
        //Waypoint waypoint = w.GetComponent<Waypoint>();

        Color selected = new Color(1F, 1F, 1F, 1F);
        this.GetComponent<Renderer>().material.color = selected;

    }

    public void ColorUpdateAdd()
    {

        //GameObject w = GameObject.Find(curr);
        //Waypoint waypoint = w.GetComponent<Waypoint>();

        Color adding = new Color(0.588F, 0.588F, 0.588F, 0.6F);
        this.GetComponent<Renderer>().material.color = adding;

    }






    // Previous implementation of move Waypoint.
    // EVERYTHING WORKS. THIS IS GOOD CODE.

    /**
     * 
     * 
	private void MovedWaypoint(Vector3 currPos) 
	{
        // Update worldPos with height adjustment.
        StartCoroutine(WaitHeight(currPos));
	}

	IEnumerator WaitHeight(Vector3 currPos)
	{

        //		 Summon the plane.
        // var heightPlane = GameObject.CreatePrimitive(PrimitiveType.Plane);

        // Resizing, rotation, and position of plane.

        // Pick position.

        //		heightPlane.transform.localPosition = new Vector3 (7.78f, 0f, 4.56f);

        // or

        //		heightPlane.transform.localPosition = new Vector3 (9.5f, 0f, 4.56f);


        //		heightPlane.transform.localScale = new Vector3(0.5f, 1f, 0.038f);
        //		heightPlane.transform.localRotation = Quaternion.Euler(0, 90, 0);
        //		heightPlane.AddComponent<TapGesture> ();
        //		heightPlane.AddComponent<HeightCommunicator> ();

        //		heightPlane.GetComponent<HeightCommunicator> ().setAction ("Move");
        //		heightPlane.GetComponent<HeightCommunicator> ().setCaller (waypointID);

        //		 Summon slider.

        WaitingYes();

        var hs = Instantiate(heightSliderPrefab) as GameObject;
        hs.transform.SetParent(Canvas.transform, false);
        hs.name = "HeightSlider";
        hs.GetComponent<SliderHelper>().setAction("Move");
        hs.GetComponent<SliderHelper>().setCaller(waypointID);


        // Summon slider text.
        var st = Instantiate(sliderText) as GameObject;
        st.transform.SetParent(Canvas.transform, false);

        // Waiting player for height.
        //		print ("Waiting for a height.");
        yield return new WaitUntil (() => TempHeight != 0);
		//		print ("Height received.");

		// Set new worldPos
		worldPos = new Vector3(currPos.x, TempHeight, currPos.z);
		// Housekeeping
		TempHeight = 0f;
        // Destroy (heightPlane);

        Destroy(hs);
        Destroy(st);
        WaitingNo();
    }

    *
    *
    * 
    **/


}
