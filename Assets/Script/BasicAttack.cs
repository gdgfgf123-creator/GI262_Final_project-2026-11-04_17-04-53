using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BasicAttack : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float lifeTime = 5f;

    private Vector2 direction;
    private int damage;
    private bool fromPlayer;
    private bool hasHit;

    public void Initialize(
        Vector2 shootDirection,
        int attackDamage,
        bool isPlayerBullet)
    {
        direction = shootDirection.normalized;
        damage = attackDamage;
        fromPlayer = isPlayerBullet;

        Destroy(gameObject, lifeTime);
    }

    private void FixedUpdate()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        rb.MovePosition(
            rb.position + direction * speed * Time.fixedDeltaTime
        );
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasHit) return;

        HealthStatus target = null;

        if (fromPlayer)
            target = other.GetComponentInParent<Enemy>();
        else
            target = other.GetComponentInParent<Player>();

        if (target == null || target.IsDead) return;

        hasHit = true;
        target.TakeDamage(damage);

        Destroy(gameObject);
    }
}