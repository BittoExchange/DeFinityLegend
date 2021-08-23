using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusicManager : MonoBehaviour
{
    public enum Enviroment { Map, Battle }

    public float fadeTime;
    public AudioSource audioSource1;
    public AudioSource audioSource2;
    public AudioClip mapClip;
    public AudioClip battleClip;

    bool selection;

    public static BackgroundMusicManager Instance;

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Play(Enviroment enviroment)
    {
        switch (enviroment)
        {
            case Enviroment.Map:
                StartCoroutine(SwitchMusic(mapClip));
                break;
            case Enviroment.Battle:
                StartCoroutine(SwitchMusic(battleClip));
                break;
        }
    }

    IEnumerator SwitchMusic(AudioClip clip)
    {
        AudioSource from;
        AudioSource to;

        from = selection ? audioSource1 : audioSource2;
        to = selection ? audioSource2 : audioSource1;

        selection = !selection;

        float t = 0;

        to.clip = clip;
        to.Play();

        while(t < 1)
        {
            t += Time.deltaTime / fadeTime;

            from.volume = Mathf.Lerp(1, 0, t);
            to.volume = Mathf.Lerp(0, 1, t);

            yield return null;
        }

        from.Stop();
    }
}