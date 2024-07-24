using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ExperimentImage : MonoBehaviour
{
    public GameObject[] rockList; // list of the special rocks
    //public Sprite rock5;
    public GameObject defaultImage; // the white box
    //public Image dataImage;
    public bool activeS; // not relevant 

    void Start() {
        ResetImage(); // sets image to the white box 
    }

    public void ChangeImage(int num) {

        defaultImage.SetActive(false); // sets the white image to false 
        for(int i = 0; i < rockList.Length; i++) {
            if (i == num) { // cycles through to find the image matching the rock 
                rockList[i].SetActive(true);
               
            }
        }




       /* for(int i = 0; i < rockList.Length; i++) {
            if(i == num) {
                dataImage.GetComponent<Image>().sprite = rockList[i];
            }
        }


        switch(num) {
            case 4:
               
                dataImage.GetComponent<Image>().sprite = rockList[0];
            break;

            case 5:
                dataImage.GetComponent<Image>().sprite = rockList[1];
            break;

            default:
           
                dataImage.GetComponent<Image>().sprite = defaultImage;
            break;

        }*/
    }

    public void ResetImage() { // sets all images to false and sets white box to active 
       // dataImage.GetComponent<Image>().sprite = defaultImage;
        for(int i = 0; i < rockList.Length; i++) {
            rockList[i].SetActive(false);
        }
        defaultImage.SetActive(true);
    }
}

