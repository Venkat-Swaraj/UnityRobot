using System.Collections;
using System.Collections.Generic;
using RosMessageTypes.Geometry;
using RosMessageTypes.Std;
using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.UnityRoboticsDemo;


public class Publisher : MonoBehaviour
{
    ROSConnection ros;
    
    public string topicName = "unity_msg";
    // Start is called before the first frame update
    void Start()
    {
        ros = ROSConnection.GetOrCreateInstance();
        ros.RegisterPublisher<RosMessageTypes.Geometry.PoseMsg>(topicName);
    }

    // Update is called once per frame
    void Update()
    {
        PointMsg pointMsg = new PointMsg();
        RosMessageTypes.Geometry.PoseMsg stringswaMsg = new PoseMsg();
        ros.Publish(topicName,stringswaMsg);
    }
}
