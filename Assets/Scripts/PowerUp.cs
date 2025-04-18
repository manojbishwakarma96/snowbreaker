using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PowerUp : MonoBehaviour
{
    public float speed = 5f;
    public float duration = 10f;
    public float speedMultiplier = 1.5f;

    private void Start()
    {
        // Ensure the collider is set to trigger
        GetComponent<Collider2D>().isTrigger = true;
    }

    private void Update()
    {
        // Power-up falls down
        transform.Translate(Vector2.down * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Paddle"))
        {
            // Apply power-up effect
            Ball ball = FindObjectOfType<Ball>();
            if (ball != null)
            {
                // Increase ball speed temporarily
                ball.speed *= speedMultiplier;

                // Reset after duration
                Invoke(nameof(ResetBallSpeed), duration);
            }

            // Destroy the power-up
            Destroy(gameObject);
        }
        else if (other.CompareTag("ResetZone"))
        {
            // Destroy if missed
            Destroy(gameObject);
        }
    }

    private void ResetBallSpeed()
    {
        Ball ball = FindObjectOfType<Ball>();
        if (ball != null)
        {
            ball.ResetBall();
        }
    }
}
