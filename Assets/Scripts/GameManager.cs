using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Text zoneText;

    // Zones
    private string currentZone;
    private Vector3 playerStart;
    public bool zone1Complete;
    public bool zone2Complete;
    public bool zone3Complete;
    public bool zone4Complete;
    public bool inZone1;
    public bool inZone2;
    public bool inZone3;
    public bool inZone4;
    public bool inBase;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        playerStart = transform.position;

        // Zones
        inZone1 = false;
        inZone2 = false;
        inZone3 = false;
        inZone4 = false;
        inBase = true;
        zone1Complete = false;
        zone2Complete = false;
        zone3Complete = false;
        zone4Complete = false;
    }

    // Update is called once per frame
    void Update()
    {
        zoneText.text = "In " + currentZone;

        if (inBase)
        {
            currentZone = "Base";
            SoundManager.instance.PlaySound("bg_base");
        }

        else if (inZone1)
        {
            currentZone = "Zone 1";
            inBase = false;
            SoundManager.instance.PlaySound("bg_area_1");
            SoundManager.instance.StopSound("bg_base");

        }

        else if (inZone2)
        {
            currentZone = "Zone 2";
            inZone1 = false;
            SoundManager.instance.PlaySound("bg_area_2");
            
        }

        else if (inZone3)
        {
            currentZone = "Zone 3";
            inZone2 = false;
            SoundManager.instance.PlaySound("bg_area_3");
            
        }

        else if (inZone4)
        {
            currentZone = "Zone 4";
            inZone3 = false;
            SoundManager.instance.PlaySound("bg_area_4");
            
        }

        else
        {
            currentZone = "Neutral Zone";
            inZone1 = false;
            inZone2 = false;
            inZone3 = false;
            inZone4 = false;
            inBase = false;
            SoundManager.instance.StopSound("bg_base");
            SoundManager.instance.StopSound("bg_area_1");
            SoundManager.instance.StopSound("bg_area_2");
            SoundManager.instance.StopSound("bg_area_3");
            SoundManager.instance.StopSound("bg_area_4");
        }
    }
}
