using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserScript : MonoBehaviour {

    public Camera cam;
    public GameObject firePoint;
    public LineRenderer lr;
    public float maximumLength;
    
	// Update is called once per frame
	void Update ()
    {
        lr.SetPosition(0, firePoint.transform.position);

        RaycastHit hit;

        if (Physics.Raycast(firePoint.transform.position, firePoint.transform.forward, out hit, maximumLength))
        {
            lr.SetPosition(1, hit.point);
        }
        else
        {
            lr.SetPosition(1, firePoint.transform.position + (firePoint.transform.forward * maximumLength));
        }
	}
}
