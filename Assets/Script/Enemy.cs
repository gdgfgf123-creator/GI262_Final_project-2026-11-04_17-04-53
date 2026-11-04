using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : HealthStatus
{
    [SerializeField] protected float moveSpeed = 2f;
    [SerializeField] private int contactDamage = 1;

    protected Rigidbody2D rb;
    protected Player target;

    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody2D>();
    }

    protected virtual void Start()
    {
        target = FindFirstObjectByType<Player>();
    }

    protected virtual void FixedUpdate()
    {
        if (target == null || target.IsDead || IsDead) return;

        Vector2 nextPosition = Vector2.MoveTowards(
            rb.position,
            target.transform.position,
            moveSpeed * Time.fixedDeltaTime
        );

        rb.MovePosition(nextPosition);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        Player player = collision.collider.GetComponentInParent<Player>();

        if (player != null)
            player.TakeDamage(contactDamage);
    }

    protected override void Die()
    {
        Debug.Log("Enemy defeated");
        base.Die();
    }
}