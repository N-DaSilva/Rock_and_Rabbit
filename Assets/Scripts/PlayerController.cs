using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;
using System.Collections;

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

    private bool stop;
    private bool moveLeft;
    private bool moveRight;

    [SerializeField] private CinemachineCamera m_PlayerCamera;
    private Animator animator;

    private SceneController sceneController;

    
    IEnumerator WaitAndSwitchScene(string action)
    {
        yield return new WaitForSeconds(3);
        if (action == "win")
        {
            sceneController.LoadScene("LevelClearScreen");
        }
        if (action == "lose")
        {
            sceneController.LoadScene("GameOverScreen");
        }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        moveAction = InputSystem.actions.FindAction("Move");

        jumpAction = InputSystem.actions.FindAction("Jump");
        isGrounded = true;
        freeze = false;

        stop = true;
        moveLeft = false;
        moveRight = false;

        isInNextLevelZone = false;
        jump = new Vector3(0.0f, 2.0f, 0.0f);
        sceneController = GameObject.FindWithTag("SceneHandler").GetComponent<SceneController>();
        animator = GetComponent<Animator>();
        animator.SetBool("isMoving", false);
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
        if (other.gameObject.tag == "Lava")
        {
            Die();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "NextLevel")
        {
            isInNextLevelZone = false;
        }
        if (other.gameObject.tag == "Finishline")
        {
            Win();
        }
    }

    private void Die()
    {
        freeze = true;
        m_PlayerCamera.Priority = 11;
        animator.SetBool("isMoving", false);
        animator.SetTrigger("Lose");
        rb.rotation = Quaternion.Euler(0, -90, 0);
        StartCoroutine(WaitAndSwitchScene("lose"));
    }

    private void Win()
    {
        freeze = true;
        m_PlayerCamera.Priority = 11;
        animator.SetBool("isMoving", false);
        animator.SetTrigger("Win");
        rb.rotation = Quaternion.Euler(0, -90, 0);
        StartCoroutine(WaitAndSwitchScene("win"));
        
    }

    public void SwitchMoveRight()
    {
        moveRight = true;
        moveLeft = false;
        stop = false;
        animator.SetBool("isMoving", true);
    }

    public void SwitchMoveLeft()
    {
        moveLeft = true;
        moveRight = false;
        stop = false;
        animator.SetBool("isMoving", true);
    }

    public void SwitchStop()
    {
        stop = true;
        moveLeft = false;
        moveRight = false;

        animator.SetBool("isMoving", false);

        rb.linearVelocity = new Vector3 (0,0,0);
    }

    public void MoveFloat(float input)
    {
        Vector2 moveValue;
        moveValue = new Vector2 (input, 0);
        Move(moveValue);
    }

    public void Move(Vector2 moveValue)
    {
        animator.SetBool("isMoving", Mathf.Abs(moveValue.x) > 0.01f);

        float horizontalMovement = moveValue.x * speed * Time.deltaTime;
        rb.linearVelocity = new Vector3(horizontalMovement, rb.linearVelocity.y, rb.linearVelocity.z);

        if (moveValue.x != 0)
        {
            float targetRotation = moveValue.x > 0 ? 180f : 0f;
            Quaternion target = Quaternion.Euler(0, targetRotation, 0);

            rb.rotation = Quaternion.Lerp(rb.rotation, target, 10f * Time.deltaTime);
        }
    }

    public void Jump()
    {
        if(isInNextLevelZone)
        {
            JumpToNextLevel();
        } else
        {
            rb.AddForce(jump * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    public void JumpToNextLevel()
    {
        rb.AddForce(jump * 17.0f, ForceMode.Impulse);
        isGrounded = false;
    }

    void FixedUpdate()
    {
        if (!freeze)
        {
            float horizontalInput = moveAction.ReadValue<Vector2>().x;

            if (!stop)
            {
               if (moveLeft && !moveRight)
                {
                    horizontalInput = -1f;
                } else if (!moveLeft && moveRight)
                {
                    horizontalInput = 1f;
                } 
            }

            Move(new Vector2(horizontalInput, 0f));
        }
    }

    void Update()
    {
        if(jumpAction.WasPressedThisFrame() && isGrounded && !freeze)
        {
            Jump();
        }
    }
}
