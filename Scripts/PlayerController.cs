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

    public float camRotationSpeed = 5f;
    public float cameraMinimumY = -60f;
    public float cameraMaximumY = 75;
    public float rotationSmoothSpeed = 10f;

    public float walkSpeed = 9f;
    public float runSpeed = 14f;
    public float maxSpeed = 20f;
    public float jumpPower = 30f;

    public float extraGravity = 45;

    public float staminaRegenRate = 1.5f;
    float bodyRotationX;
    float camRotationY;
    Vector3 directionIntentX;
    Vector3 directionIntentY;

    float speed;

    public bool grounded;

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
        healthBar.value = health/100;
        staminaBar.value = stamina/100;
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

        //control our speed based on our movement state
        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = runSpeed;
        }
        if (!Input.GetKey(KeyCode.RightShift))
        {
            speed = walkSpeed;
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
    }

    void OnCollisionEnter(Collision collision)
    {
        if (other.tag == "Enemy")
        {
          health -= 20;
        }
    }
}
