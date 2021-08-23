using UnityEngine;
using Spine;
using EZCameraShake;

public class Warrior : CharacterBase
{
    public CharacterAnimator characterAnimator;
    public GameObject energyParticle;
    public Transform energySpawnPoint;
    public AudioSource swordSound;
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
        if(trackEntry.Animation.Name == "Attack 1 DUELIST")
        {
            swordSound.Play();
            currentEnemy.Damage(attackDamage);
            energy = Mathf.Clamp(energy += 1, 0, maxEnergy);
            UpdateStats();
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

    void OnAnimationEnd(TrackEntry trackEntry)
    {
        if (health <= 0)
            return;

        characterAnimator.characterAnimator.AnimationState.SetAnimation(0, "Idle", true);
    }

    public override void Attack(EnemyBase enemy)
    {
        characterAnimator.characterAnimator.AnimationState.ClearTracks();
        characterAnimator.characterAnimator.AnimationState.SetAnimation(0, "Attack 1 DUELIST", false);
        currentEnemy = enemy;
    }

    public override void EnergyAttack(EnemyBase[] enemies)
    {
        characterAnimator.characterAnimator.AnimationState.ClearTracks();
        characterAnimator.characterAnimator.AnimationState.SetAnimation(0, "Attack1", false);
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
