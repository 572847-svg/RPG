using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class Coin : MonoBehaviour
{
    private TextMeshProUGUI coinText;

    private void Start()
    {
        coinText = GameObject.FindWithTag("coinText").GetComponent<TextMeshProUGUI>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Player player = collision.gameObject.GetComponent<Player>();
            player.coin += 1;
            coinText.text = player.coin.ToString();
            Destroy(gameObject);
        }
    }
}
