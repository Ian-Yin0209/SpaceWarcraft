using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
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
        //transform.Translate(Vector3.forward * 100f * Time.deltaTime);

        if ((transform.position - startPosition).magnitude > 20)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //if (collision.gameObject.CompareTag("Enemy"))
        //{
        //    collision.gameObject.GetComponent<Enemy>().DropItem();
        //    Destroy(collision.gameObject);
        //}

        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //Destroy(collision.gameObject);
            other.gameObject.GetComponent<PlayerHealth>().ReduceHealth();
            Destroy(gameObject);
        }

        if (other.gameObject.CompareTag("GunTurret"))
        {
            other.gameObject.GetComponent<GunTurret>().ReduceHealth();
            Destroy(gameObject);
        }

        if (other.gameObject.CompareTag("Walls") || other.gameObject.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }
}
