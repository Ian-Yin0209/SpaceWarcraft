using UnityEngine;

[System.Serializable]
public class Sounds
{
    public string name;
    public AudioClip audioClip;

    [Range(0f,1f)]
    public float volume;

    public bool loop;

    public AudioSource source;
}
