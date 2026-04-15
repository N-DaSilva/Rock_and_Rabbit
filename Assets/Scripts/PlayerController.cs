using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;
using UnityEngine.UI;
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
    [SerializeField] private AudioClip win_sound;
    [SerializeField] private AudioClip lose_sound;
    [SerializeField] private AudioClip footstep_sound;
    [SerializeField] private float footstepInterval = 0.35f;
    [SerializeField] private AudioClip jump_sound;

    [SerializeField] private GameObject bgMusic;
    private AudioSource audioSource;
    private float nextFootstepTime;

    private Image panel;


    private SceneController sceneController;

    
    private IEnumerator WaitAndSwitchScene(string action)
    {
        yield return new WaitForSeconds(4);
        if (action == "win")
        {
            sceneController.LoadScene("LevelClearScreen");
        }
        if (action == "lose")
        {
            sceneController.LoadScene("GameOverScreen");
        }
    }

    private IEnumerator FadePanelIn(float duration)
    {
        yield return new WaitForSeconds(3);
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            panel.color = Color.Lerp(new Color(0, 0, 0, 0f), new Color(0, 0, 0, 1f), elapsedTime / duration);
            yield return null;
        }

        panel.color = new Color(0, 0, 0, 1f);
    }

    private IEnumerator FadePanelOut(float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            panel.color = Color.Lerp(new Color(0, 0, 0, 1f), new Color(0, 0, 0, 0f), elapsedTime / duration);
            yield return null;
        }

        panel.color = new Color(0, 0, 0, 0f);
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
        audioSource = bgMusic.GetComponent<AudioSource>();
        panel = GameObject.FindWithTag("Panel").GetComponent<Image>();

        StartCoroutine(FadePanelOut(0.2f));
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
            audioSource.volume = Mathf.Lerp(0.25f, 0f, Time.time);
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
            audioSource.volume = Mathf.Lerp(0.25f, 0f, Time.time);
            Win();
        }
    }

    private void Die()
    {
        freeze = true;
        rb.linearVelocity = new Vector3 (0,0,0);
        rb.rotation = Quaternion.Euler(0, -90, 0);

        m_PlayerCamera.Priority = 11;
        animator.SetBool("isMoving", false);
        AudioSource.PlayClipAtPoint(lose_sound, transform.position);
        animator.SetTrigger("Lose");

        StartCoroutine(FadePanelIn(1f));
        StartCoroutine(WaitAndSwitchScene("lose"));
    }

    private void Win()
    {
        freeze = true;
        rb.linearVelocity = new Vector3 (0,0,0);
        rb.rotation = Quaternion.Euler(0, -90, 0);

        m_PlayerCamera.Priority = 11;
        animator.SetBool("isMoving", false);
        AudioSource.PlayClipAtPoint(win_sound, transform.position);
        animator.SetTrigger("Win");

        StartCoroutine(FadePanelIn(1f));
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
        bool isMoving = Mathf.Abs(moveValue.x) > 0.01f;
        animator.SetBool("isMoving", isMoving);

        float horizontalMovement = moveValue.x * speed * Time.deltaTime;
        rb.linearVelocity = new Vector3(horizontalMovement, rb.linearVelocity.y, rb.linearVelocity.z);

        if (isMoving && isGrounded && Time.time >= nextFootstepTime)
        {
            AudioSource.PlayClipAtPoint(footstep_sound, new Vector3 (transform.position.x, transform.position.y, -10.5f));
            nextFootstepTime = Time.time + footstepInterval;
        }

        if (moveValue.x != 0)
        {
            float targetRotation = moveValue.x > 0 ? 180f : 0f;
            Quaternion target = Quaternion.Euler(0, targetRotation, 0);

            rb.rotation = Quaternion.Lerp(rb.rotation, target, 10f * Time.deltaTime);
        }
    }

    public void Jump()
    {
        AudioSource.PlayClipAtPoint(jump_sound, transform.position);
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
