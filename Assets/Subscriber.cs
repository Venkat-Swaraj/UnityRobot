using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Robotics.ROSTCPConnector;

public class Subscriber : MonoBehaviour
{
    [SerializeField] private GameObject[] spheres;
    // Start is called before the first frame update
    void Start()
    {
        ROSConnection.GetOrCreateInstance().Subscribe<RosMessageTypes.Tf2.TFMessageMsg>("tf", logger);
        /*RosMessageTypes.Std.StringMsg
        ROSConnection.GetOrCreateInstance().Subscribe<RosMessageTypes.Geometry.PointMsg>("color");*/
    }

    void logger(RosMessageTypes.Tf2.TFMessageMsg message)
    {
        int index = 0;
        foreach (var sphere in spheres)
        {
            sphere.transform.position = new Vector3((float)message.transforms[index].transform.translation.x,
                (float)message.transforms[index].transform.translation.y,
                (float)message.transforms[index].transform.translation.z);
            sphere.transform.rotation = new Quaternion((float)message.transforms[index].transform.rotation.x,
                (float)message.transforms[index].transform.rotation.y, (float)message.transforms[index].transform.rotation.z,
                (float)message.transforms[index].transform.rotation.w);
            index++;
        }
        Debug.Log(message.transforms[2].transform.ToString());
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
