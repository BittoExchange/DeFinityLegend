using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    public string enemyrName;
    public Hability[] habilities;
    public float health;
    public float maxHealth;
    public float energy;
    public float maxEnergy;
    public Animator animator;
    public EnemyStats enemyStats;

    public virtual void Attack(CharacterBase character) { }
    public virtual void EnergyAttack(CharacterBase[] characters) { }
    public virtual void Damage(float value) { }

    public void UpdateStats()
    {
        enemyStats.health.fillAmount = health / maxHealth;
        enemyStats.energy.fillAmount = energy / maxEnergy;
    }
}
