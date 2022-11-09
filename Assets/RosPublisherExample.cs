using UnityEngine;
using System.Collections.Generic;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.UnityRoboticsDemo;
using UnityEngine.XR;

/// <summary>
///
/// </summary>
public class RosPublisherExample : MonoBehaviour
{
    ROSConnection ros;
    public string topicName = "ackermann_curvature_drive_fake";

    // The game object
    public GameObject cube;
    // Publish the cube's position and rotation every N seconds
    public float publishMessageFrequency = 0.5f;

    // Used to determine how much time has elapsed since the last message was published
    private float timeElapsed;

    InputDevice targetDevice;

    void Start()
    {
        // start the ROS connection
        ros = ROSConnection.GetOrCreateInstance();
        ros.RegisterPublisher<PosRotMsg>(topicName);

        var inputDevices = new List<UnityEngine.XR.InputDevice>();
        //InputDevicesCharacteristics temp = InputDevicesCharacteristics.Right | InputDevicesCharacteristics.Controller;
        UnityEngine.XR.InputDevices.GetDevices(inputDevices);


        foreach (var device in inputDevices)
        {
            Debug.Log(string.Format("Device found with name '{0}' and role '{1}'", device.name, device.role.ToString()));
            targetDevice = device;
        }
    }

    private void Update()
    {
        timeElapsed += Time.deltaTime;

        if (timeElapsed > publishMessageFrequency)
        {
            //cube.transform.rotation = Random.rotation;
            targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float testTwo);
            if (testTwo > .1f)
            {
                cube.transform.Rotate(new Vector3(30, 30, 30));
                Debug.Log("testTwo");
            }
            float velocity = 0;
            float curvature = 0;
            targetDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 testThree);
            if (testThree != Vector2.zero)
            {
                curvature = -testThree.x;
                velocity = testThree.y;
            }

            PosRotMsg cubePos = new PosRotMsg(
                testThree.x,
                testThree.y,
                cube.transform.position.z,
                cube.transform.rotation.x,
                cube.transform.rotation.y,
                cube.transform.rotation.z,
                cube.transform.rotation.w,
                velocity,
                curvature
            );

            // Finally send the message to server_endpoint.py running in ROS
            ros.Publish(topicName, cubePos);

            timeElapsed = 0;
        }
    }
}