using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour {

    // Use this for initialization

    public void OnCollision(Collision collision)
    {
        if(collision.gameObject.tag == "SaveMe")
        {
            SceneManager.LoadScene("MichellesMaze", LoadSceneMode.Additive); 


        }


    }

    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
