using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement2 : MonoBehaviour, InputActionsMap.IPlayerActions
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
    public static bool isSprinting = false;
    public static bool isCrouching = false;

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


    private float x = 0f, z = 0f;

    // TO DO:
    // Zmienic metode Start na Awake, poczytaj czym sie roznia i co w ktorej lepiej robic
    void Start()
    {
        InputManager.Instance.SetCallbacks(this);

        Speed = WalkSpeed; // Inicjacja predkosci poczatkowej
        OriginalHeight = controller.height; // Inicjacja wysokosci poczatkowej

    }

    private void Update()
    {
        // Zbierz obecna pozycje gracza
        currentPosition = this.transform.position;

        velocity.y += Gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);

        if (!IsPlayerInMove && !isCrouching)
        {
            isSprinting = IsPlayerInMove;
            Walk();
        }

        // Poruszanie sie w osiach XYZ
        Move();

        // Sprawdz, czy gracz sie poruszyl w tej iteracji
        CheckForMovement();

    }
    
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
            controller.height = ReducedHeight * 0.3f;
            if (Time.time > startTime + dashTime - 0.05f)
                controller.height = ReducedHeight;
            yield return null;
        }
    }

    public void OnInteraction(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            Ray ray = new Ray(Camera.main.gameObject.transform.position, Camera.main.gameObject.transform.forward);
            RaycastHit[] hits = Physics.RaycastAll(ray, 4f);

            foreach (var hit in hits)
            {
                Interactable interactable;
                if (hit.collider.TryGetComponent<Interactable>(out interactable))
                {
                    interactable.Interact(GetComponent<Player>());
                }
            }
        }
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        Vector2 movement = context.ReadValue<Vector2>();

        x = movement.x;
        z = movement.y;
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
        // Skradanie
        if (context.phase == InputActionPhase.Started)
        {
            isCrouching = !isCrouching;
            Crouch();
        }
    }

    public void OnThrow(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            GetComponent<Player>().Throw();
        }
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        // Sprint
        if (context.phase == InputActionPhase.Performed && isSprinting == false)
        {
            isSprinting = !isSprinting;
            Sprint();
        }
        //Chodzenie
        else if (context.phase == InputActionPhase.Performed && isSprinting == true)
        {
            isSprinting = !isSprinting;
            Walk();
        }
        else if (!IsPlayerInMove && !isCrouching)
        {
            isSprinting = IsPlayerInMove;
            Walk();
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        // Podskok
        if (context.phase == InputActionPhase.Started && isGrounded)
        {
            Jump();
        }
    }
}