using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Sound
{
    public string key;
    public AudioClip clip;
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private AudioSource SFXSource;
    [SerializeField] private List<Sound> sounds = new List<Sound>();
    private Dictionary<string, AudioClip> soundLibrary = new Dictionary<string, AudioClip>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeSoundLibrary();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeSoundLibrary()
    {
        foreach (Sound sound in sounds)
        {
            if (sound.clip != null && !soundLibrary.ContainsKey(sound.key))
            {
                soundLibrary.Add(sound.key, sound.clip);
            }
        }
    }

    public void Play(string key)
    {
        if (soundLibrary.TryGetValue(key, out AudioClip clip)) SFXSource.PlayOneShot(clip);
    }
}