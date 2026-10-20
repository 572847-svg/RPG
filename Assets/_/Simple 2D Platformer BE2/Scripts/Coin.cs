using UnityEngine;

public class Coin : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Players")
        {
            Player player = collision.gameObject.GetComponent<Player>();
            player.coin += 1;
            Destroy(gameObject);
        }
    }
}
