using UnityEngine;

public class Flag : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.GetComponent<Player>() != null)
        {
            Debug.Log("You have passed the level!");
            LevelController.PassLevel();
        }
    }
}
