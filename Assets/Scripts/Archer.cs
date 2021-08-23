using System.Collections;
using UnityEngine;
using Spine;
using EZCameraShake;

public class Archer : CharacterBase
{
    public CharacterAnimator characterAnimator;
    public GameObject energyParticle;
    public Transform energySpawnPoint;
    public GameObject arrowPrefab;
    public Transform arrowSpawnPoint;
    public float arrowHitTime;
    public float attackDamage;
    public float energyAttackDamage;

    EnemyBase currentEnemy;
    EnemyBase[] currentEnemies;

    void OnEnable()
    {
        characterAnimator.characterAnimator.AnimationState.Event += OnAttack;
        characterAnimator.characterAnimator.AnimationState.Complete += OnAnimationEnd;
    }

    void OnAttack(TrackEntry trackEntry, Spine.Event e)
    {
        if (trackEntry.Animation.Name == "Shoot1")
        {
            StartCoroutine(ThrowArrow());
        }
        else
        {
            GameObject go = Instantiate(energyParticle, energySpawnPoint.position, energyParticle.transform.rotation);
            Destroy(go, 5);
            CameraShaker.Instance.Shake(CameraShakePresets.Explosion);

            for (int i = 0; i < currentEnemies.Length; i++)
            {
                currentEnemies[i].Damage(energyAttackDamage);
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
            arrow.transform.position = Vector3.Lerp(arrowSpawnPoint.transform.position, currentEnemy.transform.position +  2.5f * Vector3.up, t);
            arrow.transform.right = currentEnemy.transform.position + 2.5f * Vector3.up - arrow.transform.position;

            yield return null;
        }

        currentEnemy.Damage(attackDamage);

        Destroy(arrow);

        energy = Mathf.Clamp(energy += 1, 0, maxEnergy);
        UpdateStats();
    }

    void OnAnimationEnd(TrackEntry trackEntry)
    {
        if (health <= 0)
            return;

        characterAnimator.characterAnimator.AnimationState.SetAnimation(0, "Idle ARCHER", true);
    }

    public override void Attack(EnemyBase enemy)
    {
        characterAnimator.characterAnimator.AnimationState.ClearTracks();
        characterAnimator.characterAnimator.AnimationState.SetAnimation(0, "Shoot1", false);
        currentEnemy = enemy;
    }

    public override void EnergyAttack(EnemyBase[] enemies)
    {
        characterAnimator.characterAnimator.AnimationState.ClearTracks();
        characterAnimator.characterAnimator.AnimationState.SetAnimation(0, "Shoot2", false);
        currentEnemies = enemies;
    }

    public override void Damage(float value)
    {
        health = Mathf.Clamp(health -= value, 0, maxHealth);
        energy = Mathf.Clamp(energy += value, 0, maxEnergy);

        if (health <= 0)
        {
            characterAnimator.characterAnimator.AnimationState.ClearTracks();
            characterAnimator.characterAnimator.AnimationState.SetAnimation(0, "Death", false);
        }
        else
        {
            characterAnimator.characterAnimator.AnimationState.ClearTracks();
            characterAnimator.characterAnimator.AnimationState.SetAnimation(0, "Hurt", false);
        }

        UpdateStats();
    }

    void OnDisable()
    {
        characterAnimator.characterAnimator.AnimationState.Event -= OnAttack;
    }
}
