using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetVolume : MonoBehaviour
{
    public Slider volumeSlider; // Reference to the fov slider
    public float volumeValue; // Value that will be entered into the "size" field of the main camera
    public AudioSource audioSource; // Audio that will be adjusted

    // Start is called before the first frame update
    void Start()
    {
        volumeSlider.value = audioSource.volume;

        // Add listener to detect when slider value changes
        volumeSlider.onValueChanged.AddListener(delegate { OnVolumeChanged(); });
    }

    public void OnVolumeChanged() {
        // Change the volume based on what value the slider was changed to
        audioSource.volume = volumeSlider.value;

        // Set the volume using "PlayerPrefs" so it can be set across scenes
        PlayerPrefs.SetFloat("volumeValue", volumeSlider.value);

        // Set isVolumeChanged to true (1), so the other scenes know to refer to the PlayerPrefs volume instead of the default
        PlayerPrefs.SetInt("isVolumeChanged", 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
