using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class OtherScenesMusicManager : MonoBehaviour
{
    private static OtherScenesMusicManager instance;
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
            _PlayMusic(backgroundMusic);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

        if (volumeSlider != null) {

            // Add listener to detect when slider value changes
            volumeSlider.onValueChanged.AddListener(delegate { OnVolumeChanged(); });
        }
        else {

            if (PlayerPrefs.GetInt("isVolumeChanged") == 1) {
                audioSource.volume = PlayerPrefs.GetFloat("volumeValue");
            }
            else {
                audioSource.volume = 1.0f;
            }
        }
        

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
                _PlayMusic(backgroundMusic);
            }
        }
        else
        {
            _StopMusic();
        }
    }

    public void _PlayMusic(AudioClip clip)
    {
        if (audioSource.clip != clip)
        {
            audioSource.clip = clip;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    public void _StopMusic()
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
