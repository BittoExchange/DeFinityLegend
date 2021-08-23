using UnityEngine;
using UnityEngine.UI;

public class HabilityBox : MonoBehaviour
{
    public Text text;
    public Image fill;
    public Hability hability;

    public void OnClick()
    {
        BattleManager.Instance.SelectHability(hability);
    }
}