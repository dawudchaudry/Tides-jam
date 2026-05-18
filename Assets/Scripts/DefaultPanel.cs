using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class DefaultPanel : Panel
{

    [SerializeField] private TextMeshProUGUI cashDisplay;

    RectTransform panelRectTransform;

    private void Awake()
    {
        panelRectTransform = GetComponent<RectTransform>();
    }

    override public void Enter(PlayerUIManager UIManager)
    {
        UIManager.playerInput.SwitchCurrentActionMap("Default");
        gameObject.SetActive(true);
        cashDisplay.text = "Cash: $" + UIManager.playerCashComponent.GetCashHeld().ToString();
    }

    override public void UpdatePanel(PlayerUIManager UIManager)
    {
        
    }

    override public void FixedUpdatePanel(PlayerUIManager UIManager)
    {

    }

    public override void Exit(PlayerUIManager UIManager)
    {
        gameObject.SetActive(false);
    }

}

