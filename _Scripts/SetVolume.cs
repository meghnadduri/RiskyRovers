using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetVolume : MonoBehaviour
{
    public AudioSource audioSource; // Reference to the audio
    public Slider volumeSlider; // Reference to the volume slider
    public float volumeValue; // Value that will be entered into the "volume" field of the audio component

    // Start is called before the first frame update
    void Start()
    {
        //volumeSlider.onValueChanged.AddListener(setVol);
    }

    public void setVol() {
        volumeValue = volumeSlider.value;

        audioSource.volume = volumeValue;
        PlayerPrefs.SetFloat("volumeValue", volumeValue);
    }
}
