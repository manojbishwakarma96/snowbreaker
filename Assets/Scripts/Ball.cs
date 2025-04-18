using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Ball : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float defaultSpeed = 10f;
    public float speed { get; set; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        speed = defaultSpeed;
    }

    private void Start()
    {
        ResetBall();
    }

    public void ResetBall()
    {
        rb.velocity = Vector2.zero;
        transform.position = Vector2.zero;
        speed = defaultSpeed;

        CancelInvoke();
        Invoke(nameof(SetRandomTrajectory), 1f);
    }

    private void SetRandomTrajectory()
    {
        Vector2 force = new Vector2(Random.Range(-1f, 1f), -1f);
        rb.AddForce(force.normalized * speed, ForceMode2D.Impulse);
    }

    private void FixedUpdate()
    {
        rb.velocity = rb.velocity.normalized * speed;
    }
}
