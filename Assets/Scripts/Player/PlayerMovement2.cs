using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement2 : MonoBehaviour
{
    public CharacterController controller;
    private Vector3 velocity;
    
    // Zmienne okreslajace kolejno: aktualna predkosc gracza, sile grawitacji, wysokosc skoku
    public float Speed = 8f;
    public float Gravity = -9.81f;
    public float JumpHeight = 3f;
        
    // Zmienne sluzace do zbadania czy gracz jest w powietrzu czy na ziemi
    public Transform GroundCheck;
    public float GroundDistance = 0.4f;
    public LayerMask GroundMask;
    private bool isGrounded;

    // Zmienne sprawdzajace stan gracza
    private bool isSprinting = false;
    private bool isCrouching = false;
    
    // Zmienne opisujace predkosc w zaleznosci od stanu w jakim znajduje sie gracz
    public float SprintSpeed = 12f;
    public float WalkSpeed = 8f;
    public float CrouchSpeed = 5f;
    
    // Wysokosc gracza na stojaco / w przykucu
    public float OriginalHeight;
    public float ReducedHeight;
    
    // Zmienna zapisujaca ruch gracza
    public static bool IsPlayerInMove = false;
    private Vector3 currentPosition;
    
    //Zmienne do wslizgu
    public float dashSpeed;
    public float dashTime;
    Vector3 move = Vector3.zero;    

    
    
    // TO DO:
    // Zmienic metode Start na Awake, poczytaj czym sie roznia i co w ktorej lepiej robic
    void Start()
    {
        Speed = WalkSpeed; // Inicjacja predkosci poczatkowej
        OriginalHeight = controller.height; // Inicjacja wysokosci poczatkowej
        
    }
    
    private void Update()
    {
        // Zbierz obecna pozycje gracza
        currentPosition = this.transform.position;
        
        // Podskok
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump();
        }
        velocity.y += Gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
        
        // Sprint
        if (Input.GetKeyDown(KeyCode.LeftShift) && isSprinting == false) 
        { 
            isSprinting = !isSprinting;
            Sprint();
        }
        
        //Chodzenie
        else if (Input.GetKeyDown(KeyCode.LeftShift) && isSprinting == true) 
        { 
            isSprinting = !isSprinting;
            Walk();
        }
        
        else if (!IsPlayerInMove && !isCrouching)
        {
            isSprinting = IsPlayerInMove;
            Walk();
        }

        // Skradanie
        if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.C))
        {
            isCrouching = !isCrouching;
            Crouch();
        }
        // Poruszanie sie w osiach XYZ
        Move();
        
        // Sprawdz, czy gracz sie poruszyl w tej iteracji
        CheckForMovement();
    }

    // TO DO:
    // Nie pisz pustych komentarzy jak ten w 88 linii
    //===================Funkcje=====================//
    private void Move()
    {
        isGrounded = Physics.CheckSphere(GroundCheck.position, GroundDistance, GroundMask);

        if (isGrounded && velocity.y < 0 && controller.height == OriginalHeight)
        {
            velocity.y = -2f;
        }
        else if (isGrounded && controller.height == ReducedHeight)
        {
            velocity.y = -20f;
        }
        
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        
        move = transform.right * x + transform.forward * z;

        controller.Move(Time.deltaTime * Speed * move);
    }

    private void CheckForMovement()
    {
        if (this.transform.position != currentPosition)
        {
            IsPlayerInMove = true;
        }
        else
        {
            IsPlayerInMove = false;
        }
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
        if (isCrouching)
        {
            isCrouching = !isCrouching;
            controller.height = OriginalHeight;
            
            // Gdy kucamy mamy mozliwosc wyzszego skoku
            JumpHeight *= 1.3f;
            velocity.y = Mathf.Sqrt(JumpHeight * -2f * Gravity);
            JumpHeight /= 1.3f;
            Speed = WalkSpeed;
        }
        else if (isSprinting)
        {
            // Jak biegamy to tez xD
            JumpHeight *= 1.3f;
            velocity.y = Mathf.Sqrt(JumpHeight * -2f * Gravity);
            JumpHeight /= 1.3f;
        }
        else
            velocity.y = Mathf.Sqrt(JumpHeight * -2f * Gravity);
    }
    
    private void Crouch()
    {
        if (isCrouching == true)
        {
            controller.height = ReducedHeight;
            Speed = CrouchSpeed;
            if (isSprinting)
            {
                isSprinting = !isSprinting;
                StartCoroutine(Dash());
            }
        }
        else
        {
            controller.height = OriginalHeight;
            Speed = WalkSpeed;
        }
    }

    IEnumerator Dash()
    {
        float startTime = Time.time;
        while (Time.time < startTime + dashTime)
        {
            controller.Move(transform.forward * dashSpeed * Time.deltaTime);
            
            yield return null;
        }
    }
}