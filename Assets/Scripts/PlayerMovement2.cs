using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement2 : MonoBehaviour
{
    public CharacterController controller;
    public float Speed = 12f;
    public float Gravity = -9.81f;
    public float JumpHeight = 3f;
        
    public Transform GroundCheck;
    public float GroundDistance = 0.4f;
    public LayerMask GroundMask;
    
    Vector3 velocity;
    bool isGrounded;

    private bool isSprinting = false;
    public float SpeedMultiplier = 1.5f;
    
    public float OriginalHeight;
    public float ReducedHeight;
    private bool isCrouching = false;


    void Start()
    {
        OriginalHeight = controller.height;
    }

    // Update is called once per frame
    void Update()
    {
        //Moving and jumping
        Move();

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump();
        }
        velocity.y += Gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
        
        
        //Sprinting
        if (Input.GetKeyDown(KeyCode.LeftShift)) 
        { 
            isSprinting = !isSprinting;
            Sprint();
        }
        
        //Crouching
        if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.C))
        {
            isCrouching = !isCrouching;
            Crouch();
        }
    }

    private void Move()
    {
        isGrounded = Physics.CheckSphere(GroundCheck.position, GroundDistance, GroundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        
        Vector3 move = transform.right * x +transform.forward * z;

        controller.Move(move * Speed * Time.deltaTime); 
    }

    private void Sprint()
    {
        if (isSprinting == true)
        {
            Speed *= SpeedMultiplier;
        }
        else if (isSprinting == false)
        {
            Speed /= SpeedMultiplier;
        }
    }
    
    private void Jump()
    {
        velocity.y = Mathf.Sqrt(JumpHeight * -2f * Gravity);
    }
    
    private void Crouch()
    {
        if (isCrouching == true)
        {
            controller.height = ReducedHeight;
        }
        else 
        {
            controller.height = OriginalHeight;
        }
    }
    
}