using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public float lifetime = 2.0f;

    void Awake()
    {
        Destroy(gameObject, lifetime);
    }
}
