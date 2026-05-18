using NUnit.Framework;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class PlayerInputManager : MonoBehaviour
{

    [SerializeField] Camera playerCamera;
    [SerializeField] Grid tilemapGrid;
    [SerializeField] Rigidbody2D playerRigidbody;
    [SerializeField] PlayerUIManager uiManager;

    Collider2D currentStairCollider;
    public Collider2D currentCashPileCollider;

    bool cashGrab = false;

    [SerializeField] float moveSpeed; 
    Vector2 moveVector = Vector2.zero;

    void Start()
    {
        ChangePlayerFloor(10); // 10 is ground
    }

    private void ChangePlayerFloor(int floorLayer)
    {
        Tilemap[] tilemaps = tilemapGrid.GetComponentsInChildren<Tilemap>(true);
        for (int i = 0; i < tilemaps.Length; i++)
        {
            if (tilemaps[i].gameObject.layer != floorLayer)
            {
                tilemaps[i].gameObject.SetActive(false);
            }
            else
            {
                tilemaps[i].gameObject.SetActive(true);
            }
        }
        gameObject.layer = floorLayer;

    }

    private void HandleStairs()
    {
        Stairs currentStairs = currentStairCollider.gameObject.GetComponent<Stairs>();
        float stairProgress = (transform.position.y - currentStairCollider.bounds.min.y) / currentStairCollider.bounds.size.y; // using bounds is not reliable for rotated collider
        if (currentStairs.isLowerStairs)
        {
            if(stairProgress > 1)
            {
                ChangePlayerFloor(currentStairs.otherStairs.gameObject.layer);
            }
        }
        else
        {
            if (stairProgress < 0)
            {
                ChangePlayerFloor(currentStairs.otherStairs.gameObject.layer);
            }
        }
        
    }

    private IEnumerator CashGrabTransition(Panel nextPanel, Camera camera, float targetZoom,float duration)
    {
        float timeElapsed = 0f;
        while (timeElapsed < duration)
        {
            camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, targetZoom, timeElapsed / duration);
            camera.transform.localPosition = Vector3.Lerp(camera.transform.localPosition, new Vector3(0f, 0f, camera.transform.localPosition.z), timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        camera.orthographicSize = targetZoom;
        camera.transform.localPosition = new Vector3(0f, 0f, camera.transform.localPosition.z);

        uiManager.SwitchPanel(nextPanel);
    }

    private void StartCashGrab()
    {
        if (!cashGrab)
        {
            cashGrab = true;
            Debug.Log(currentCashPileCollider);
            playerCamera.transform.SetParent(currentCashPileCollider.transform.Find("CameraPivot")); //genuinely disgusting
            StartCoroutine(CashGrabTransition(uiManager.cashGrabPanel ,playerCamera, 2f, 1f));
        }
        
    }

    public void EndCashGrab()
    {
        cashGrab = false;
        playerCamera.transform.SetParent(gameObject.transform.Find("CameraPivot")); //genuinely disgusting
        StartCoroutine(CashGrabTransition(uiManager.defaultPanel ,playerCamera, 8f, 1f));
    }

    // ----------------------- MONOBEHAVIOUR -----------------------------
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Stairs"))
        {
            currentStairCollider = collision;
        }
        if (collision.CompareTag("Cash Pile"))
        {
            currentCashPileCollider = collision;
        }

    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Stairs"))
        {
            currentStairCollider = null;
        }
        if (collision.CompareTag("Cash Pile"))
        {
            currentCashPileCollider = null;
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveVector = context.ReadValue<Vector2>();
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (currentCashPileCollider)
        {
            StartCashGrab();
        }
    }

    private void FixedUpdate()
    {
        if (!cashGrab)
        {
            playerRigidbody.linearVelocity = moveVector * moveSpeed * Time.fixedDeltaTime;
        }
        
    }

    void Update()
    {
        if (currentStairCollider != null)
        {
            HandleStairs();
        }

    }
}
