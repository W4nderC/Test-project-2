using UnityEngine;

public enum SoundType
{
    PUNCH,
    HURT,
    FOOTSTEPS,
}

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField] private AudioClip[] soundList;
    private AudioSource audioSource;

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
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(SoundType sound, Transform transform, float volume = 1f)
    {
        // Instance.audioSource.PlayOneShot(Instance.soundList[(int)sound], volume);
        AudioSource.PlayClipAtPoint(soundList[(int)sound], transform.position, volume);
    }
}
