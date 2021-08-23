using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BattleGameManager : MonoBehaviour
{
    public Text worldNameText;
    public AudioSource audioSource;
    public AudioClip victoryClip;
    public AudioClip defeatClip;
    public Text resultText;
    public GameObject resultBox;

    public static string MapName;
    public static BattleGameManager Instance;

    void Awake()
    {
        Instance = this;
        worldNameText.text = MapName;
    }

    public void BackToMap()
    {
        BackgroundMusicManager.Instance.Play(BackgroundMusicManager.Enviroment.Map);
        SceneManager.LoadScene("Map");
    }

    public void Victory()
    {
        audioSource.clip = victoryClip;
        audioSource.Play();
        resultText.text = "VICTORY";
        resultBox.SetActive(true);
    }

    public void Defeat()
    {
        audioSource.clip = defeatClip;
        audioSource.Play();
        resultText.text = "DEFEAT";
        resultBox.SetActive(true);
    }
}
