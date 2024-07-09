using RosMessageTypes.Geometry;
using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.UnityRoboticsDemo;

/// <summary>
///
/// </summary>
public class RosPublisherExample : MonoBehaviour
{
    ROSConnection ros;
    public string topicName = "pos_rot";

    // The game object
    public GameObject cube;
    // Publish the cube's position and rotation every N seconds
    public float publishMessageFrequency = 0.5f;

    // Used to determine how much time has elapsed since the last message was published
    private float timeElapsed;

    void Start()
    {
        // start the ROS connection
        ros = ROSConnection.GetOrCreateInstance();
        ros.RegisterPublisher<RosMessageTypes.Geometry.PoseMsg>(topicName);
    }

    private void Update()
    {
        timeElapsed += Time.deltaTime;

        if (timeElapsed > publishMessageFrequency)
        {
            cube.transform.rotation = Random.rotation;
            PointMsg pointMsg = new PointMsg(cube.transform.position.x, cube.transform.position.y,
                cube.transform.position.z);
            QuaternionMsg quaternionMsg = new QuaternionMsg(cube.transform.rotation.x,cube.transform.rotation.y,cube.transform.rotation.z,cube.transform.rotation.w);
            RosMessageTypes.Geometry.PoseMsg cubePos = new RosMessageTypes.Geometry.PoseMsg(pointMsg,quaternionMsg);

            // Finally send the message to server_endpoint.py running in ROS
            ros.Publish(topicName, cubePos);

            timeElapsed = 0;
        }
    }
}