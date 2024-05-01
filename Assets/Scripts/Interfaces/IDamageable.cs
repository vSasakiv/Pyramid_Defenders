public interface IDamageable
{
    public float Health { get; set; }
    public bool TakeDamage(float damageAmount);
}