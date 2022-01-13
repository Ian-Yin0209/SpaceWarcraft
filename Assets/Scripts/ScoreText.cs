using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ScoreText : MonoBehaviour
{
    public Text scoreText;
    static Text scoreTextIns;

    public Text playerHealth;
    static Text playerHealthIns;

    public Text partsText;
    static Text partsTextIns;

    public Text staminaText;
    static Text staminaTextIns;

    public Text ammoText;
    static Text ammoTextIns;

    public GameObject helpText;

    private void Awake()
    {
        scoreTextIns = scoreText;
        playerHealthIns = playerHealth;
        partsTextIns = partsText;
        staminaTextIns = staminaText;
        ammoTextIns = ammoText;
    }
    public static void setText(int score)
    {
        scoreTextIns.text = "Resource: " + score + "\nHold H key for instructions";
    }

    public static void setPlayerHealthText(int num) 
    {
        playerHealthIns.text = "Health: " + num;
    }

    public static void setPartsText(int num)
    {
        partsTextIns.text = "Ship parts: " + num;
    }

    public static void setStaminaText(int num)
    {
        staminaTextIns.text = "Stamina: " + num;
    }

    public static void setAmmoText(int num)
    {
        ammoTextIns.text = "Ammo: " + num;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
