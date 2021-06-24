using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public LayerMask PlatformLayerMask;
    public enum MotionState
    {
        Walking, Jumping, Idling
    }
    public MotionState State = MotionState.Idling;

    public float MoveSpeed = 10f;
    public float MaxAxisSpeed = 10f;
    public float JumpForce = 800f;
    public float ExtraHeightText = 0.1f;
    private Vector2 JumpVector;
    private Vector3 AxisVector = new Vector3();

    private Animator Animator;
    private SpriteRenderer Sprite;
    private Rigidbody2D Body;
    private CircleCollider2D CircleCollider;

    private void Start()
    {
        JumpVector = new Vector2(0f, JumpForce);
        Animator = GetComponent<Animator>();
        Sprite = GetComponent<SpriteRenderer>();
        Body = GetComponent<Rigidbody2D>();
        CircleCollider = GetComponent<CircleCollider2D>();
    }

    private void Update()
    {
        // if player fell from the platform, reset level
        if (transform.position.y < -10)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private void FixedUpdate()
    {
        // toggle walking animation
        Animator.SetFloat("Speed", Mathf.Abs(AxisVector.x));

        // move player
        transform.Translate(AxisVector * MoveSpeed * Time.deltaTime);

        // flip sprite based on orientation
        if (AxisVector.x > 0)
        {
            if (Sprite.flipX) Sprite.flipX = false;
        }
        else if (AxisVector.x < 0)
        {
            if (!Sprite.flipX) Sprite.flipX = true;
        }

        IsGrounded();
        // if (IsGrounded())
        // {
        //     State = MotionState.Idling;
        // }
    }

    public void Move(InputAction.CallbackContext context)
    {
        AxisVector.x = context.ReadValue<Vector2>().x;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.started && IsGrounded())
        {
            // add jump force
            Body.AddForce(JumpVector);
            if (State != MotionState.Jumping) State = MotionState.Jumping;

            // play animation
            Animator.SetTrigger("Jump");
        }
    }

    private bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.BoxCast(
            CircleCollider.bounds.center,
            CircleCollider.bounds.size - new Vector3(0.1f, 0, 0), // so the box won't mistake walls for ground
            0f,
            Vector2.down,
            CircleCollider.bounds.extents.y + ExtraHeightText,
            PlatformLayerMask
        );

        Color rayColor;
        if (hit.collider != null)
        {
            rayColor = Color.red;
        }
        else
        {
            rayColor = Color.green;
        }

        // draw a box - debug only
        Debug.DrawRay(
            new Vector3(CircleCollider.bounds.center.x + (CircleCollider.bounds.size.x / 2f), CircleCollider.bounds.center.y, 0f),
            Vector2.down * (CircleCollider.bounds.extents.y + ExtraHeightText),
            rayColor
        );

        Debug.DrawRay(
            new Vector3(CircleCollider.bounds.center.x - (CircleCollider.bounds.size.x / 2f), CircleCollider.bounds.center.y, 0f),
            Vector2.down * (CircleCollider.bounds.extents.y + ExtraHeightText),
            rayColor
        );

        Debug.DrawRay(
            new Vector3(CircleCollider.bounds.center.x - (CircleCollider.bounds.size.x / 2f), CircleCollider.bounds.center.y - (CircleCollider.bounds.size.y / 2f) - ExtraHeightText, 0f),
            Vector2.right * (CircleCollider.bounds.size.x),
            rayColor
        );

        return hit.collider != null;
    }
}
