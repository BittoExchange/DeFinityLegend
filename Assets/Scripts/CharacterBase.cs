using UnityEngine;
using System;

public class CharacterBase : MonoBehaviour
{
    public string characterName;
    public Sprite icon;
    public Hability[] habilities;
    public float health;
    public float maxHealth;
    public float energy;
    public float maxEnergy;
    public CharacterStats characterStats;

    public virtual void Attack(EnemyBase enemy) { }
    public virtual void EnergyAttack(EnemyBase[] enemies) { }
    public virtual void Heal() { }
    public virtual void Damage(float value) { }

    public void UpdateStats()
    {
        characterStats.health.fillAmount = health / maxHealth;
        characterStats.energy.fillAmount = energy / maxEnergy;
    }
}

[Serializable]
public class Hability
{
    public enum Name
    {
        Attack, EnergyAttack, Heal
    }

    public Name name;
    public Color color;
    public bool isEnergetic;
}
