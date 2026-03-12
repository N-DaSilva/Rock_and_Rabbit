using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 500.0f;
    private Rigidbody rb;

    InputAction moveAction;
    InputAction jumpAction;

    private bool isGrounded;
    private Vector3 jump;
    [SerializeField] public float jumpForce = 10.0f;

    private bool isInNextLevelZone;
    private bool freeze;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        isGrounded = true;
        freeze = false;
        isInNextLevelZone = false;
        jump = new Vector3(0.0f, 2.0f, 0.0f);
    }

    void OnCollisionStay()
    {
        isGrounded = true;
    }

    void OnCollisionExit()
    {
        isGrounded = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "NextLevel")
        {
            isInNextLevelZone = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "NextLevel")
        {
            isInNextLevelZone = false;
        }
    }

    public void Move()
    {
        Vector2 moveValue = moveAction.ReadValue<Vector2>();
        float horizontalMovement = moveValue.x * speed * Time.deltaTime;
        rb.linearVelocity = new Vector2(horizontalMovement, rb.linearVelocity.y);
    }

    public void Jump()
    {
        rb.AddForce(jump * jumpForce, ForceMode.Impulse);
        isGrounded = false;
    }

    public void JumpToNextLevel()
    {
        freeze = true;
        rb.AddForce(jump * 17.0f, ForceMode.Impulse);
        isGrounded = false;
        freeze = false;
    }

    void FixedUpdate()
    {
        if (!freeze)
        {
            Move();
        }
    }

    void Update()
    {
        if(jumpAction.WasPressedThisFrame() && isGrounded)
        {
            if(isInNextLevelZone)
            {
                JumpToNextLevel();
            } else
            {
                Jump();
            }
        }
    }
}
