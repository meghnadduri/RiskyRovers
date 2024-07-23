using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource audioSource; // Audio that will be adjusted

    // Start is called before the first frame update
    void Start()
    {
        
        if (PlayerPrefs.GetInt("isVolumeChanged") == 1) {
            audioSource.volume = PlayerPrefs.GetFloat("volumeValue");
        }
        else {
            audioSource.volume = 1.0f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
