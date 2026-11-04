using UnityEngine;

public class HealthStatus : MonoBehaviour
{
    [SerializeField] protected int maxHP = 3;

    public int CurrentHP { get; private set; }
    public int MaxHP => maxHP;
    public bool IsDead => CurrentHP <= 0;

    protected virtual void Awake()
    {
        CurrentHP = maxHP;
    }

    public virtual void TakeDamage(int damage)
    {
        if (IsDead || damage <= 0) return;

        CurrentHP = Mathf.Max(0, CurrentHP - damage);

        if (IsDead)
            Die();
    }

    public void Heal(int amount)
    {
        if (IsDead || amount <= 0) return;

        CurrentHP = Mathf.Min(maxHP, CurrentHP + amount);
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }
}