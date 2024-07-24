using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoverSpriteChanger : MonoBehaviour
{

    public Sprite[] roverSprites; // all the different rover colors 
    public GameObject rover1; // the rover character 
    // Start is called before the first frame update
    void Start()
    {
        int selectedSprite = PlayerPrefs.GetInt("selectedCharacter");  // gets the selectedCharacter variable from the character selection script 
        rover1.GetComponent<SpriteRenderer>().sprite = roverSprites[selectedSprite]; // sets the sprite to the rover variable shown in character selection 
    }

    
}
