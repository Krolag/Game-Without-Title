using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement2 : MonoBehaviour
{
    public CharacterController controller;
    Vector3 velocity;
    
    //Zmienne okreslajace kolejno: aktualna predkosc gracza, sile grawitacji, wysokosc skoku
    public float Speed = 8f;
    public float Gravity = -9.81f;
    public float JumpHeight = 3f;
        
    //Zmienne sluzace do zbadaniac czy gracz jest w powietrzu czy na ziemmi
    public Transform GroundCheck;
    public float GroundDistance = 0.4f;
    public LayerMask GroundMask;
    bool isGrounded;

    //Zmienne sprawdzajace stan gracza
    private bool isSprinting = false;
    private bool isCrouching = false;
    
    //Zmienne opisujace predkosc w zaleznosci od stanu w jakim znajduje sie gracz
    public float SprintSpeed = 12f;
    public float WalkSpeed = 8f;
    public float CrouchSpeed = 5f;
    
    //Wysokosc gracza na stojaco / w przykucu
    public float OriginalHeight;
    public float ReducedHeight;
    
    void Start()
    {
        Speed = WalkSpeed; //Inicjacja predkosci poczatkowej
        OriginalHeight = controller.height; //Inicjacja wysokosci poczatkowej
    }
    
    void Update()
    {
        //Poruszanie sie w osiach XYZ
        Move();
        
        //Podskok
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump();
        }
        velocity.y += Gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
        
        //Sprint/chodzenie
        if (Input.GetKeyDown(KeyCode.LeftShift) && isSprinting == false) 
        { 
            isSprinting = !isSprinting;
            Sprint();
        }
        
        else if (Input.GetKeyDown(KeyCode.LeftShift) && isSprinting == true) 
        { 
            isSprinting = !isSprinting;
            Walk();
        }
        
        //Skradanie
        if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.C))
        {
            isCrouching = !isCrouching;
            Crouch();
        }
    }

    //===================Funkcje=====================//
    private void Move()
    {
        isGrounded = Physics.CheckSphere(GroundCheck.position, GroundDistance, GroundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -700f;
        }
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        
        Vector3 move = transform.right * x +transform.forward * z;

        controller.Move(move * Time.deltaTime * Speed); 
    }

    private void Sprint()
    {
        Speed = SprintSpeed;
        if (isCrouching)
        {
            isCrouching = !isCrouching;
            controller.height = OriginalHeight;
        }
    }

    private void Walk()
    {
        Speed = WalkSpeed;
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
            Speed = CrouchSpeed;
            if (isSprinting)
                isSprinting = !isSprinting;
        }
        else
        {
            controller.height = OriginalHeight;
            Speed = WalkSpeed;
        }
    }
    
}