using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] int health;
    [SerializeField] int stamina;
    [SerializeField] int maxStamina;
    public bool canRun;
    float recoverTime;
    float exhaustTime;

    // Start is called before the first frame update
    void Start()
    {
        stamina = maxStamina;
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0) 
        {
            Destroy(this.gameObject);
        }

        if (stamina <= 0) 
        {
            stamina = 0;
            canRun = false;
        }

        if (stamina > 0) 
        {
            canRun = true;
        }
    }

    public void ReduceHealth(int demage) 
    {
        health -= demage;
        if (demage > 1){
            Debug.Log(health.ToString() + ' ' + demage.ToString());
        }
    }

    public void ReduceStamina() 
    {
        exhaustTime -= Time.deltaTime;

        if (exhaustTime <= 0) 
        {
            stamina--;
            exhaustTime = 0.02f;
        }
        
    }

    public void RecoverStamina() 
    {
        recoverTime -= Time.deltaTime;

        if (stamina <= maxStamina) 
        {
            if (recoverTime <= 0) 
            {
                stamina++;
                recoverTime = 0.5f;
            }
        }


        if (stamina >= maxStamina) 
        {
            stamina = maxStamina;
            canRun = true;
        }
    }

    public int GetPlayerHealth() 
    {
        return health;
    }

    public int GetPlayerStamina()
    {
        return stamina;
    }
}
