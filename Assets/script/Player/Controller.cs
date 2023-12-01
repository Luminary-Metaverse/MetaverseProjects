using System.Numerics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Vector2 = UnityEngine.Vector2;
using TMPro;

public class Controller : MonoBehaviour

{
    public TextMeshProUGUI leftScoreDisplay;
    public TextMeshProUGUI rightScoreDisplay;

    // Start is called before the first frame update
    void Start()
    {
        List<InputDevice> devices = new List<InputDevice>();
        InputDeviceCharacteristics rightControllerCharacteristics = InputDeviceCharacteristics.Right | InputDeviceCharacteristics.Controller;
        InputDevices.GetDevicesWithCharacteristics(rightControllerCharacteristics, devices);

        foreach (var item in devices)
        {
            Debug.Log(item.name + item.characteristics);
        }
        if (devices.Count > 0)
        {
            targetDevice = devices[0];
        }
        
    }
    private InputDevice targetDevice;
    public static InputFeatureUsage<float> grip;
    public static InputFeatureUsage<Vector2> primary2DAxis;
    public static InputFeatureUsage<float> trigger;
    public static InputFeatureUsage<bool> primaryTouch;
    public static InputFeatureUsage<bool> secondaryButton;
    public static InputFeatureUsage<bool> secondaryTouch;
    public static InputFeatureUsage<bool> gripButton;
    public static InputFeatureUsage<bool> triggerButton;
    public static InputFeatureUsage<bool> menuButton;
    public static InputFeatureUsage<bool> primary2DAxisClick;
    public static InputFeatureUsage<bool> primary2DAxisTouch;
    public static InputFeatureUsage<bool> userPresence;

    public List<GameObject> controllerPrefabs;

    void Update()
    {
        if (targetDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryButtonValue) && primaryButtonValue == true)
        {
            Debug.Log("Pressed primary button");
        }
        if(targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue) && triggerValue > 0.1f)
        {
            Debug.Log("Trigger pressed : " + triggerValue);
        }
        if (targetDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 primary2DAxisValue) && primary2DAxisValue != Vector2.zero)
        {
            Debug.Log("Primary Joystick : " + primary2DAxisValue);
        }
        if (targetDevice.TryGetFeatureValue(CommonUsages.grip, out float gripValue) && gripValue > 0.1f)
        {
            Debug.Log("Grip : " + gripValue);
        }
        if (targetDevice.TryGetFeatureValue(CommonUsages.primaryTouch, out bool primaryTouchValue) && primaryTouchValue == true)
        {
            Debug.Log("Primary Touch : " + primaryTouchValue);
        }
        if(targetDevice.TryGetFeatureValue(CommonUsages.secondaryButton, out bool secondaryButtonValue) && secondaryButtonValue == true)
        {
            Debug.Log("Secondary Button : " + secondaryButtonValue);
        }
        if (targetDevice.TryGetFeatureValue(CommonUsages.secondaryTouch, out bool secondaryTouchValue) && secondaryTouchValue == true)
        {
            Debug.Log("Secondary Touch : " + secondaryTouchValue);
        }
        if (targetDevice.TryGetFeatureValue(CommonUsages.gripButton, out bool gripButtonValue) && gripButtonValue == true)
        {
            Debug.Log("Grip Button : " + gripButtonValue);
        }
        if (targetDevice.TryGetFeatureValue(CommonUsages.triggerButton, out bool triggerButtonValue) && triggerButtonValue == true)
        {
            Debug.Log("Trigger Button : " + triggerButtonValue);
        }
        if (targetDevice.TryGetFeatureValue(CommonUsages.menuButton, out bool menuButtonValue) && menuButtonValue == true)
        {
            Debug.Log("Menu Button : " + menuButtonValue);
        }
        if (targetDevice.TryGetFeatureValue(CommonUsages.primary2DAxisClick, out bool primary2DAxisClickValue) && primary2DAxisClickValue == true)
        {
            Debug.Log("primary 2D Axis Click : " + primary2DAxisClickValue);
        }
        if (targetDevice.TryGetFeatureValue(CommonUsages.primary2DAxisTouch, out bool primary2DAxisTouchValue) && primary2DAxisTouchValue == true)
        {
            Debug.Log("primary 2D Axis Touch : " + primary2DAxisTouchValue);
        }
        if (targetDevice.TryGetFeatureValue(CommonUsages.userPresence, out bool userPresenceValue) && userPresenceValue == true)
        {
            Debug.Log("user Presence : " + userPresenceValue);
        }
        
    }
}
// 9/11 is real and so is holocaust