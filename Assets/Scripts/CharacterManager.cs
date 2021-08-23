using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class CharacterManager : MonoBehaviour
{
    public CharacterBase[] characters;
    public CharacterSelectionBox selectionBoxPrefab;
    public Transform selectionBoxHolder;
    public Transform[] points;
    public GameObject[] pointsText;
    public CharacterStats characterStatsPrefab;
    public Transform characterStatsHolder;
    public Animator canvasAnimator;
    public Button startButton;

    public List<CharacterBase> SpawnedCharacters { get; private set; } = new List<CharacterBase>();

    public static CharacterManager Instance;

    void Awake()
    {
        Instance = this;

        for (int i = 0; i < characters.Length; i++)
        {
            CharacterSelectionBox box = Instantiate(selectionBoxPrefab, selectionBoxHolder);
            box.characterPrefab = characters[i];
            box.icon.sprite = characters[i].icon;
        }
    }

    public void Place(CharacterSelectionBox selectionBox)
    {
        CharacterBase characterObject = Instantiate(selectionBox.characterPrefab);
        SpawnedCharacters.Add(characterObject);
        selectionBox.character = characterObject;
        UpdateBehaviour();
    }

    public void Delete(CharacterSelectionBox selectionBox)
    {
        SpawnedCharacters.Remove(selectionBox.character);
        Destroy(selectionBox.character.gameObject);
        UpdateBehaviour();
    }

    void UpdateBehaviour()
    {
        for (int i = 0; i < pointsText.Length; i++)
        {
            pointsText[i].SetActive(false);
        }

        for (int i = 0; i < SpawnedCharacters.Count; i++)
        {
            pointsText[i].SetActive(true);
            pointsText[i].transform.position = Camera.main.WorldToScreenPoint(points[i].position);
            SpawnedCharacters[i].transform.position = points[i].position;
        }

        startButton.interactable = SpawnedCharacters.Count >= 1;
    }

    public void StartBattle()
    {
        for (int i = 0; i < pointsText.Length; i++)
        {
            pointsText[i].SetActive(false);
        }

        for (int i = SpawnedCharacters.Count - 1; i >= 0; i--)
        {
            var character = Instantiate(characterStatsPrefab, characterStatsHolder);
            character.icon.sprite = SpawnedCharacters[i].icon;
            SpawnedCharacters[i].characterStats = character;
        }

        canvasAnimator.SetTrigger("Battle");
        BattleManager.Instance.StartBatle();
    }

    public CharacterBase GetAliveCharacter()
    {
        for (int i = 0; i < SpawnedCharacters.Count; i++)
        {
            if (SpawnedCharacters[i].health > 0)
            {
                return SpawnedCharacters[i];
            }
        }

        return null;
    }

    public CharacterBase[] GetAliveCharacters()
    {
        List<CharacterBase> characterAlive = new List<CharacterBase>();

        for (int i = 0; i < SpawnedCharacters.Count; i++)
        {
            if (SpawnedCharacters[i].health > 0)
            {
                characterAlive.Add(SpawnedCharacters[i]);
            }
        }

        return characterAlive.ToArray();
    }
}
