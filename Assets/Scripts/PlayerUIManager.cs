using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerUIManager : MonoBehaviour
{
    [SerializeField] public Camera playerCamera;
    [SerializeField] public Canvas canvas;
    public PlayerInput playerInput;
    public PlayerInputManager playerInputManager;
    public CashComponent playerCashComponent;

    [SerializeField] public DefaultPanel defaultPanel;
    [SerializeField] public CashGrabPanel cashGrabPanel;

    private Panel currentPanel;

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerInputManager = GetComponent<PlayerInputManager>();
        playerCashComponent = GetComponent<CashComponent>();

        currentPanel = defaultPanel;
        currentPanel.Enter(this);
    }

    public void SwitchPanel(Panel nextPanel)
    {
        currentPanel.Exit(this);
        currentPanel = nextPanel;
        nextPanel.Enter(this);
    }

    public void OnGrab(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            cashGrabPanel.StartGrab();

        }
        else if (context.canceled)
        {
            cashGrabPanel.EndGrab();
        }
    }

    public void OnEscape(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            SwitchPanel(defaultPanel);
            playerInputManager.EndCashGrab();
        }
    }

    private void Update()
    {
        if(currentPanel != null)
        {
            currentPanel.UpdatePanel(this);
        }
        
    }

    private void FixedUpdate()
    {
        if(currentPanel != null)
        {
            currentPanel.FixedUpdatePanel(this);
        }
        
    }
}
