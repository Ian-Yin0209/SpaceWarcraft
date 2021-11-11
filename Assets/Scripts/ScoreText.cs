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

    private void Awake()
    {
        scoreTextIns = scoreText;
        playerHealthIns = playerHealth;
        partsTextIns = partsText;
    }
    public static void setText(int score)
    {
        scoreTextIns.text = "Resource: " + score;
    }

    public static void setPlayerHealthText(int num) 
    {
        playerHealthIns.text = "Health: " + num;
    }

    public static void setPartsText(int num)
    {
        partsTextIns.text = "Parts: " + num;
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
