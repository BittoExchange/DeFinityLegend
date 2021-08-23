using System.Collections;
using UnityEngine;
using EZCameraShake;

public class EnemyWarrior : EnemyBase
{
    public Transform energySpawnPoint;
    public GameObject energyParticle;
    public GameObject arrowPrefab;
    public Transform arrowSpawnPoint;
    public float arrowHitTime;
    public float attackDamage;
    public float energyAttackDamage;

    CharacterBase currentCharacter;
    CharacterBase[] currentCharacters;

    public void OnAttack(string attack)
    {
        BattleManager.Instance.HabilityUsed();

        if (attack == "Attack")
        {
            StartCoroutine(ThrowArrow());
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

    IEnumerator ThrowArrow()
    {
        GameObject arrow = Instantiate(arrowPrefab, arrowSpawnPoint.position, Quaternion.identity);

        float t = 0;

        while (t < 1)
        {
            t += Time.deltaTime / arrowHitTime;
            arrow.transform.position = Vector3.Lerp(arrowSpawnPoint.transform.position, currentCharacter.transform.position + 1.5f * Vector3.up, t);
            arrow.transform.right = currentCharacter.transform.position + 1.5f * Vector3.up - arrow.transform.position;

            yield return null;
        }

        currentCharacter.Damage(attackDamage);

        Destroy(arrow);

        energy = Mathf.Clamp(energy += 1, 0, maxEnergy);
        UpdateStats();
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
