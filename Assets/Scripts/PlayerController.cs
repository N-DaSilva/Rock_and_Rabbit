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

    [SerializeField] private CinemachineCamera m_PlayerCamera;

    private SceneController sceneController;

    
    IEnumerator WaitAndSwitchScene(string action)
    {
        yield return new WaitForSeconds(5);
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
        isInNextLevelZone = false;
        jump = new Vector3(0.0f, 2.0f, 0.0f);
        sceneController = GameObject.FindWithTag("SceneHandler").GetComponent<SceneController>();
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
            freeze = true;
            m_PlayerCamera.Priority = 11;
            StartCoroutine(WaitAndSwitchScene("win"));
        }
    }

    private void Die()
    {
        freeze = true;
        StartCoroutine(WaitAndSwitchScene("lose"));
    }

    public void Move()
    {
        Vector2 moveValue = moveAction.ReadValue<Vector2>();
        float horizontalMovement = moveValue.x * speed * Time.deltaTime;
        rb.linearVelocity = new Vector3(horizontalMovement, rb.linearVelocity.y, rb.linearVelocity.z);
    }

    public void Jump()
    {
        rb.AddForce(jump * jumpForce, ForceMode.Impulse);
        isGrounded = false;
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
            Move();
        }
    }

    void Update()
    {
        if(jumpAction.WasPressedThisFrame() && isGrounded && !freeze)
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
