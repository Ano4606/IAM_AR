using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioController : MonoBehaviour
{
    public static AudioController Instance;

    private AudioSource _audioSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        _audioSource = GetComponent<AudioSource>();
    }

    public void Play()
    {
        _audioSource.Play();
    }

    public void Pause()
    {
        _audioSource.Pause();
    }

    public void UnPause()
    {
        _audioSource.UnPause();
    }
}
