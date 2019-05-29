using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using UnityEngine.EventSystems;

public class VRInputModule : BaseInputModule
{
    //Inputs
    public Transform camRig;
    public Camera player;
    public Camera m_Camera;
    public SteamVR_Input_Sources m_TargetSource;
    public SteamVR_Input_Sources m_FireSource;
    public SteamVR_Input_Sources m_FlySource;
    public SteamVR_Action_Boolean m_ClickAction;
    public SteamVR_Action_Boolean m_FireAction;
    public SteamVR_Action_Boolean m_FlyAction;

    //Data Variables
    private GameObject m_CurrentObject = null;
    private GameObject m_LazerHit = null;
    private PointerEventData m_Data = null;

    //Flight Variables
    public float moveSpeed = 0.25f;

    //Lazer Variables
    public float weaponRange = 50.0f;
    private WaitForSeconds shotDuraction = new WaitForSeconds(0.07f);
    private LineRenderer lazerLine;
    public Transform gunEnd;
    public GameObject newFriend;
    public GameObject newEnemy;
    public float lazerDelay = 60.0f;
    private float lazerTimer;
    private ParticleSystem myParticle;
    private float lazerDistanceStretchSpeed = 1.0f;
    private float currWeaponRange = 0.0f;

    protected override void Start()
    {
        myParticle = GetComponent<ParticleSystem>();
        myParticle.Stop();
    }

    protected override void Awake()
    {
        base.Awake();

        lazerTimer = lazerDelay;
        m_Data = new PointerEventData(eventSystem);
        lazerLine = GetComponent<LineRenderer>();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftAlt))
        {

            myParticle.Emit(1);
            Debug.Log("Trying to Fire");
        }
        if (m_FlyAction.GetState(m_FireSource) || Input.GetKey(KeyCode.Space))
        {
            lazerLine.enabled = true;
            myParticle.Emit(1);
            ProcessFire();
        }
        else if(Input.GetKeyUp(KeyCode.Space) || m_FlyAction.GetStateUp(m_FireSource))
        {
            myParticle.Stop();
            lazerLine.enabled = false;
            currWeaponRange = 0.0f;
        }
        lazerTimer--;
    }

    public override void Process()
    {
        //Reset data, set camera
        m_Data.Reset();
        m_Data.position = new Vector2(m_Camera.pixelWidth / 2, m_Camera.pixelHeight / 2);

        //Raycast
        eventSystem.RaycastAll(m_Data, m_RaycastResultCache);
        m_Data.pointerCurrentRaycast = FindFirstRaycast(m_RaycastResultCache);
        m_CurrentObject = m_Data.pointerCurrentRaycast.gameObject;

        //Clear
        m_RaycastResultCache.Clear();

        //Hover
        HandlePointerExitAndEnter(m_Data, m_CurrentObject);

        //Controller Fly
        if (m_FlyAction.GetState(m_FlySource))
        {
            //Debug.Log("Foward!");
            Quaternion orientation = player.transform.rotation;
            Vector3 moveDirection = orientation * Vector3.forward;
            Vector3 rigPos = camRig.transform.position;
            rigPos += (moveDirection * moveSpeed);
            camRig.transform.position = rigPos;
        }

        //Press
        if (m_ClickAction.GetStateDown(m_TargetSource))
            ProcessPress(m_Data);

        //Release
        if (m_ClickAction.GetStateUp(m_TargetSource))
            ProcessRelease(m_Data);
    }

    public PointerEventData GetData()
    {
        return m_Data;
    }

    void ProcessFire()
    {
        RaycastHit hit;

        lazerLine.SetPosition(0, gunEnd.position);

        if (Physics.Raycast(gunEnd.position, gunEnd.forward, out hit, currWeaponRange) && hit.transform.gameObject.tag != "SaveMe")
        {
            //Debug.Log("That's a hit!");

            lazerLine.SetPosition(1, hit.point);

            m_LazerHit = hit.transform.gameObject;

            if (m_LazerHit.tag == "fullPower" && lazerTimer <= 0)
            {
                Transform holdPosition = m_LazerHit.transform;
                Destroy(m_LazerHit);
                GameObject brandNew = (GameObject)Instantiate(newEnemy, holdPosition.position, holdPosition.rotation);
                lazerTimer = lazerDelay;
            }

            if (m_LazerHit.tag == "almostThere" && lazerTimer <= 0)
            {
                Transform holdPosition = m_LazerHit.transform;
                Destroy(m_LazerHit);
                GameObject brandNew = (GameObject)Instantiate(newFriend, holdPosition.position, holdPosition.rotation);
                lazerTimer = lazerDelay;
            }
        }
        else
        {
            if(currWeaponRange < weaponRange)
            {
                currWeaponRange += lazerDistanceStretchSpeed;
                lazerLine.SetPosition(1, gunEnd.position + (gunEnd.forward * currWeaponRange));
            }
            else
            {
                lazerLine.SetPosition(1, gunEnd.position + (gunEnd.forward * weaponRange));
            }
        }
    }

    private void ProcessPress(PointerEventData data)
    {
        //Set raycast
        data.pointerPressRaycast = data.pointerCurrentRaycast;

        //Check for object hit, get the down handler, call
        GameObject newPoiterPress = ExecuteEvents.ExecuteHierarchy(m_CurrentObject, data, ExecuteEvents.pointerDownHandler);

        //If no down handler, try and get click handler
        if (newPoiterPress == null)
            newPoiterPress = ExecuteEvents.GetEventHandler<IPointerClickHandler>(m_CurrentObject);

        //Set data
        data.pressPosition = data.position;
        data.pointerPress = newPoiterPress;
        data.rawPointerPress = m_CurrentObject;
    }

    private void ProcessRelease(PointerEventData data)
    {
        //Execute pointer up
        ExecuteEvents.Execute(data.pointerPress, data, ExecuteEvents.pointerUpHandler);

        //Check for click handler
        GameObject pointerUpHandler = ExecuteEvents.GetEventHandler<IPointerClickHandler>(m_CurrentObject);

        //Check if actual
        if (data.pointerPress == pointerUpHandler)
        {
            ExecuteEvents.Execute(data.pointerPress, data, ExecuteEvents.pointerUpHandler);
        }

        //Clear selected gameobject
        eventSystem.SetSelectedGameObject(null);

        //Reset data
        data.pressPosition = Vector2.zero;
        data.pointerPress = null;
        data.rawPointerPress = null;
    }

}
