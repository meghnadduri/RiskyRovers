using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class SkinManager : MonoBehaviour // NOT BEING USED - A REFERENCE SCRIPT 
{

    public SpriteRenderer sr;
    public List<Sprite> skins = new List<Sprite>();
    private int selectedSkin = 0;
    public GameObject playerskin;
    // Start is called before the first frame update
    public void NextOption()
    {
        selectedSkin = selectedSkin + 1;
        if (selectedSkin == skins.Count)
        {
            selectedSkin = 0;
        }
        sr.sprite = skins[selectedSkin];
    }

    // Update is called once per frame
   public void BackOption()
    {
        selectedSkin = selectedSkin - 1;
        if ( selectedSkin < 0)
        {
            selectedSkin = skins.Count - 1;

        }
        sr.sprite = skins[selectedSkin];
    }

    public void PlayGame()
    {
        PrefabUtility.SaveAsPrefabAsset(playerskin, "Assets/selectedskin.prefab");
        
    }

}
