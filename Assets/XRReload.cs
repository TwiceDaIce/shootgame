using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Management;

public class XRReload : MonoBehaviour
{
    public async void Awake()
    {
        await Task.Delay(10);
        if (XRGeneralSettings.Instance.Manager.activeLoader != null)
        {
            XRGeneralSettings.Instance.Manager.StopSubsystems();
            XRGeneralSettings.Instance.Manager.DeinitializeLoader();
        }
        XRGeneralSettings.Instance.Manager.InitializeLoaderSync();
        XRGeneralSettings.Instance.Manager.StartSubsystems();

        //var actions = new InputAction();
        //actions.Enable();

        //actions.vrControls.leftAnalogPos.performed += ctx => Debug.LogError( ctx.ReadValue<Vector2>() );
    }
}
