namespace RTS.Object.Unit.Capabilities.General
{
    public interface IAttackable
    {
        public float CurrentHealth { get; set; }
        public void TakeDamage(float attackPoint);
        public void Die();
    }
}
