using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    // UI
    public GameObject HUD;
    public GameObject mainMenu;
    public GameObject UICAM;
    public GameObject gameOverCam;
    public GameObject loseText;

    // PLayer
    public GameObject player;

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

        Time.timeScale = 0;
        UICAM.SetActive(true);
    }

    // Start is called before the first frame update
    void Start()
    {
        //LoadLevel("Main");
        mainMenu.SetActive(true);
        player.SetActive(false);
        HUD.SetActive(false);
        SoundManager.instance.PlaySound("bg_menu");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LoadLevel(string levelName)
    {
        Debug.Log("Loading " + levelName);
        Time.timeScale = 1;
        player.SetActive(true);
        mainMenu.SetActive(false);
        HUD.SetActive(true);
        UICAM.SetActive(false);
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
