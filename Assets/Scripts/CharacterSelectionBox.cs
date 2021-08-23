using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectionBox : MonoBehaviour
{
    public CharacterBase characterPrefab;
    public CharacterBase character;
    public Image icon;

    public void OnSelect(Toggle toggle)
    {
        if (toggle.isOn)
        {
            CharacterManager.Instance.Place(this);
        }
        else
        {
            CharacterManager.Instance.Delete(this);
        }
    }
}
