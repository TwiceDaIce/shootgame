using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class NewBehaviourScript : MonoBehaviour
{
    // Start is called before the first frame update
    public UnityEngine.XR.InputDevice device;
    void Start()
    {
        var inputDevices = new List<UnityEngine.XR.InputDevice>();
        UnityEngine.XR.InputDevices.GetDevices(inputDevices);

        foreach (var device in inputDevices)
        {
#pragma warning disable CS0618 // Type or member is obsolete
            Debug.Log(string.Format("Device found with name '{0}' and role '{1}'", device.name, device.role.ToString()));
#pragma warning restore CS0618 // Type or member is obsolete
            this.device = device;
        }
        var inputFeatures = new List<UnityEngine.XR.InputFeatureUsage>();
        if (device.TryGetFeatureUsages(inputFeatures))
        {
            foreach (var feature in inputFeatures)
            {
                if (feature.type == typeof(bool))
                {
                    bool featureValue;
                    if (device.TryGetFeatureValue(feature.As<bool>(), out featureValue))
                    {
                        Debug.Log(string.Format("Bool feature {0}'s value is {1}", feature.name, featureValue.ToString()));
                    }
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        bool triggerValue;
        if (device.TryGetFeatureValue(UnityEngine.XR.CommonUsages.triggerButton, out triggerValue) && triggerValue)
        {
            Debug.Log("Trigger pressed lol");
        }
    }
}
