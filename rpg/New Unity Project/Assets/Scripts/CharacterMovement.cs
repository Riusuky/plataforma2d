using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    // RidgidBody component instance for the player
    private Rigidbody2D playerRigidBody2D;

    //Variable to track how much movement is needed from input
    private float movePlayerVector;

    // For determining which way the player is currently facing.
    private bool facingRight;

    // Speed modifier for player movement
    public float speed = 4.0f;

    // Reference to the player's sprite GameObject.
    private GameObject playerSprite;

    // Reference to the player's animator component.
    private Animator anim;

    void Awake()
    {
        // Setting up references.
        playerRigidBody2D = (Rigidbody2D)GetComponent(typeof(Rigidbody2D));
        playerSprite = transform.Find("PlayerSprite").gameObject;
        anim = (Animator)playerSprite.GetComponent(typeof(Animator));
    }

    // Update is called once per frame
    void Update()
    {
        // Cache the horizontal input.
        movePlayerVector = Input.GetAxis("Horizontal");

        anim.SetFloat("speed", Mathf.Abs(movePlayerVector));

        playerRigidBody2D.velocity = new Vector2(movePlayerVector * speed, playerRigidBody2D.velocity.y);

        if (movePlayerVector > 0 && !facingRight)
        {
            Flip();
        }
        else if (movePlayerVector < 0 && facingRight)
        {
            Flip();
        }
    }

    void Flip()
    {
        // Switch the way the player is labelled as facing.
        facingRight = !facingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = playerSprite.transform.localScale;
        theScale.x *= -1;
        playerSprite.transform.localScale = theScale;
    }
}
