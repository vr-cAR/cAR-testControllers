using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class NewBehaviourScript : MonoBehaviour
{
    InputDevice targetDevice;
    // Start is called before the first frame update
    void Start()
    {
        var inputDevices = new List<UnityEngine.XR.InputDevice>();
        //InputDevicesCharacteristics temp = InputDevicesCharacteristics.Right | InputDevicesCharacteristics.Controller;
        UnityEngine.XR.InputDevices.GetDevices(inputDevices);


        foreach (var device in inputDevices)
        {
            Debug.Log(string.Format("Device found with name '{0}' and role '{1}'", device.name, device.role.ToString()));
            targetDevice = device;
        }
    }

    // Update is called once per frame
    void Update()
    {
        targetDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool testOne);
        if (testOne)
        {
            Debug.Log("testOne");
        }
        targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float testTwo);
        if (testTwo > .1f)
        {
            Debug.Log("testTwo");
        }
        
        targetDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 testThree);
        if (testThree != Vector2.zero)
        {
            Debug.Log("testThree");
            if (testThree.x < 0)
            {
                Debug.Log("Move Left");
            }
            if (testThree.x > 0)
            {
                Debug.Log("Move Right");
            }
            if (testThree.y < 0)
            {
                Debug.Log("Move Forward");
            }
            if (testThree.y > 0)
            {
                Debug.Log("Move Backwards");
            }
        }
        
    }
}
