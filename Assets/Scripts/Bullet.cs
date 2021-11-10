using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Vector3 startPosition;
    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        GetComponent<Rigidbody>().AddForce(transform.forward * 50, ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        if ((transform.position - startPosition).magnitude > 20)
        {
            Destroy(gameObject);
        }    
    }

    private void OnCollisionEnter(Collision collision)
    {
        //if (collision.gameObject.CompareTag("Enemy")) 
        //{
        //    collision.gameObject.GetComponent<Enemy>().ReduceHealth();
        //    //Destroy(collision.gameObject);
        //}

        //Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("EnemyBody"))
        {
            other.gameObject.transform.parent.GetComponent<Enemy>().ReduceHealth();
            //Destroy(collision.gameObject);
            Destroy(gameObject);
        }
        
    }
}