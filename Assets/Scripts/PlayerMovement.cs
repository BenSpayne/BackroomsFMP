using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 5f;
    public float sprintSpeed = 8f;
    public float moveSpeed;
    public CharacterController controller;

    [Header("Stamina Settings")]
    public float maxStamina = 7f;
    public float staminaRechargeSpeed = 2f;
    private float currentStamina;
    public bool isSprinting;
    public bool isCooldown;

    [Header("References")]
    public Transform playerCamera;
    private UIManager uiManager;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        uiManager = FindObjectOfType<UIManager>();
        moveSpeed = walkSpeed;
        currentStamina = maxStamina;
    }

    void Update()
    {
        HandleMovement();
        HandleSprint();
    }

    void HandleMovement()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        controller.Move(move * moveSpeed * Time.deltaTime);

        if (!controller.isGrounded)
        {
            controller.Move(Vector3.down * 5f * Time.deltaTime); 
        }
    }

    void HandleSprint()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        bool isMoving = moveX != 0 || moveZ != 0;

        if (Input.GetKey(KeyCode.LeftShift) && currentStamina > 0 && !isCooldown && isMoving)
        {
            isSprinting = true;
            moveSpeed = sprintSpeed;
            currentStamina -= Time.deltaTime;
            uiManager.UpdateSprintBar(currentStamina / maxStamina, isSprinting);
        }
        else
        {
            isSprinting = false;
            moveSpeed = walkSpeed;

            if (currentStamina < maxStamina && !isCooldown)
            {
                currentStamina += Time.deltaTime * staminaRechargeSpeed;
                uiManager.UpdateSprintBar(currentStamina / maxStamina, isSprinting);
            }
        }

        if (currentStamina <= 0 && !isCooldown)
        {
            isCooldown = true;
            uiManager.ShowCooldownBar();
            Invoke(nameof(ResetSprintCooldown), 5f);
        }
    }

    void ResetSprintCooldown()
    {
        isCooldown = false;
        uiManager.HideCooldownBar();
        currentStamina = maxStamina;
        uiManager.UpdateSprintBar(1f, isSprinting);
    }

    //------------------------
    public bool IsSprinting() => isSprinting;
    public bool IsGrounded() => controller.isGrounded;
    public Vector3 GetVelocity() => controller.velocity;
    //------------------------
}