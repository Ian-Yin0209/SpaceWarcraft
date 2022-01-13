using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public Slider sensitivitySlider; 
    public Slider volumeSlider;

    // Update is called once per frame
    void Update()
    {
        PlayerController.sensitivity = sensitivitySlider.value;
        AudioListener.volume = volumeSlider.value;
    }

    public void OnClickContinue()
    {
        PlayerController.gamePaused = false;
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        gameObject.SetActive(false);
    }
    public void OnClickExit()
    {
        Application.Quit();
    }

    public void OnClickRestart()
    {
        OnClickContinue();
        SceneManager.LoadScene("SampleScene");
    }
}
