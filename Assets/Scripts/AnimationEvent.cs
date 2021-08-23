using UnityEngine;

public class AnimationEvent : MonoBehaviour
{
    public void Close()
    {
        GetComponent<Animator>().SetTrigger("Close");
    }

    public void SetActiveFalse()
    {
        gameObject.SetActive(false);
    }
}
