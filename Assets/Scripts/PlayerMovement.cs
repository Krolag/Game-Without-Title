using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
<<<<<<< Updated upstream
=======
    //Zmienne do sprawdzania czy gracz znajduje sie w powietrzu czy nie
    public Transform GroundCheck;
    public float GroundDistanace = 0.4f; //<- promien kuli pod graczem
    public LayerMask GroundMask;
    private Vector3 velocity;
    
    //Zmienne do sterowania graczem w osiach XYZ
>>>>>>> Stashed changes
    private float moveForward;
    private float moveSide;
    private float moveUp;
    
    //Zmienne okreslajace predkosc poruszania sie i predkosc skoku
    public float Speed = 5f;
    public float JumpSpeed = 5f;
    
    private Rigidbody rig;

    private bool isGrounded;

    CapsuleCollider collider;

    //Zmienne okreslajace wysokosc gracza w zaleznosci od tego w jakims tanie sie znajduje (idle/crouching/sliding)
    public float OriginalHeight;
    public float ReducedHeight;
    
    //Zmienne okreslajace predkosc wslizgu i to czy wslizg wystepuje
    public float SlideSpeed = 10f;
    public bool IsSliding = false;

    //Zmienne okreslajaca predkosc w trakcie sprintu i to czy sprint wystepuje
    public float SpeedMultiplier = 1.5f;
    private bool isSprinting = false;
    
    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody>();
        collider = GetComponent<CapsuleCollider>();
        OriginalHeight = collider.height;
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(GroundCheck.position, GroundDistanace, GroundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        //Sprinting
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            isSprinting = true;
            Speed *= SpeedMultiplier;
        }
        
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isSprinting = false;
            Speed /= SpeedMultiplier;
        }
        
        Movement();
        
        //Jumping
        if (isGrounded && Input.GetKey(KeyCode.Space))
            Jump();
        
        //Sliding
        if(isSprinting)
        {
            if (Input.GetKeyDown(KeyCode.Q))//LeftControl))
                Slide();
            else if (Input.GetKeyUp(KeyCode.Q))//LeftControl))
                GoUp();
        }
        
        //Crouching
        if(Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.C))
            Crouch();
        else if(Input.GetKeyUp(KeyCode.LeftControl) || Input.GetKeyUp(KeyCode.C))
            GoUp();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
        }
    }
    
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = false;
        }
    }
    
    //Movement
    private void Movement()
    {
        moveForward = Input.GetAxis("Vertical") * Speed;
        moveSide = Input.GetAxis("Horizontal") * Speed;

        //Moving the character forward and on side;
        rig.velocity = (transform.forward * moveForward) + (transform.right * moveSide) + (transform.up * rig.velocity.y); // <-- ten kawalek kodu powoduje ze sie podskakuje na schodach
    }

    //Jumping
    private void Jump()
    {
        moveUp = Input.GetAxis("Jump") * JumpSpeed;
        
        rig.AddForce(transform.up * moveUp, ForceMode.VelocityChange);
        isGrounded = false;
    }
    
    //Sliding
    private void Slide()
    {
        collider.height = ReducedHeight;
        rig.AddForce(transform.forward * SlideSpeed, ForceMode.Impulse);
        isSprinting = false;
    }
    
    //Crouching
    private void Crouch()
    {
        collider.height = ReducedHeight;
    }
    
    private void GoUp()
    {
        collider.height = OriginalHeight;
    }
}
