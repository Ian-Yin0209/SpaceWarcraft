using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartRotator : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody>().AddTorque(new Vector3(0, 100, 0));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}