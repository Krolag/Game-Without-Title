using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Variables
    private float moveForward;

    private float moveSide;

    private float moveUp;

    float speed = 5f;
    float jumpSpeed = 5f;
    
    private Rigidbody rig;

    private bool isGrounded;

    CapsuleCollider collider;

    float originalHeight;

    public float reducedHeight;
    public float slideSpeed = 10f;
    public bool isSliding = false;

    private bool isSprinting = false;
    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody>();
        collider = GetComponent<CapsuleCollider>();
        originalHeight = collider.height;
    }

    // Update is called once per frame
    void Update()
    {
        //Sprinting
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            isSprinting = true;
            speed *= 2;
        }
        
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isSprinting = false;
            speed /= 2;
        }

        moveForward = Input.GetAxis("Vertical") * speed;
        moveSide = Input.GetAxis("Horizontal") * speed;
        moveUp = Input.GetAxis("Jump") * jumpSpeed;
        
        //Moving the character forward and on side;
        rig.velocity = (transform.forward * moveForward) + (transform.right * moveSide) + (transform.up * rig.velocity.y);
        
        //Jumping
        if (isGrounded && moveUp != 0)
        {
            rig.AddForce(transform.up * moveUp, ForceMode.VelocityChange);
            isGrounded = false;
        }
        
        //Sliding
        
        if (Input.GetKeyDown(KeyCode.Q))
                Slide();
        else if (Input.GetKeyUp(KeyCode.Q))
                GoUp();
        

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
    
    //Sliding
    private void Slide()
    {
        collider.height = reducedHeight;
        rig.AddForce(transform.forward * slideSpeed, ForceMode.VelocityChange);
        isSprinting = false;
    }
    
    //Crouching
    void Crouch()
    {
        collider.height = reducedHeight;
    }
    
    private void GoUp()
    {
        collider.height = originalHeight;
    }
}
