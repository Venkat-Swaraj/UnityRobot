using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.Geometry;

public class BinPublisher : MonoBehaviour
{
    public Transform bin;

    public Transform baselink;
    
    ROSConnection ros;
    public string topicName = "bin";
    // Start is called before the first frame update
    void Start()
    {
        baselink = this.transform.GetChild(0).transform;
        Debug.Log(baselink.name);
        ros = ROSConnection.GetOrCreateInstance();
        ros.RegisterPublisher<RosMessageTypes.Geometry.PoseMsg>(topicName);
    }

    // Update is called once per frame
    void Update()
    {
        
        PointMsg pointMsg = new PointMsg(bin.position.x, bin.position.y,
            bin.position.z);
        QuaternionMsg quaternionMsg = new QuaternionMsg(bin.rotation.x,bin.rotation.y,bin.rotation.z,bin.rotation.w);
        RosMessageTypes.Geometry.PoseMsg cubePos = new RosMessageTypes.Geometry.PoseMsg(pointMsg,quaternionMsg);

        // Finally send the message to server_endpoint.py running in ROS
        ros.Publish(topicName, cubePos);
    }
}
