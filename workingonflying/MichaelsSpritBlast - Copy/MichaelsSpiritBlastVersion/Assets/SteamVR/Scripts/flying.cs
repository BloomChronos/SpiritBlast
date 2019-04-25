using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using UnityEngine.EventSystems;


public class Flying : MonoBehaviour
{
	
	public Transform head;
    public Transform leftController;

	public SteamVR_Input_Sources leftHand;
    public SteamVR_TrackedObject theLeft;

    public SteamVR_Action_Boolean m_FlyAction;

    private Vector3 leftDir;
    private Vector3 rightDir;
    private Vector3 dir;

	private bool isFlying = false;
	
	
	// Update is called once per frame
	void Update () {
		if (m_FlyAction.GetStateDown(leftHand))
		{
			isFlying = !isFlying;
		}
		if (isFlying)
		{
            leftDir = leftController.transform.position - head.position;
			rightDir = leftController.transform.position - head.position;

			dir = leftDir + rightDir;

			transform.position += dir;
		}


	}
}