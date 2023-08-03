using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController2D : MonoBehaviour
{
    [Header("Player Data")] // Public Data
    [SerializeField] private float speed = 6f;
    [SerializeField] private float jumpForce = 12f;
    [SerializeField] [Range(0.0f, 0.9f)] private float jumpMultiplier = 0.55f;

    // References
    private Rigidbody2D rb;
    private Transform groundCheck;
    private LayerMask groundLayer = 1 << 6;

    // Private Data
    private float x;
    private bool isFacingRight;
    private float groundCheckSize = 0.2f;

    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody2D>(); // Define "rb" Variable as Rigidbody2D 
        groundCheck = transform.Find("GroundCheck"); // Define "groundCheck" Variable as GroundCheck Object
    }

    void FixedUpdate() {
        rb.velocity = new Vector2(x * speed, rb.velocity.y); // Move Player Character

        if (!isFacingRight && x < 0f) {
            Flip(); // Flip Player Character if Moving Right & Facing Left
        } else if (isFacingRight && x > 0f) {
            Flip(); // Flip Player Character if Moving Left & Facing Right
        }
    }

    public void Walk(InputAction.CallbackContext context) {
        x = context.ReadValue<Vector2>().x; // Define "x" Variable, Dependant on what the Player is Doing

        if (x > 0 && x < 1) {
            x = 1; // Round Up "x" Variable
        } else if (x < 0 && x > -1) {
            x = -1; // Round Down "x" Variable
        }
    }

    private void Flip() {
        isFacingRight = !isFacingRight; // Change "isFacingRight" Variable to the Opposite
        Vector3 rotation = transform.rotation.eulerAngles; // Store Player Character Rotation to Vector3
        
        if (rotation.y == 0) {
            rotation.y += 180; // Add 180 to the Y-Axis Data in "rotation" Variable
        } else if (rotation.y == 180) {
            rotation.y -= 180; // Remove 180 to the Y-Axis Data in "rotation" Variable
        }

        transform.rotation = Quaternion.Euler(rotation); // Rotate Player Character
    }

    public void Jump(InputAction.CallbackContext context) {
        if (IsGrounded()) {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce); // Make Player Character Jump, if IsGrounded() is true
        }

        if (rb.velocity.y > 0f) {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * jumpMultiplier); // Make Player Character Jump Higher, if Player Holds Jump Button
        }
    }

    bool IsGrounded() {
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckSize, groundLayer); // Check if Player is Touching the Ground
    }
}
