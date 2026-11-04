using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : HealthStatus
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("Attack")]
    [SerializeField] private BasicAttack bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireInterval = 0.2f;
    [SerializeField] private int damage = 10;

    [Header("Critical")]
    [SerializeField, Range(0f, 1f)] private float critChance = 0.1f;
    [SerializeField] private int critMultiplier = 2;

    [Header("Health")]
    [SerializeField] private float invincibleTime = 1f;

    private Rigidbody2D rb;
    private Camera mainCamera;
    private Vector2 movement;
    private Vector2 aimDirection = Vector2.up;
    private float nextFireTime;
    private float nextDamageTime;

    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (IsDead || Time.timeScale == 0f) return;

        ReadMovement();
        ReadAim();

        if (Mouse.current != null &&
            Mouse.current.leftButton.isPressed &&
            Time.time >= nextFireTime)
        {
            Shoot();
        }
    }

    private void ReadMovement()
    {
        movement = Vector2.zero;
        Keyboard keyboard = Keyboard.current;

        if (keyboard == null) return;

        if (keyboard.wKey.isPressed) movement.y += 1;
        if (keyboard.sKey.isPressed) movement.y -= 1;
        if (keyboard.aKey.isPressed) movement.x -= 1;
        if (keyboard.dKey.isPressed) movement.x += 1;

        movement = movement.normalized;
    }

    private void ReadAim()
    {
        if (Mouse.current == null || mainCamera == null) return;

        Vector2 screenPosition = Mouse.current.position.ReadValue();

        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(
            new Vector3(
                screenPosition.x,
                screenPosition.y,
                -mainCamera.transform.position.z
            )
        );

        Vector2 direction = (Vector2)worldPosition - rb.position;

        if (direction.sqrMagnitude > 0.001f)
            aimDirection = direction.normalized;
    }

    private void FixedUpdate()
    {
        if (IsDead) return;

        rb.MovePosition(
            rb.position + movement * moveSpeed * Time.fixedDeltaTime
        );
    }

    private void Shoot()
    {
        if (bulletPrefab == null || firePoint == null) return;

        nextFireTime = Time.time + fireInterval;

        bool isCritical = Random.value < critChance;
        int finalDamage = isCritical
            ? damage * critMultiplier
            : damage;

        BasicAttack bullet = Instantiate(
            bulletPrefab,
            firePoint.position,
            Quaternion.identity
        );

        bullet.Initialize(aimDirection, finalDamage, true);
    }

    public override void TakeDamage(int damage)
    {
        if (IsDead || damage <= 0 || Time.time < nextDamageTime) return;

        nextDamageTime = Time.time + invincibleTime;
        base.TakeDamage(damage);
    }

    protected override void Die()
    {
        Debug.Log("Game Over");
        base.Die();
    }
}