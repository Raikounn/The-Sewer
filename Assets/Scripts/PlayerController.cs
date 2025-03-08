using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneTemplate;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class PlayerController : MonoBehaviour
{
    [Header("Basic Movement")]
    [SerializeField] PhysicMaterial frictionator;
    [SerializeField] private Transform camTransform;
    private Vector3 velocity;
    private float movSpeed = 2.5f;
    private float jumpForce = 1f;
    private float rotSpeed = 5f;
    private float gravity = -9.81f;
    private bool isGrounded;

    [Header("Sprint")]
    [SerializeField] private float sprintSpeed;
    [SerializeField] private float stamina;
    private bool isRunning = false;
    private float maxStamina = 10f;

    [Header("Crouch")]
    [SerializeField] private bool underRoof = false;
    [SerializeField] private bool isCrouching = false;


    
    private KeyCode sprintKey = KeyCode.LeftShift;
    private CharacterController controller;

    private void Awake()
    {
        stamina = maxStamina;
        sprintSpeed = movSpeed;
    }

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        Running();
        Crouch();
        Jump();
    }

    // Basic Movement
    void Movement()
    {
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = 0f;
        }

        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        Vector3 forward = camTransform.forward;
        Vector3 right = camTransform.right;
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        Vector3 moveDir = (forward * moveZ + right * moveX).normalized;
        controller.Move(moveDir * movSpeed * Time.deltaTime);

        if (moveDir != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(moveDir, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotSpeed * Time.deltaTime);
        }
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void Jump()
    {
        if (isCrouching || underRoof)
        {
            return;
        }
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y += Mathf.Sqrt(jumpForce * -2f * gravity);
        }
        
    }

    // Sprint
    void Running()
    {
        if (isCrouching || underRoof)
        {
            return;
        }
        if (Input.GetKeyDown(sprintKey) && movSpeed == sprintSpeed)
        {
            if (!isRunning && stamina > 0)
            {
                isRunning = true;
                if (isRunning)
                {
                    StopAllCoroutines();
                    StartCoroutine(DecreaseStamina());
                }
            }
        }
        else if (Input.GetKeyUp(sprintKey) && movSpeed != sprintSpeed)
        {
            if (isRunning || stamina != maxStamina)
            {
                isRunning = false;
                if (!isRunning)
                {
                    StopAllCoroutines();
                    StartCoroutine(RegenStamina());
                }
            }
        }
    }

    // Crouching
    void Crouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl) && controller.height == 2f)
        {
            controller.height = 1f;
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl) && !underRoof && controller.height != 2f)
        {
            controller.height = 2f;
        }
        if (Input.GetKeyDown(KeyCode.LeftControl)) isCrouching = true;
        if (Input.GetKeyUp(KeyCode.LeftControl)) isCrouching = false;
    }

    // Stamina Regeneration
    IEnumerator RegenStamina()
    {
        float tempStamina;
        tempStamina = stamina;
        movSpeed = sprintSpeed;
        while (stamina < maxStamina)
        {
            yield return new WaitForSeconds(0.5f);
            stamina++;
        }
    }
    
    // Stamina Regeneration't
    IEnumerator DecreaseStamina()
    {
        float tempStamina;
        tempStamina = stamina;
        movSpeed += sprintSpeed;
        while (stamina > 0)
        {
            yield return new WaitForSeconds(0.25f);
            stamina--;
        }
        isRunning = false;
        StartCoroutine(RegenStamina());
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            frictionator.dynamicFriction = 0f;
            frictionator.staticFriction = 0f;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            frictionator.dynamicFriction = 0.6f;
            frictionator.staticFriction = 0.6f;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Roof")
        {
            underRoof = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Roof")
        {
            underRoof = false;
            if (!isCrouching)
            {
                controller.height = 2f; 
            }
        }
    }
}
