using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public float lifetime = 3f;

    private Vector2 Direction;

    public void SetDirection(Vector2 dir)
    {
        Direction = dir;
        Destroy(gameObject, lifetime);
    }
    void Update()
    {
        transform.Translate(Direction * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player") return;
        
        Destroy(gameObject);
    }
}
