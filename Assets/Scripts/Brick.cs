using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Brick : MonoBehaviour
{
    public int points = 100;
    public bool unbreakable;

    [Header("Power-up Settings")]
    public float powerUpChance = 0.2f;
    public GameObject powerUpPrefab;

    private SpriteRenderer spriteRenderer;
    private int health = 2; // 2 hits: first for white, second to break
    private Color originalColor; // Store the original random color

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        ResetBrick();
    }

    public void ResetBrick()
    {
        gameObject.SetActive(true);

        if (!unbreakable)
        {
            health = 2;
            // Set random initial color (avoiding white)
            originalColor = new Color(
                Random.Range(0.2f, 0.9f),
                Random.Range(0.2f, 0.9f),
                Random.Range(0.2f, 0.9f)
            );
            spriteRenderer.color = originalColor;
        }
    }

    private void Hit()
    {
        if (unbreakable) {
            return;
        }

        health--;

        if (health == 1)
        {
            // First hit - turn white
            spriteRenderer.color = Color.white;
        }
        else if (health <= 0)
        {
            // Second hit - destroy brick
            // Chance to spawn power-up
            if (Random.value < powerUpChance && powerUpPrefab != null)
            {
                Instantiate(powerUpPrefab, transform.position, Quaternion.identity);
            }
            gameObject.SetActive(false);
        }

        GameManager.Instance.OnBrickHit(this);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball")) {
            Hit();
        }
    }
}
