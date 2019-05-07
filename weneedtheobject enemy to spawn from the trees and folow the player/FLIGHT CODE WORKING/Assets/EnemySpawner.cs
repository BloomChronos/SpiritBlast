using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    // Use this for initialization

    public GameObject enemy;

	void Start () {
       // Spawn(enemy);
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.Space)){
            Spawn(enemy);
        }
	}

    public void Spawn(GameObject obj)
    {
        
        Instantiate(enemy, gameObject.transform.position, gameObject.transform.rotation);
    }

}
