using UnityEngine;

public class AudioManager : MonoBehaviour
{
  // Static instance that any other script can access (e.g., AudioManager.Instance.PlaySFX())
  public static AudioManager Instance { get; private set; }

  [Header("Audio Sources")]
  [SerializeField] private AudioSource musicSource;
  [SerializeField] private AudioSource sfxSource;

  [Header("Background Music Tracks")]
  public AudioClip backgroundMusic;

  void Awake()
  {
    if (Instance == null)
    {
      Instance = this;
      DontDestroyOnLoad(gameObject);
    }
    else
    {
      Destroy(gameObject);
      return;
    }
  }

  void Start()
  {
    if (backgroundMusic != null)
    {
      PlayMusic(backgroundMusic);
    }
  }

  public void PlayMusic(AudioClip clip)
  {
    if (musicSource == null || clip == null) return;

    musicSource.clip = clip;
    musicSource.loop = true;
    musicSource.Play();
  }

  public void PlaySFX(AudioClip clip, float volume = 1f)
  {
    if (sfxSource == null || clip == null) return;

    sfxSource.PlayOneShot(clip, volume);
  }
}