using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewEnemyai : MonoBehaviour
{
    public GameObject Target;
    public GameObject Bullet;
    [SerializeField] private float speed;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {


        Follow(Target);
    }



    public void Follow(GameObject Targ) //Follows the target(Typically the player
    {
         float step =  speed * Time.deltaTime; // calculate distance to move
        transform.position = Vector3.MoveTowards(transform.position, Targ.transform.position, step);
        //Vector3 Direction = Targ.transform.position - gameObject.transform.position;
        transform.LookAt(Targ.transform);
        //Direction.Normalize();
        //Direction *= 0.01f;
        //gameObject.transform.position += Direction;

    }

    public void Shoot(GameObject Projectile)//Shoot projectiles at the player
    {
        Instantiate(Bullet, gameObject.transform);

        
    }

}