namespace Interfaces
{
    public interface IDamageable
    {
        public float GetHealth();
        public bool TakeDamage(float damageAmount);
    }
}