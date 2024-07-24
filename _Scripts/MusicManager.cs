using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MusicManager : MonoBehaviour
{
    private static MusicManager instance;
    private AudioSource audioSource;

    public AudioClip backgroundMusic;
    public string[] seamlessScenes;

    public Slider volumeSlider;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            audioSource = GetComponent<AudioSource>();
            PlayMusic(backgroundMusic);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

        volumeSlider.onValueChanged.AddListener(delegate { OnVolumeChanged(); });
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (System.Array.Exists(seamlessScenes, element => element == scene.name))
        {
            if (!audioSource.isPlaying)
            {
                PlayMusic(backgroundMusic);
            }
        }
        else
        {
            StopMusic();
        }
    }

    public void PlayMusic(AudioClip clip)
    {
        if (audioSource.clip != clip)
        {
            audioSource.clip = clip;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    public void StopMusic()
    {
        audioSource.Stop();
    }

    public void OnVolumeChanged() {
        // Change the volume based on what value the slider was changed to
        audioSource.volume = volumeSlider.value;

        // Set the volume using "PlayerPrefs" so it can be set across scenes
        PlayerPrefs.SetFloat("volumeValue", volumeSlider.value);

        // Set isVolumeChanged to true (1), so the other scenes know to refer to the PlayerPrefs volume instead of the default
        PlayerPrefs.SetInt("isVolumeChanged", 1);
    }
}
