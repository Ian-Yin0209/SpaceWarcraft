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
        if ((transform.position - startPosition).magnitude > 50)
        {
            Destroy(gameObject);
        }    
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy")) 
        {
           collision.gameObject.GetComponent<Rigidbody>().AddForce(transform.forward * 0.01f);
           collision.gameObject.GetComponent<Enemy>().ReduceHealth();
        }

        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("EnemyBody"))
        {
            print("hit body");
            other.gameObject.transform.parent.GetComponent<Enemy>().ReduceHealth();
            //Destroy(other.gameObject.transform.parent.gameObject);
            Destroy(gameObject);
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Walls") || other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Destroy(gameObject);
        }

        Destroy(gameObject,1f);
    }
}