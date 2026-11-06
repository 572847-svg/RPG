using Unity.VisualScripting;
using UnityEngine;

public class MovingPlatform1 : MonoBehaviour
{
    public float TimeBeforeFall;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            transform.AddComponent<Rigidbody2D>();
        }
    }
}
