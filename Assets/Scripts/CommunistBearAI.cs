using UnityEngine;
using UnityEngine.SceneManagement;

public class CommunistBearAI : MonoBehaviour
{
    public GameObject Player;
    public Animator Animator;

    public LayerMask PlayerLayerMask;

    private BoxCollider2D Collision;

    public float Speed = 3f;
    public float MaxSpeed = 1f;
    public float RangeOfSight = 10f;

    private void Start()
    {
        Animator = GetComponent<Animator>();
        Collision = GetComponent<BoxCollider2D>();
    }

    private void FixedUpdate()
    {
        float distance = Vector3.Distance(transform.position, Player.transform.position);

        bool facingLeft = (Player.transform.position.x - transform.position.x) < 0;

        RaycastHit2D hit = Physics2D.BoxCast(
            Collision.bounds.center,
            Collision.bounds.size * 2,
            0f,
            facingLeft ? Vector2.left : Vector2.right,
            RangeOfSight,
            PlayerLayerMask
        );

        // Color rayColor;
        // if (hit.collider != null)
        // {
        //     rayColor = Color.red;
        // }
        // else
        // {
        //     rayColor = Color.green;
        // }

        // Debug.DrawLine(
        //     Collision.bounds.center,
        //     new Vector3(Collision.bounds.center.x + (facingLeft ? -RangeOfSight : RangeOfSight), Collision.bounds.center.y, 0f),
        //     rayColor
        // );

        if (hit.collider != null)
        {
            Vector3 velocity = new Vector3();

            if (facingLeft)
            {
                velocity.x = -Speed * Time.deltaTime;
                transform.localScale = new Vector3(2, 2, 0);
            }
            else
            {
                velocity.x = Speed * Time.deltaTime;
                transform.localScale = new Vector3(-2, 2, 0);
            }

            Vector3.ClampMagnitude(velocity, MaxSpeed);

            // play animation
            if (!Animator.GetBool("InPursuit")) Animator.SetBool("InPursuit", true);

            // move enemy
            transform.Translate(velocity);
        }
        else
        {
            if (Animator.GetBool("InPursuit")) Animator.SetBool("InPursuit", false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.GetComponent<Player>() != null)
        {
            Debug.Log("You have died!");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
