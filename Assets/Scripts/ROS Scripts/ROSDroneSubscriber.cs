using ROSBridgeLib;
using ROSBridgeLib.std_msgs;
using ROSBridgeLib.interface_msgs;
using System.Collections;
using SimpleJSON;
using UnityEngine;

public class ROSDroneSubscriber : ROSBridgeSubscriber
{
    public static string GetMessageTopic()
    {
        return "/state/position_velocity";
    }

    public static string GetMessageType()
    {
        return "crazyflie_msgs/PositionVelocityStateStamped";
    }

    public static ROSBridgeMsg ParseMessage(JSONNode msg)
    {
        return new DronePositionMsg(msg);
    }

    public static void CallBack(ROSBridgeMsg msg)
    {
        //Debug.Log("callback");
        GameObject robot = GameObject.FindWithTag("Drone");
        if (robot == null)
            Debug.Log("The RosDroneSubscriber script can't find the robot.");
        else
        {
            DronePositionMsg pose = (DronePositionMsg)msg;
            robot.transform.localPosition = new Vector3(pose._x, pose._z, -pose._y) + new Vector3(0.0044f, -0.0388f, 0.0146f);;
        }
    }
}