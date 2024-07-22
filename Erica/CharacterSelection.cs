using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelection : MonoBehaviour
{

    public GameObject[] characters;// An array that you put all your characters inside (the rovers).
    public int selectedCharacter = 0; // An int that finds the character that is chosen.

    void Start() {
        for (int i = 1; i < characters.Length; i++) {
                characters[i].SetActive(false);// goes through and sets all the characters unactive except the first one. 
        }
    }

    public void NextCharacter()
    {
        characters[selectedCharacter].SetActive(false);// sets the character that was previously loaded to false.
        selectedCharacter = (selectedCharacter + 1) % characters.Length;// moves on in the array to the next option, unless you're at the end 
        characters[selectedCharacter].SetActive(true);// Sets new character to true.
    }

    public void PreviousCharacter()
    {
        characters[selectedCharacter].SetActive(false); // Same thing as the NextCharacter() method, except backwards. 
        selectedCharacter--; 
        if (selectedCharacter < 0)
        {
            selectedCharacter += characters.Length;
        }
        characters[selectedCharacter].SetActive(true);

    }


    public void StartGame() // put on start button
    {
        PlayerPrefs.SetInt("selectedCharacter", selectedCharacter); // sets the selected character to an int that won't change in between scenes. 
       // SceneManager.LoadScene(4, LoadSceneMode.Single);  <-------- // commented out line that is supposed to move you to the next scene, however I find that it is easier just to use the ChangeScene Class at the same time. 
       
    }
    
}
