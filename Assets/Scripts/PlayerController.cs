using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    Vector2 mov_val;
    int speed = 500;
    private Vector2 _rotation;
    private CharacterController characterController;
    private float _sensitivity = 3f;
    public GameObject bullet;
    public static Vector3 playerPosition;
    public Camera gameOverCam;
    public static int resource;
    public GameObject gunTower;
    public GameObject buildBar;
    GameObject buildBarIns = null;
    GunTower buildingGunTower = null;

    // -----------New-------------
    Keyboard keyboard;

    public GameObject bulletSpawnPoint;
    //public static int enemyKills;
    // For Jump
    [SerializeField] float jumpHeight = 5.0f;
    [SerializeField] LayerMask groundMask;
    [SerializeField] float gravity = -9.81f;
    [SerializeField] float checkRadius = 0.5f;
    [SerializeField] Transform groundCheck;
    Vector3 downForce;
    bool jump;
    bool isGrounded;

    // For Roll or Evade
    float doublePressTime;
    float requiredTime = 0.5f;
    bool doublePressed = false;
    float rollSpeed = 1500f;

    // For win - prototype (temp)
    [SerializeField] bool collectedAllParts = false;
    [SerializeField] int n = 0;
    [SerializeField] bool playerInBase;
    [SerializeField] GameObject winText;
    [SerializeField] GameObject loseText;

    // Start is called before the first frame update
    void Start()
    {
        // win lose
        winText.SetActive(false);
        loseText.SetActive(false);
        ScoreText.setPartsText(n);

        playerInBase = true;

        // New
        keyboard = InputSystem.GetDevice<Keyboard>();

        characterController = GetComponent<CharacterController>();
        buildBarIns = Instantiate(buildBar);
        buildBarIns.transform.SetParent(null, false);
        buildBarIns.SetActive(false);
    }

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
        var buildAction = GetComponent<PlayerInput>().actions["Build"];
            buildAction.performed += content =>
            {
                var ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit) && hit.distance < 10.0f)
                {
                    if (hit.collider.gameObject.CompareTag("GunTower"))
                    {
                        buildingGunTower = hit.collider.gameObject.GetComponent<GunTower>();
                        if (buildingGunTower.isFinished())
                        {
                            buildingGunTower = null;
                        }
                        return;
                    }
                    if (hit.collider.gameObject.CompareTag("Ground") && resource >= 300)
                    {
                        var gts = GameObject.FindGameObjectsWithTag("GunTower");
                        foreach (var gt in gts){
                            print("Mag: " + (gt.transform.position - hit.point).magnitude.ToString());
                            if ((gt.transform.position - hit.point).magnitude < 2)
                            {
                                return;
                            }
                        }
                        resource -= 300;
                        var raycastPos = hit.point;
                        var obj = Instantiate(gunTower, hit.point + new Vector3(0f, 1f, 0f), Quaternion.Euler(0, 0, 0));
                        buildingGunTower = obj.GetComponent<GunTower>();
                        return;
                    }
                }
            };
            buildAction.canceled += content =>
            {
                buildingGunTower = null;
            };
    }

    private void FixedUpdate()
    {
        Vector3 moveDirection = transform.forward * mov_val.y + transform.right * mov_val.x;
        characterController.SimpleMove(moveDirection * speed * Time.fixedDeltaTime);
        Roll(moveDirection);
        playerPosition = transform.position;
        build();
    }

    // Update is called once per frame
    void Update()
    {
        // for win condition
        if (n >= 5) 
        {
            collectedAllParts = true;
        }

        if (collectedAllParts && playerInBase) 
        {
            winText.SetActive(true);
            gameOverCam.gameObject.SetActive(true);
        }

        // Player health
        ScoreText.setPlayerHealthText(GetComponent<PlayerHealth>().GetPlayerHealth());

        // New - Jump and Run
        isGrounded = Physics.CheckSphere(groundCheck.position, checkRadius, groundMask);

        if (isGrounded && downForce.y < 0)
            downForce.y = -2;

        downForce.y += gravity * Time.deltaTime;

        characterController.Move(downForce * Time.deltaTime); // adds constant down force with gravity value

        if (keyboard.spaceKey.wasPressedThisFrame && isGrounded) 
        {
            downForce.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
        }

        if (keyboard.leftShiftKey.IsPressed())
        {
            speed = 1000;
        }
        else 
        {
            speed = 500;
        }

        transform.rotation = Quaternion.Euler(_rotation);
        ScoreText.setText(resource);
        //ScoreText.setEnemyText(enemyKills);
    }

    void Roll(Vector3 dir) 
    {
        if (keyboard.aKey.wasPressedThisFrame) 
        {
            if (Time.time - doublePressTime < requiredTime) 
            {
                doublePressed = true;
                doublePressTime = 0f;

                if (doublePressed) 
                {
                    characterController.SimpleMove(dir * 10000f * Time.deltaTime);
                    Debug.Log(doublePressed);
                    doublePressed = false;
                }

                doublePressTime = Time.time;
            }
        }
    }

    private void OnMove(InputValue value)
    {
        mov_val = value.Get<Vector2>();
    }
    private void OnLook(InputValue value)
    {
        Vector2 lookValue = value.Get<Vector2>();
        _rotation.x += -lookValue.y * 0.03f * _sensitivity;
        if (_rotation.x > 90)
        {
            _rotation.x = 90;
        }
        if (_rotation.x < -90)
        {
            _rotation.x = -90;
        }
        _rotation.y += lookValue.x * 0.03f * _sensitivity;
    }
    private void OnFire(InputValue value)
    {
        float isFire = value.Get<float>();
        if (isFire > 0f)
        {
            Instantiate(bullet, bulletSpawnPoint.transform.position + bulletSpawnPoint.transform.forward * 2, transform.rotation);
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (gameOverCam != null) 
        {
            loseText.SetActive(true);
            gameOverCam.gameObject.SetActive(true);
        }
    }

    private void build()
    {
        if (buildingGunTower != null && resource > 0 && !buildingGunTower.isFinished())
        {
            resource -= 1;
            buildingGunTower.build(0.2f * Time.fixedDeltaTime);
            buildBarIns.transform.position = transform.position * 0.25f + buildingGunTower.transform.position * 0.75f;
            buildBarIns.transform.LookAt(buildingGunTower.transform.position);
            buildBarIns.GetComponent<BuildProcessBar>().setValue(buildingGunTower.buildingProgress);
            buildBarIns.SetActive(true);
        }
        else
        {
            buildBarIns.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("ShipPart")) 
        {
            n++;
            ScoreText.setPartsText(n);
            Destroy(other.gameObject);
        }

        if (other.gameObject.CompareTag("BaseDoor")) 
        {
            playerInBase = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("BaseDoor")) 
        {
            playerInBase = false;
        }
    }
}
