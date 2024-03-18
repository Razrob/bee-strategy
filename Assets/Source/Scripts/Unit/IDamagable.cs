
public interface IDamagable
{
    public AffiliationEnum Affiliation { get; }
    
    public void TakeDamage(IDamageApplicator damageApplicator);
}