using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public GameObject menuPanel;
    public GameObject playerController;
    private PlayerController pc;
    // Start is called before the first frame update

    void Awake()
    {
        pc = playerController.GetComponent<PlayerController>();
    }

    void Start()
    {
        var buildAction = GetComponent<PlayerInput>().actions["Build"];
        buildAction.performed += content =>
        {
            pc.tryBuild();
        };
        buildAction.canceled += content =>
        {
            pc.buildingGunTurret = null;
        };
        var fireAction = GetComponent<PlayerInput>().actions["Fire"];
        fireAction.performed += content => { pc.firePressed = true; };
        fireAction.canceled += content => { pc.firePressed = false; };
        var menuAction = GetComponent<PlayerInput>().actions["Menu"];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMenu(InputValue value)
    {
        if (! PlayerController.gamePaused){
            PlayerController.gamePaused = true;
            menuPanel.SetActive(true);
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
        else
        {
            PlayerController.gamePaused = false;
            menuPanel.SetActive(false);
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    private void OnMove(InputValue value)
    {
        pc.setMovVal(value.Get<Vector2>());
    }

    private void OnLook(InputValue value)
    {
        pc.setLookVal(value.Get<Vector2>());
    }
}
