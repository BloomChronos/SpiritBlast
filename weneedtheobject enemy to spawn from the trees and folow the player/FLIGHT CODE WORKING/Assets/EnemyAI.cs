using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour {

    public GameObject Target;
    public float MoveSpeed;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //Vector3 Direction = Target.transform.position - gameObject.transform.position;
       // Direction.Normalize();
       // Direction *= 10;
       // gameObject.transform.position += Direction;

    }
}
