using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5.0f; //meters per second, shows up in inspector and can be edited.(public)
    public float rotationSpeed = 280.0f;
    public float jumpForce = 10f;
    public float gravityScale = 5f;
    public static float globalGravity = -9.81f;

    [Header("Collision Checks")]
    // Ground checker radius
    [SerializeField] float groundCheckerRadius = 0.25f;
    // Ground checker
    [SerializeField] Transform groundChecker;
    // Ground layer
    [SerializeField] LayerMask groundLayer;

    // Bools for physics
    public bool isGrounded;

    float horizontal;
    float vertical;      //float variables that hold horizontal and vertical input

    // Cache these to save on performance
    Vector2 moveInput;
    Vector3 moveDirection, projCamForward;
    Quaternion rotationToCam, rotationToMoveDirection;
    new Camera camera;

    Rigidbody rb;

    #region Unity Base Methods

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        camera = Camera.main;
    }

    void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
    }

    void FixedUpdate()
    {   
        CheckPhysics(); // Check the physics status

        Vector3 gravity = globalGravity * gravityScale * Vector3.up;
        rb.AddForce(gravity, ForceMode.Acceleration);
        
        
    }

    private void Update()
    {
        moveDirection = Vector3.forward * vertical + Vector3.right * horizontal;  //player movement

        projCamForward = Vector3.ProjectOnPlane(camera.transform.forward, Vector3.up);
        rotationToCam = Quaternion.LookRotation(projCamForward, Vector3.up);

        moveDirection = rotationToCam * moveDirection;

        Quaternion rotationToMoveDirection = (moveDirection == Vector3.zero) ? Quaternion.identity : Quaternion.LookRotation(moveDirection, Vector3.up);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotationToMoveDirection, rotationSpeed * Time.deltaTime);

        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }

    void OnDrawGizmos()
    {
        // Draw gizmos to see the settings
        Gizmos.DrawSphere(groundChecker.position, groundCheckerRadius);
    }
    #endregion



    #region User Methods
    void CheckPhysics()
    {
        // Check if the player is touching the ground
        isGrounded = Physics.CheckSphere(groundChecker.position, groundCheckerRadius, groundLayer);
    }

    public void Jump(Vector3 dir)
    {
        // Zero out the Y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);

        // make the player jump
        rb.velocity += dir * jumpForce;
    }

    public void OnMovePerformed(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();   //when OnMovePerformed is called it will pull this function

        vertical = moveInput.y;
        horizontal = moveInput.x;
    }


    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded)
        {
            Jump(Vector3.up);
        }
    }
    #endregion

    public void onMoveInput(float horizontal, float vertical)
    {
        this.vertical = vertical;
        this.horizontal = horizontal;
        Debug.Log($"input {vertical}, {horizontal}");
    }

}
