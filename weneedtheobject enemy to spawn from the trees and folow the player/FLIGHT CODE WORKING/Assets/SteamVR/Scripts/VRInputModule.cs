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
    public float fireRate = 0.25f;
    public float weaponRange = 50.0f;
    public float hitForce = 100.0f;
    private WaitForSeconds shotDuraction = new WaitForSeconds(0.07f);
    private LineRenderer lazerLine;
    private float nextFire;
    public Transform gunEnd;

    //Bullet Variables
    public float bulletSpeed = 200.0f;
    public Rigidbody bullet;

    protected override void Awake()
    {
        base.Awake();
        
        m_Data = new PointerEventData(eventSystem);
        lazerLine = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        
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

        if(Input.GetKeyDown(KeyCode.Space))
        {
            nextFire = Time.time + fireRate;

            ProcessFireBullet();

            if (m_CurrentObject != null)
            {
                Destroy(m_CurrentObject);
            }
        }

        //Fire
        if (m_FireAction.GetStateDown(m_FireSource) && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;

            ProcessFire();

            if(m_CurrentObject != null)
            {
                Destroy(m_CurrentObject);
            }
        }

        //Controller Fly
        if (m_FlyAction.GetState(m_FlySource))
        {
            Debug.Log("Foward!");
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

    private IEnumerator ShotEffect()
    {
        lazerLine.enabled = true;

        yield return shotDuraction;

        lazerLine.enabled = false;
    }

    public PointerEventData GetData()
    {
        return m_Data;
    }

    void ProcessFire()
    {
        //Debug.Log("Firing lazer!");
        StartCoroutine(ShotEffect());

        RaycastHit hit;

        lazerLine.SetPosition(0, gunEnd.position);

        if(Physics.Raycast(gunEnd.position, gunEnd.forward, out hit, weaponRange))
        {
            //Debug.Log("That's a hit!");

            lazerLine.SetPosition(1, hit.point);

            m_LazerHit = hit.transform.gameObject;

            if(m_LazerHit.tag != "SaveMe") Destroy(m_LazerHit);
        }
        else
        {
            lazerLine.SetPosition(1, gunEnd.position + (gunEnd.forward * weaponRange));
        }
    }

    void ProcessFireBullet()
    {
        //Create Bullet Object
        Rigidbody bulletClone = (Rigidbody)Instantiate(bullet, gunEnd.position, gunEnd.rotation);

        //Fire the bullet forward
        bulletClone.velocity = gunEnd.forward * bulletSpeed;
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
        if(data.pointerPress == pointerUpHandler)
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
