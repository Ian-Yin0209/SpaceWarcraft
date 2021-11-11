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

    private void Awake()
    {
        scoreTextIns = scoreText;
        playerHealthIns = playerHealth;
    }
    public static void setText(int score)
    {
        scoreTextIns.text = "Resource: " + score;
    }

    public static void setPlayerHealthText(int num) 
    {
        playerHealthIns.text = "Health: " + num;
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
