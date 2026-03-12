using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField] private CinemachineCamera m_Camera1;
    [SerializeField] private CinemachineCamera m_Camera2;
    [SerializeField] private CinemachineCamera m_Camera3;
    [SerializeField] private CinemachineCamera m_Camera4;

    private InputAction m_Camera1Action;
    private InputAction m_Camera2Action;
    private InputAction m_Camera3Action;
    private InputAction m_Camera4Action;

    private const int MIN_CAMERA_PRIORITY = 0;
    private const int MAX_CAMERA_PRIORITY = 10;

    private void Awake()
    {
        m_Camera1.Priority = MAX_CAMERA_PRIORITY;
        m_Camera2.Priority = MIN_CAMERA_PRIORITY;
        m_Camera3.Priority = MIN_CAMERA_PRIORITY;
        m_Camera4.Priority = MIN_CAMERA_PRIORITY;
    }

    private void Start()
    {
        m_Camera1Action = InputSystem.actions.FindAction("Camera1");
        m_Camera2Action = InputSystem.actions.FindAction("Camera2");
        m_Camera3Action = InputSystem.actions.FindAction("Camera3");
        m_Camera4Action = InputSystem.actions.FindAction("Camera4");
    }

    public void DisplayCamera1()
    {
        m_Camera1.Priority = MAX_CAMERA_PRIORITY;
        m_Camera2.Priority = MIN_CAMERA_PRIORITY;
        m_Camera3.Priority = MIN_CAMERA_PRIORITY;
        m_Camera4.Priority = MIN_CAMERA_PRIORITY;
    }

    public void DisplayCamera2()
    {
        m_Camera1.Priority = MIN_CAMERA_PRIORITY;
        m_Camera2.Priority = MAX_CAMERA_PRIORITY;
        m_Camera3.Priority = MIN_CAMERA_PRIORITY;
        m_Camera4.Priority = MIN_CAMERA_PRIORITY;
    }

    public void DisplayCamera3()
    {
        m_Camera1.Priority = MIN_CAMERA_PRIORITY;
        m_Camera2.Priority = MIN_CAMERA_PRIORITY;
        m_Camera3.Priority = MAX_CAMERA_PRIORITY;
        m_Camera4.Priority = MIN_CAMERA_PRIORITY;
    }

    public void DisplayCamera4()
    {
        m_Camera1.Priority = MIN_CAMERA_PRIORITY;
        m_Camera2.Priority = MIN_CAMERA_PRIORITY;
        m_Camera3.Priority = MIN_CAMERA_PRIORITY;
        m_Camera4.Priority = MAX_CAMERA_PRIORITY;
    }


    void Update()
    {
        if (m_Camera1Action.WasPerformedThisFrame())
        {
            DisplayCamera1();
        }
        else if (m_Camera2Action.WasPerformedThisFrame())
        {
            DisplayCamera2();
        }
        else if (m_Camera3Action.WasPerformedThisFrame())
        {
            DisplayCamera3();
        }
        else if (m_Camera4Action.WasPerformedThisFrame())
        {
            DisplayCamera4();
        }
    }
}
