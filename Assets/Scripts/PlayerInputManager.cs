using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class PlayerInputManager : MonoBehaviour
{

    [SerializeField] Camera playerCamera;
    [SerializeField] Grid tilemapGrid;
    [SerializeField] Rigidbody2D playerRigidbody;

    Collider2D currentStairCollider;
    Collider2D currentCashPileCollider;

    bool cashGrab;

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

    private void StartCashGrab()
    {
        cashGrab = true;
        

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

    public void OnInteract()
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
