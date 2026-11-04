using UnityEngine;

public class Boss : Enemy
{
    [Header("Movement")]
    [SerializeField] private Transform[] movePoints;
    [SerializeField] private float arriveDistance = 0.1f;

    [Header("Attack")]
    [SerializeField] private BasicAttack bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireInterval = 1f;

    private int pointIndex;
    private float nextFireTime;

    private void Update()
    {
        if (IsDead || target == null || target.IsDead ||
            Time.timeScale == 0f) return;

        if (Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireInterval;
            Shoot();
        }
    }

    protected override void FixedUpdate()
    {
        if (IsDead || target == null || target.IsDead) return;
        if (movePoints == null || movePoints.Length == 0) return;

        Transform destination = movePoints[pointIndex];

        if (destination == null) return;

        rb.MovePosition(
            Vector2.MoveTowards(
                rb.position,
                destination.position,
                moveSpeed * Time.fixedDeltaTime
            )
        );

        if (Vector2.Distance(rb.position, destination.position)
            <= arriveDistance)
        {
            pointIndex = (pointIndex + 1) % movePoints.Length;
        }
    }

    private void Shoot()
    {
        if (bulletPrefab == null || firePoint == null) return;

        Vector2 direction =
            target.transform.position - firePoint.position;

        BasicAttack bullet = Instantiate(
            bulletPrefab,
            firePoint.position,
            Quaternion.identity
        );

        bullet.Initialize(direction, 1, false);
    }

    protected override void Die()
    {
        Debug.Log("Boss defeated - You Win!");
        base.Die();
    }
}