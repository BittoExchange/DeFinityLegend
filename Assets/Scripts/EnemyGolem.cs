using UnityEngine;
using EZCameraShake;

public class EnemyGolem : EnemyBase
{
    public float attackDamage;
    public float energyAttackDamage;
    public AudioSource attackSound;
    public GameObject energyParticle;
    public Transform energySpawnPoint;

    CharacterBase currentCharacter;
    CharacterBase[] currentCharacters;

    public void OnAttack(string attack)
    {
        if(attack == "Attack")
        {
            attackSound.Play();
            currentCharacter.Damage(attackDamage);
            energy = Mathf.Clamp(energy += 1, 0, maxEnergy);
            UpdateStats();
        }
        else
        {
            GameObject go = Instantiate(energyParticle, energySpawnPoint.position, energySpawnPoint.localRotation);
            Destroy(go, 5);
            CameraShaker.Instance.Shake(CameraShakePresets.Explosion);

            for (int i = 0; i < currentCharacters.Length; i++)
            {
                currentCharacters[i].Damage(energyAttackDamage);
            }

            energy = 0;
            UpdateStats();
        }

        BattleManager.Instance.HabilityUsed();
    }

    public override void Attack(CharacterBase character)
    {
        animator.SetTrigger("Attack");
        currentCharacter = character;
    }

    public override void EnergyAttack(CharacterBase[] characters)
    {
        animator.SetTrigger("EnergyAttack");
        currentCharacters = characters;
    }

    public override void Damage(float value)
    {
        health = Mathf.Clamp(health -= value, 0, maxHealth);
        energy = Mathf.Clamp(energy += value, 0, maxEnergy);

        if (health <= 0)
        {
            animator.SetTrigger("Death");
        }
        else
        {
            animator.SetTrigger("Damage");
        }

        UpdateStats();
    }
}
