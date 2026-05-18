using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;
using UnityEngine.XR;

public class CashGrabPanel : Panel
{
    [SerializeField] private CashUIItem cashPrefab;

    [SerializeField] private RectTransform handTransform;
    [SerializeField] private RectTransform cashSpawnerTranform;
    [SerializeField] private RectTransform dropoffTransform;
    [SerializeField] private TextMeshProUGUI cashDisplay;
    RectTransform panelRectTransform;

    [SerializeField] float handAcceleration = 5f;

    CashUIItem currentCash;

    private void Awake()
    {
        panelRectTransform = GetComponent<RectTransform>();
    }

    public void StartGrab()
    {
        
        if (IsOverlapping(handTransform, cashSpawnerTranform) && UIManager.playerInputManager.currentCashPileCollider.GetComponent<CashComponent>().GetCashHeld() > 0) // holy shit what the fuck
        {
            currentCash = Instantiate(cashPrefab, handTransform);
        }

    }

    public void EndGrab()
    {
        if (currentCash != null)
        {
            if (IsOverlapping(currentCash.transform.GetComponent<RectTransform>(), dropoffTransform))
            {
                CashComponent pile = UIManager.playerInputManager.currentCashPileCollider.GetComponent<CashComponent>(); // i think im going to be sick
                pile.TransferTo(UIManager.playerCashComponent, 100);
                cashDisplay.text = "Cash: $" + UIManager.playerCashComponent.GetCashHeld().ToString();
            }
            else
            {
                Debug.Log("cash dropped");
            }
            Destroy(currentCash.gameObject);
            currentCash = null;
        }
    }

    override public void UpdatePanel(PlayerUIManager UIManager)
    {
        // hand movement
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Vector2 mouseLocalPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(panelRectTransform, mousePosition, null, out mouseLocalPosition);
        if ( Mathf.Abs(Vector2.Distance(handTransform.localPosition, mousePosition)) >= 100 && currentCash != null) //snapping doesnt seem to work
        {
            float t = 1f - Mathf.Exp(-handAcceleration * Time.deltaTime);
            handTransform.localPosition = Vector2.Lerp(handTransform.localPosition, mouseLocalPosition, t);
        }
        else
        {
            handTransform.localPosition = mouseLocalPosition;
        }

    }
    override public void Enter(PlayerUIManager UIManager)
    {
        UIManager.playerInput.SwitchCurrentActionMap("Cash Grab Panel");
        gameObject.SetActive(true);
        cashDisplay.text = "Cash: $" + UIManager.playerCashComponent.GetCashHeld().ToString();
    }

    public override void Exit(PlayerUIManager UIManager)
    {
        gameObject.SetActive(false);
        if (currentCash != null)
        {
            Destroy(currentCash.gameObject);
            currentCash = null;
        }
    }

}
