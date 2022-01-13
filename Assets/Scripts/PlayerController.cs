using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    Vector2 mov_val;
    Vector2 mov_val_bkp;
    int speed;
    public int walkSpeed = 500;
    public int runSpeed = 1500;
    private Vector2 _rotation;
    private CharacterController characterController;
    public static float sensitivity = 2f;
    public GameObject bullet;
    public static Vector3 playerPosition;
    public Camera gameOverCam;
    public static int resource;
    public GameObject gunTurret;
    public GameObject buildBar;
    public GameObject menuPanel;
    public static bool gamePaused = false;
    GameObject buildBarIns = null;
    public GunTurret buildingGunTurret = null;
    public bool firePressed = false;
    public bool helpPressed = false;
    public bool rushPressed = false;
    private int fireCooldown = 0; 
    [SerializeField] private int fireCooldownMax = 15;

    // Keyboard device
    Keyboard keyboard;
    
    // Spawn point for bullet
    public GameObject bulletSpawnPoint;

    //public static int enemyKills;
    // For Jump
    [SerializeField] float jumpHeight = 5.0f;
    [SerializeField] LayerMask groundMask;
    [SerializeField] float gravity = -9.81f;
    [SerializeField] float checkRadius = 0.5f;
    [SerializeField] Transform groundCheck;
    Vector3 downForce;
    bool isGrounded;

    // For Roll or Evade
    float doublePressTime;
    float requiredTime = 0.5f;
    bool doublePressed = false;
    //float rollSpeed = 1500f;

    // For win - prototype (temp)
    [SerializeField] bool collectedAllParts = false;
    [SerializeField] int n = 0;
    [SerializeField] bool playerInBase;
    [SerializeField] GameObject winText;
    [SerializeField] GameObject loseText;

    PlayerHealth playerHealth;
    ScoreText scoreText;

    // AMMO
    public int ammo;
    public int maxAmmo;

    // Sounds
    SoundManager soundManager;

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
        soundManager = FindObjectOfType<SoundManager>();
        playerHealth = GetComponent<PlayerHealth>();
        scoreText = FindObjectOfType<ScoreText>();
        buildBarIns = Instantiate(buildBar);
        buildBarIns.transform.SetParent(null, false);
        buildBarIns.SetActive(false);

        // AMMO
        ammo = maxAmmo;
    }

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void FixedUpdate()
    {
        if (gamePaused)
        {
            return;
        }
        Vector3 moveDirection = transform.forward * mov_val.y + transform.right * mov_val.x;
        characterController.SimpleMove(moveDirection * speed * Time.fixedDeltaTime);
        //Roll(moveDirection);
        playerPosition = transform.position;
        build();

        if (firePressed && fireCooldown <= 0 && ammo > 0)
        {
            StartCoroutine(nameof(ThreeShoots));
            fireCooldown = fireCooldownMax;
        }
        fireCooldown -= 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (gamePaused)
        {
            return;
        }

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
        ScoreText.setStaminaText(GetComponent<PlayerHealth>().GetPlayerStamina());
        ScoreText.setAmmoText(ammo);

        if (helpPressed)
        {
            scoreText.helpText.gameObject.SetActive(true);
            Time.timeScale = 0;
            return;
        }
        else 
        {
            scoreText.helpText.gameObject.SetActive(false);
            Time.timeScale = 1;
        }

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

        if (rushPressed && playerHealth.GetPlayerStamina() > 0 && playerHealth.canRun)
        {
            speed = runSpeed;
            if (mov_val.magnitude > 0)
                playerHealth.ReduceStamina();
            else 
            {
                if (mov_val.magnitude == 0)
                    playerHealth.RecoverStamina();
            }
        }

        else
        {
            speed = walkSpeed;
            playerHealth.RecoverStamina();
        }

        transform.rotation = Quaternion.Euler(_rotation);
        ScoreText.setText(resource);
        //ScoreText.setEnemyText(enemyKills);

        // AMMO
        if (ammo < 0)
        {
            ammo = 0;
        }

        if (ammo > maxAmmo) 
        {
            ammo = maxAmmo;
        }

        // if (keyboard.rKey.isPressed && (ammo - maxAmmo) <= 0) 
        // {
        //     if (resource > 100)
        //     {
        //         ammo++;
        //         resource -= 100;
        //     }
        // }
    }

    public void purchaseAmmo() {
        if (ammo < maxAmmo && resource >= 1000) 
        {
            ammo+=30;
            resource -= 1000;
        }
    }

    // Unfinished
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

    public void setMovVal(Vector2 inVal){
        mov_val = inVal;
    }

    public void setLookVal(Vector2 lookValue){
        _rotation.x += -lookValue.y * 0.03f * sensitivity;
        if (_rotation.x > 90)
        {
            _rotation.x = 90;
        }
        if (_rotation.x < -90)
        {
            _rotation.x = -90;
        }
        _rotation.y += lookValue.x * 0.03f * sensitivity;
    }


    private void BulletGenerate(bool firstShoot=false)
    {
        GameObject newBullet = Instantiate(bullet, bulletSpawnPoint.transform.position + bulletSpawnPoint.transform.forward * 2, transform.rotation);
        soundManager.PlaySound("player_shoot");
        if (rushPressed) { // make bullet faster while running
            newBullet.GetComponent<Rigidbody>().AddForce(newBullet.transform.forward * 30, ForceMode.Impulse);
        }
        if (!firstShoot)
        {
            _rotation += new Vector2(Random.Range(-0.5f, -0.3f), Random.Range(-0.5f, 0.3f));
        }
    }

    private IEnumerator ThreeShoots()
    {
        bool firstShoot = false;
        if (fireCooldown < -10)
        {
            firstShoot = true;
        }
        for (int i = 0; i < 3; i++)
        {
            BulletGenerate(firstShoot);
            ammo--;
            yield return new WaitForSeconds(0.1f);
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

    public void tryBuild(){
        var ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit) && hit.distance < 10.0f)
        {
            if (hit.collider.gameObject.CompareTag("GunTurret"))
            {
                buildingGunTurret = hit.collider.gameObject.GetComponent<GunTurret>();
                if (buildingGunTurret.isFinished())
                {
                    buildingGunTurret = null;
                }
                return;
            }
            if (hit.collider.gameObject.CompareTag("Ground") && resource >= 300)
            {
                var gts = GameObject.FindGameObjectsWithTag("GunTurret");
                foreach (var gt in gts){
                    print("Mag: " + (gt.transform.position - hit.point).magnitude.ToString());
                    if ((gt.transform.position - hit.point).magnitude < 2)
                    {
                        return;
                    }
                }
                resource -= 300;
                var obj = Instantiate(gunTurret, hit.point + new Vector3(0f, 1f, 0f), Quaternion.Euler(0, 0, 0));
                print(obj);
                print(obj.transform.position);
                buildingGunTurret = obj.GetComponent<GunTurret>();
                return;
            }
        }
    }

    private void build()
    {
        if (buildingGunTurret != null && resource > 0 && !buildingGunTurret.isFinished())
        {
            resource -= 1;
            buildingGunTurret.build(0.6f * Time.fixedDeltaTime);
            buildBarIns.transform.position = transform.position * 0.25f + buildingGunTurret.transform.position * 0.75f;
            buildBarIns.transform.LookAt(buildingGunTurret.transform.position);
            buildBarIns.GetComponent<BuildProcessBar>().setValue(buildingGunTurret.buildingProgress);
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
