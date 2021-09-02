using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public enum HandType
{
    Left,
    Right
}

public class XRHandController : MonoBehaviour
{
    public HandType handType;
    public float thumbMoveSpeed = 0.1f;

    private Animator animator;
    private InputDevice inputDevice;

    private float indexValue;
    private float thumbValue;
    private float threeFingersValue;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        inputDevice = GetInputDevice();
    }

    // Update is called once per frame
    void Update()
    {
        AnimateHand();
    }

    InputDevice GetInputDevice()
    {
        InputDeviceCharacteristics controllerCharacteristic = InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.Controller;

        if (handType == HandType.Left)
        {
            controllerCharacteristic = controllerCharacteristic | InputDeviceCharacteristics.Left;
        }
        else
        {
            controllerCharacteristic = controllerCharacteristic | InputDeviceCharacteristics.Right;
        }

        List<InputDevice> inputDevices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(controllerCharacteristic, inputDevices);

        return inputDevices[0];
    }

    void AnimateHand()
    {
        
        if(inputDevice.TryGetFeatureValue(CommonUsages.trigger, out float indexValue))
        {
            
            animator.SetFloat("Trigger", indexValue);
        }
        else
        {
            animator.SetFloat("Trigger", 0);
        }

        if(inputDevice.TryGetFeatureValue(CommonUsages.grip, out float threeFingersValue))
        {
            animator.SetFloat("Grip", threeFingersValue);
        }
        else
        {
            animator.SetFloat("Grip", 0);
        }
        
    }
}
