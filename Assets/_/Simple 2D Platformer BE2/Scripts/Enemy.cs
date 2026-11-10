using UnityEditor.Tilemaps;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 2f;
    public Transform[] points;

    public bool canCharge = false;
    public float ChargeSpeed = 5f;
    public float sightDistance = 3f;
    public float LoseDistance = 5f;
    public LayerMask playerLayer;

    private int i;
    private SpriteRenderer spriteRenderer;
    private Transform player;
    private bool isCharging;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }


    void Update()
    {
        if (canCharge && DetectPlayer())
        {
            isCharging = true;
        }

        if (isCharging)
        {
            ChargePlayer();
        }
        else
        {
            patrol();
        }
    }

    void patrol()
    {
        if (Vector2.Distance(transform.position, points[i].position) < 0.25f)
        {
            i++;
            if (i == points.Length)
            {
                i = 0;
            }
        }

        transform.position = Vector2.MoveTowards(transform.position, points[i].position, speed = Time.deltaTime);

        spriteRenderer.flipX = (transform.position.x - points[i].position.x) < 0f;
    }

    bool DetectPlayer()
    {
        Vector2 direction = spriteRenderer.flipX ? Vector2.left : Vector2.right;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, sightDistance, playerLayer);

        if(hit.collider != null)
        {
            return true;
        }

        return false;
    }

    void ChargePlayer()
    {
        if (player == null)
        {
            isCharging = false;
            return;
        }

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance > LoseDistance)
        {
            isCharging = false;
            player = null;
            return;
        }

        Vector2 target = new Vector2(player.position.x, transform.position.y);

        transform.position = Vector2.MoveTowards(transform.position, target, ChargeSpeed * Time.deltaTime);

        spriteRenderer.flipX = (transform.position.x - player.position.x) > 0f;
    }
}
