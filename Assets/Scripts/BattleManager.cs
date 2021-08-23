using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    public GameObject characterIndicator;
    public Transform habilitiesHolder;
    public HabilityBox habilityPrefab;
    public Vector3 characterIndicatorOffset;
    public float characterIndicatorSpeed;

    CharacterBase currentCharacter;
    EnemyBase currentEnemy;
    bool habilityUsed;

    public static BattleManager Instance;

    void Awake()
    {
        Instance = this;    
    }

    void Update()
    {
        CharacterIndicatorPosition();
    }

    public void StartBatle()
    {
        StartCoroutine(BattleCycle());
    }

    IEnumerator BattleCycle()
    {
        characterIndicator.SetActive(true);

        //Player turn
        for (int i = 0; i < CharacterManager.Instance.SpawnedCharacters.Count; i++)
        {
            currentCharacter = CharacterManager.Instance.SpawnedCharacters[i];

            if(currentCharacter.health <= 0)
            {
                continue;
            }

            for (int j = 0; j < currentCharacter.habilities.Length; j++)
            {
                var hability = Instantiate(habilityPrefab, habilitiesHolder);
                hability.text.text = AddSpacesToSentence(currentCharacter.habilities[j].name.ToString());
                hability.fill.color = currentCharacter.habilities[j].color;
                hability.hability = currentCharacter.habilities[j];

                if (currentCharacter.habilities[j].isEnergetic)
                {
                    hability.fill.fillAmount = Mathf.Lerp(.3f, .7f, currentCharacter.energy / currentCharacter.maxEnergy);

                    if(currentCharacter.energy != currentCharacter.maxEnergy)
                    {
                        hability.GetComponent<Button>().interactable = false;
                    }
                }
            }

            while (!habilityUsed)
            {
                yield return null;
            }

            habilityUsed = false;

            yield return new WaitForSeconds(1f);

            if (EnemyManager.Instance.GetAliveEnemies().Length == 0)
            {
                BattleGameManager.Instance.Victory();
                yield break;
            }
        }

        currentCharacter = null;

        //Enemy turn
        for (int i = 0; i < EnemyManager.Instance.enemies.Count; i++)
        {
            currentEnemy = EnemyManager.Instance.enemies[i];

            if (currentEnemy.health <= 0)
            {
                continue;
            }

            if (currentEnemy.energy == currentEnemy.maxEnergy)
            {
                currentEnemy.EnergyAttack(CharacterManager.Instance.GetAliveCharacters());
            }
            else
            {
                currentEnemy.Attack(CharacterManager.Instance.GetAliveCharacter());
            }

            while (!habilityUsed)
            {
                yield return null;
            }

            habilityUsed = false;

            yield return new WaitForSeconds(1f);

            if (CharacterManager.Instance.GetAliveCharacters().Length == 0)
            {
                BattleGameManager.Instance.Defeat();
                yield break;
            }
        }

        StartCoroutine(BattleCycle());
    }

    void CharacterIndicatorPosition()
    {
        if(currentCharacter != null)
        {
            characterIndicator.transform.position = Vector3.Lerp(characterIndicator.transform.position, currentCharacter.transform.position + characterIndicatorOffset, characterIndicatorSpeed * Time.deltaTime);
        }
        else if(currentEnemy != null)
        {
            characterIndicator.transform.position = Vector3.Lerp(characterIndicator.transform.position, currentEnemy.transform.position + characterIndicatorOffset, characterIndicatorSpeed * Time.deltaTime);
        }
    }

    public void SelectHability(Hability hability)
    {
        foreach (Transform child in habilitiesHolder)
        {
            Destroy(child.gameObject);
        }

        switch (hability.name)
        {
            case Hability.Name.Attack:
                currentCharacter.Attack(EnemyManager.Instance.GetAliveEnemy());
                break;
            case Hability.Name.EnergyAttack:
                currentCharacter.EnergyAttack(EnemyManager.Instance.GetAliveEnemies());
                break;
            case Hability.Name.Heal:
                currentCharacter.Heal();
                break;
        }
    }

    public void HabilityUsed()
    {
        habilityUsed = true;
    }

    string AddSpacesToSentence(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return "";
        StringBuilder newText = new StringBuilder(text.Length * 2);
        newText.Append(text[0]);
        for (int i = 1; i < text.Length; i++)
        {
            if (char.IsUpper(text[i]) && text[i - 1] != ' ')
                newText.Append(' ');
            newText.Append(text[i]);
        }
        return newText.ToString();
    }
}
