using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunTurret : MonoBehaviour
{
    LineRenderer line;
    GameObject lockedEnemy = null;
    public GameObject bullet;
    int coolDown;
    public float buildingProgress = 0.0f;

    // New - Tower health
    [SerializeField] int turretHealth;

    SoundManager soundManager;
    // Start is called before the first frame update
    void Start()
    {
        soundManager = FindObjectOfType<SoundManager>();
        line = gameObject.AddComponent<LineRenderer>();
        line.positionCount = 2;
        line.enabled = false;
        line.startWidth = 0.009f;
        line.endWidth = 0.009f;
        GetComponent<Renderer>().material.color = Color.gray;

    }

    void RefreshLine()
    {
        line.SetPosition(0, transform.position);
        line.SetPosition(1, transform.position + transform.forward * 10);
        line.enabled = true;
    }

    public void build(float value)
    {
        buildingProgress += value;
        if (isFinished())
        {
            buildingProgress = 1.0f;
            GetComponent<Renderer>().material.color = Color.yellow;
        }
    }

    public bool isFinished()
    {
        return buildingProgress >= 1.0f;
    }

    public void ReduceHealth()
    {
        turretHealth--;
    }

    // Update is called once per frame
    void Update()
    {
        RefreshLine();

        if (turretHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        if (!isFinished())
        {
            return;
        }
        if (lockedEnemy == null)
        {
            line.material.color = Color.green;
            //transform.RotateAround(transform.up, 1f * Time.fixedDeltaTime);
            transform.Rotate(transform.up * 80f * Time.fixedDeltaTime);
            RaycastHit hit;
            Physics.Raycast(transform.position, transform.forward, out hit);
            if ((hit.distance > -1 && hit.distance < 15) && hit.collider != null && (hit.collider.gameObject.CompareTag("Enemy") || hit.collider.gameObject.CompareTag("EnemyBody")))
            {
                lockedEnemy = hit.collider.gameObject;
                Debug.Log("Lock: " + hit.collider.gameObject + ": " + hit.distance);
                line.material.color = Color.red;
            }
        }
        else
        {
            var pos = lockedEnemy.transform.position;
            pos.y = 1;
            transform.LookAt(pos);
            if (coolDown == 0)
            {
                soundManager.PlaySound("player_shoot");
                Instantiate(bullet, transform.position + transform.forward * 2, transform.rotation);
                coolDown = 20;
            }
            coolDown--;
            if ((pos - transform.position).magnitude > 12)
            {
                //lockedEnemy = null;
            }
        }
    }
}