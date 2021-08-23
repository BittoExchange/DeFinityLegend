using UnityEngine;
using UnityEngine.UI;

public class WorldButton : MonoBehaviour
{
    public void OnClick()
    {
        MapGameManager.Instance.BattleConfirmation(GetComponentInChildren<Text>().text);
    }
}
