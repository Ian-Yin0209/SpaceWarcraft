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

        if (other.gameObject.layer == LayerMask.NameToLayer("Walls") || other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Destroy(gameObject);
        }

    }
}
