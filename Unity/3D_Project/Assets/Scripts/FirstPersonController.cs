using UnityEngine;
using UnityEngine.UI;

public class FirstPersonController : MonoBehaviour
{
    public float speed = 5f;
    public float sprintSpeed = 10f;
    public float mouseSensitivity = 2f;
    public float verticalLookLimit = 80f;
    public float maxStamina = 100f;
    public float staminaRegenRate = 5f;
    public float sprintStaminaCost = 10f;
    public float gravity = 9.81f; // Gravity force
    public Image staminaBar; // Reference to the stamina bar UI element

    private CharacterController characterController;
    private float rotationX = 0f;
    private float currentStamina;
    private bool isSprinting = false;
    private Vector3 velocity; // Velocity for gravity

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor to the center of the screen
        currentStamina = maxStamina; // Initialize stamina
    }

    void Update()
    {
        // Mouse look
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -verticalLookLimit, verticalLookLimit);
        Camera.main.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.Rotate(0, mouseX, 0);

        // Movement
        float moveDirectionX = Input.GetAxis("Horizontal") * speed;
        float moveDirectionZ = Input.GetAxis("Vertical") * speed;

        // Sprinting
        if (Input.GetKey(KeyCode.LeftShift) && currentStamina > 0)
        {
            isSprinting = true;
            moveDirectionX *= sprintSpeed / speed;
            moveDirectionZ *= sprintSpeed / speed;
            currentStamina -= sprintStaminaCost * Time.deltaTime;
            if (currentStamina < 0)
            {
                currentStamina = 0;
            }
        }
        else
        {
            isSprinting = false;
            currentStamina += staminaRegenRate * Time.deltaTime;
            if (currentStamina > maxStamina)
            {
                currentStamina = maxStamina;
            }
        }

        Vector3 move = transform.right * moveDirectionX + transform.forward * moveDirectionZ;

        // Apply gravity
        if (characterController.isGrounded)
        {
            velocity.y = 0f; // Reset velocity when grounded
        }
        else
        {
            velocity.y -= gravity * Time.deltaTime; // Apply gravity
        }

        characterController.Move((move + velocity) * Time.deltaTime);

        // Update stamina bar
        if (staminaBar != null)
        {
            staminaBar.fillAmount = currentStamina / maxStamina;
        }

        // Debugging stamina
        //Debug.Log("Current Stamina: " + currentStamina);
    }
}