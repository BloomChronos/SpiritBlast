using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour {

    // Use this for initialization

    public void OnCollisionEnter(Collision collision)
    {


        Debug.Log("Collide");
        if(collision.gameObject.tag == "SaveMe")
        {

            Debug.Log("Hit!");
            //SceneManager.LoadScene("MichellesMaze", LoadSceneMode.Single);
            SceneManager.LoadScene("michaelssparklelevel", LoadSceneMode.Single);
            
        }


    }

    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
