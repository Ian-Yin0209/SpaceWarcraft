using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;



public class Enemy : MonoBehaviour
{
    public GameObject dropItem;

    public NavMeshAgent agent;
    GameObject target;

    // New Enemy Attack
    bool isPlayerInRadius = false;
    bool isGunTurretInRadius = false;
    bool provoked = false;
    [SerializeField] GameObject enemyBullet;
    [SerializeField] Transform bulletPoint;
    float timeToShoot = 0.3f;

    GameObject player;
    [SerializeField] GameObject gunTurret;

    [SerializeField] int maxHealth = 5;
    [SerializeField] int meleeAttack = 0;
    GameObject[] newObject = new GameObject[50];
    int n = 0;

    // Enemy Health
    int health;

    [SerializeField] int meleeCooldown = 0;

    //[SerializeField] GameObject pickupHolder;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0) 
        {
            DropItem();
            Destroy(gameObject);
        }

        // timer
        timeToShoot -= Time.deltaTime;

        if (target != null && agent != null)
            agent.SetDestination(target.transform.position);

        // When enemy provoked from player bullets
        if (provoked) 
        {
            target = player;
        }

        // Attack player
        if (isPlayerInRadius)
        {
            target = player;
            if (timeToShoot <= 0) 
            {
                Attack();
                timeToShoot = 0.3f;
            }
        }

        // Attack turret
        else if (isGunTurretInRadius && !isPlayerInRadius)
        {
            foreach (GameObject gameObject in newObject)
            {
                if (gameObject != null) 
                {
                    target = gameObject;
                    
                    if (timeToShoot <= 0)
                    {
                        Attack();
                        timeToShoot = 0.3f;
                    }
                }
            }
        }
    }

    public void ReduceHealth() 
    {
        health--;
    }

    void move(float speed)
    {
        GetComponent<Rigidbody>().MovePosition(transform.position + transform.forward * Time.fixedDeltaTime * speed);
    }

    void turn(float speed)
    {
        if (target != null) 
        {
            Vector3 dir = target.transform.position - transform.position;
            dir.y = 0;
            Quaternion q = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, q, speed);
        }
    }

    private void FixedUpdate()
    {
        meleeCooldown -= 1;
        if (meleeCooldown > 0){
            print(meleeCooldown);
        }
        //float angle = Vector3.Angle(target.transform.position - transform.position, transform.forward);
        turn(0.7f);
        //if (target != null && angle < 25f || (target.transform.position - transform.position).magnitude < 2)
        //{
        //    //GetComponent<Animator>().SetBool("is walk", true);
        //    move(8f);
        //}
        //else
        //{
        //    //GetComponent<Animator>().SetBool("is walk", false);
        //    turn(0.7f);
        //}
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (meleeCooldown <= 0 && collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerHealth>().ReduceHealth(meleeAttack);
            meleeCooldown = 40;   
            //Destroy(collision.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bullet")) 
        {
            provoked = true;
        }

        if (other.gameObject.CompareTag("Player")) 
        {
            isPlayerInRadius = true;
        }

        if (other.gameObject.CompareTag("GunTurret"))
        {
            isGunTurretInRadius = true;
            newObject[n] = other.gameObject;
            n += 1;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isPlayerInRadius = false;
            provoked = false;
        }

        if (other.gameObject.CompareTag("GunTurret"))
        {
            isGunTurretInRadius = false;
            provoked = false;
        }
    }

    public void DropItem()
    {
        //Debug.Log(Random.Range(0f, 1f));
        //if (Random.Range(0f, 1f) > 0.66f)
        //{

        //}

        var item = Instantiate(dropItem, transform.position, dropItem.transform.rotation);
        item.transform.position = new Vector3(item.transform.position.x, 1, item.transform.position.z);
    }

    public void Attack()
    {
        Instantiate(enemyBullet, bulletPoint.transform.position, transform.rotation);
        FindObjectOfType<SoundManager>().PlaySound("enemy_shoot");
    }
}
