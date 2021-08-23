using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MapGameManager : MonoBehaviour
{
    public static MapGameManager Instance;

    public GameObject battleConfirmation;
    public Text battleConfirmationTitle;

    string mapName;

    void Awake()
    {
        Instance = this;    
    }

    public void BattleConfirmation(string mapName)
    {
        this.mapName = mapName;
        battleConfirmationTitle.text = mapName;
        battleConfirmation.SetActive(true);
    }

    public void StartBattle()
    {
        BattleGameManager.MapName = mapName;
        BackgroundMusicManager.Instance.Play(BackgroundMusicManager.Enviroment.Battle);
        SceneManager.LoadScene("Battle");
    }
}
