using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float movSpeed = 2.5f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float rotSpeed = 5f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private Vector3 velocity;
    [SerializeField] private Transform camTransform;
    [SerializeField] PhysicMaterial frictionator;

    [SerializeField] private bool isGrounded;

    private CharacterController controller;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
        rb.drag = 5;
        rb.angularDrag = 5;
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

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

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y += Mathf.Sqrt(jumpForce * -2f * gravity);
        }
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            frictionator.dynamicFriction = 0f;
            frictionator.staticFriction = 0f;
        }

        /*if (collision.gameObject.tag == "Floor")
        {
            isGrounded = true;
        }*/
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            frictionator.dynamicFriction = 0.6f;
            frictionator.staticFriction = 0.6f;
        }

       /* if (collision.gameObject.tag == "Floor")
        {
            isGrounded = false;
        }*/
    }
}
