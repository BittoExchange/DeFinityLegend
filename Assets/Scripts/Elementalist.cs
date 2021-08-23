using System.Collections;
using UnityEngine;
using Spine;
using EZCameraShake;

public class Elementalist : CharacterBase
{
    public CharacterAnimator characterAnimator;
    public GameObject energyParticle;
    public AudioSource healSound;
    public AudioSource hitSound;
    public AudioSource spellSound;
    public GameObject spellPrefab;
    public GameObject spellHitPrefab;
    public Transform spellSpawnPoint;
    public float spellHitTime;
    public float attackDamage;
    public float healAmount;

    EnemyBase currentEnemy;

    void OnEnable()
    {
        characterAnimator.characterAnimator.AnimationState.Event += OnAttack;
        characterAnimator.characterAnimator.AnimationState.Complete += OnAnimationEnd;
    }

    void OnAttack(TrackEntry trackEntry, Spine.Event e)
    {
        if (trackEntry.Animation.Name == "Cast2")
        {
            StartCoroutine(ThrowSpell());
        }
        else
        {
            for (int i = 0; i < CharacterManager.Instance.SpawnedCharacters.Count; i++)
            {
                if (CharacterManager.Instance.SpawnedCharacters[i].health <= 0)
                    continue;

                CharacterManager.Instance.SpawnedCharacters[i].health = Mathf.Clamp(CharacterManager.Instance.SpawnedCharacters[i].health += healAmount, 0, CharacterManager.Instance.SpawnedCharacters[i].maxHealth);
                CharacterManager.Instance.SpawnedCharacters[i].UpdateStats();
                GameObject go = Instantiate(energyParticle, CharacterManager.Instance.SpawnedCharacters[i].transform.position, energyParticle.transform.rotation);
                Destroy(go, 5);
            }

            healSound.Play();
            hitSound.Play();

            CameraShaker.Instance.Shake(CameraShakePresets.Explosion);

            energy = 0;
            UpdateStats();
        }

        BattleManager.Instance.HabilityUsed();
    }

    IEnumerator ThrowSpell()
    {
        GameObject spell = Instantiate(spellPrefab, spellSpawnPoint.position, Quaternion.identity);

        float t = 0;

        spellSound.Play();

        while (t < 1)
        {
            t += Time.deltaTime / spellHitTime;
            spell.transform.position = Vector3.Lerp(spellSpawnPoint.transform.position, currentEnemy.transform.position + 2.5f * Vector3.up, t);
            spell.transform.right = currentEnemy.transform.position + 2.5f * Vector3.up - spell.transform.position;

            yield return null;
        }

        GameObject hit = Instantiate(spellHitPrefab, spell.transform.position, spell.transform.rotation);

        Destroy(spell, .5f);
        Destroy(hit, 5);

        currentEnemy.Damage(attackDamage);

        energy = Mathf.Clamp(energy += 1, 0, maxEnergy);
        UpdateStats();
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
        characterAnimator.characterAnimator.AnimationState.SetAnimation(0, "Cast2", false);
        currentEnemy = enemy;
    }

    public override void Heal()
    {
        characterAnimator.characterAnimator.AnimationState.ClearTracks();
        characterAnimator.characterAnimator.AnimationState.SetAnimation(0, "Cast3", false);
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
