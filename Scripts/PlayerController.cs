using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerController : MonoBehaviour
{
    public Transform camera1;
    public Rigidbody rb;

    public Slider healthBar;
    public Slider staminaBar;

    public float health = 100;
    public float stamina = 100;
    public Rigidbody arrow;

    public float camRotationSpeed = 5f;
    public float cameraMinimumY = -60f;
    public float cameraMaximumY = 75;
    public float rotationSmoothSpeed = 10f;

    private float walkSpeed = 6f;
    private float runSpeed = 11f;
    private float maxSpeed = 20f;
    private float jumpPower = 10f;
    private float speed;

    public float extraGravity = 45;

    private float staminaRegenRate = 1.5f;
    float bodyRotationX;
    float camRotationY;
    Vector3 directionIntentX;
    Vector3 directionIntentY;

    [Header("Footstep Parameters")]
    [SerializeField] private float baseStepSpeed = 0.5f;
    [SerializeField] private float crouchStepMultiplier = 1.5f;
    [SerializeField] private float springSpeedMultiplier = 0.6f;
    [SerializeField] private AudioSource audioSource = default;
    [SerializeField] private AudioClip[] footsteps = default;
    [SerializeField] private AudioClip jumpSound = default;

    private bool grounded;
    private bool running; 
    private Vector2 currentInput;

    private float footstepTimer = 0;
    private float GetCurrentOffset => running ? baseStepSpeed * springSpeedMultiplier : baseStepSpeed;

    private GameObject spawn;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    void Update()
    {
        LookRotation();
        Movement();
        ExtraGravity();
        GroundCheck();
        if (grounded && Input.GetButtonDown("Jump") && stamina >= 20)
        {
            Jump();
        }
        PlayerStats();
        ShootArrow();
        healthBar.value = health / 100;
        staminaBar.value = stamina / 100;

            HandleFootsteps();
    }
    void PlayerStats()
    {
        if (stamina >= 100)
        {
            stamina = 100;
        }
        else if (stamina < 0)
        {
            stamina = 0;
        }
        else if (grounded)
        {
            stamina += Time.deltaTime * staminaRegenRate;
        }
    }

    void LookRotation()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        //get camera and body rotatational values
        bodyRotationX += Input.GetAxis("Mouse X") * camRotationSpeed;
        camRotationY += Input.GetAxis("Mouse Y") * camRotationSpeed;

        //stop the camera from rotating 360 degrees
        camRotationY = Mathf.Clamp(camRotationY, cameraMinimumY, cameraMaximumY);

        //create rotation targets and handle rotations of the body and camera
        Quaternion camTargetRotation = Quaternion.Euler(-camRotationY, 0, 0);
        Quaternion bodyTargetRotation = Quaternion.Euler(0, bodyRotationX, 0);

        //handle rotations
        transform.rotation = Quaternion.Lerp(transform.rotation, bodyTargetRotation, Time.deltaTime * rotationSmoothSpeed);

        camera1.localRotation = Quaternion.Lerp(camera1.localRotation, camTargetRotation, Time.deltaTime * rotationSmoothSpeed);
    }
    void Movement()
    {
        //direction must match camera direction
        directionIntentX = camera1.right;
        directionIntentX.y = 0;
        directionIntentX.Normalize();

        directionIntentY = camera1.forward;
        directionIntentY.y = 0;
        directionIntentY.Normalize();

        //change charachter's direction in this direction
        rb.velocity = directionIntentY * Input.GetAxis("Vertical") * speed + directionIntentX * Input.GetAxis("Horizontal") * speed + Vector3.up * rb.velocity.y;
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);

        currentInput = new Vector2(speed * Input.GetAxis("Vertical"), speed * Input.GetAxis("Horizontal"));
        //control our speed based on our movement state
        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = runSpeed;
            running = true;
        }
        if (!Input.GetKey(KeyCode.LeftShift))
        {
            speed = walkSpeed;
            running = false;
        }

    }
    void ExtraGravity()
    {
        rb.AddForce(Vector3.down * extraGravity);
    }
    void GroundCheck()
    {
        RaycastHit groundHit;
        grounded = Physics.Raycast(transform.position, -transform.up, out groundHit, 1.25f);
    }
    void Jump()
    {
        rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
        stamina -= 20;
        audioSource.PlayOneShot(jumpSound);

    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            health -= 20;
        }
    }
    private void HandleFootsteps()
    {
        if (!grounded)
        {
            return;
        }
        if (currentInput == Vector2.zero)
        {
            return;
        }

        footstepTimer -= Time.deltaTime;

        if (footstepTimer <= 0)
        {
            audioSource.PlayOneShot(footsteps[Random.Range(0, footsteps.Length - 1)]);
            footstepTimer = GetCurrentOffset;
        }
    }
    public void ShootArrow()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Rigidbody spawn;
            spawn = Instantiate(arrow, transform.position, transform.rotation);
            spawn.velocity = transform.forward * 10;
        }
    }
}