using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class FlyingNew : MonoBehaviour {

    public Transform head;
    public Transform leftController;
    public Transform rightController;

    public VRInputModule m_InputModule;

    public SteamVR_Input_Sources leftHand;

    public SteamVR_Action_Boolean m_FlyAction;

    private Vector3 leftDir;
    private Vector3 rightDir;
    private Vector3 dir;

    private bool isFlying = false;


    // Update is called once per frame
    void Update()
    {
        if (m_FlyAction.GetStateDown(leftHand))
        {
            Debug.Log("Trigger Fire!");
            isFlying = !isFlying;
        }
        if (isFlying)
        {
            Debug.Log("You're Flying!");

            leftDir = leftController.transform.position - head.position;
            rightDir = rightController.transform.position - head.position;

            dir = leftDir + rightDir;

            head.transform.position += dir;
        }


    }
}
